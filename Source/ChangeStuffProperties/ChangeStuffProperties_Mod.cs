using System;
using System.Collections.Generic;
using System.Linq;
using Mlie;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace ChangeStuffProperties.Settings;

public class ChangeStuffProperties_Mod : Mod
{
    /// <summary>
    ///     The instance of the settings to be read by the mod
    /// </summary>
    public static ChangeStuffProperties_Mod instance;

    public static readonly Vector2 buttonSize = new Vector2(100f, 25f);

    private static readonly Vector2 searchSize = new Vector2(175f, 25f);

    private static readonly Vector2 iconSize = new Vector2(48f, 48f);

    private static readonly int buttonSpacer = 200;

    private static readonly float columnSpacer = 0.1f;

    private static float leftSideWidth;

    private static Listing_Standard listing_Standard;

    private static Vector2 tabsScrollPosition;

    private static string currentVersion;

    private static Vector2 scrollPosition;

    private static string searchText = "";

    public static List<string> CurrentTags;
    public static string TagStage;

    private static readonly Color alternateBackground = new Color(0.1f, 0.1f, 0.1f, 0.5f);

    private static readonly List<string> settingTabs = new List<string>
    {
        "Settings",
        null,
        "Commonality",
        "Mass",
        "MarketValue"
    };

    /// <summary>
    ///     The private settings
    /// </summary>
    private ChangeStuffProperties_Settings settings;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="content"></param>
    public ChangeStuffProperties_Mod(ModContentPack content)
        : base(content)
    {
        instance = this;

        SelectedDef = "Settings";
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(
                ModLister.GetActiveModWithIdentifier("Mlie.ChangeStuffProperties"));
    }

    /// <summary>
    ///     The instance-settings for the mod
    /// </summary>
    internal ChangeStuffProperties_Settings Settings
    {
        get
        {
            if (settings == null)
            {
                settings = GetSettings<ChangeStuffProperties_Settings>();
            }

            return settings;
        }

        set => settings = value;
    }

    public string SelectedDef { get; set; }

    public override void WriteSettings()
    {
        base.WriteSettings();
        SelectedDef = "Settings";
    }


    /// <summary>
    ///     The settings-window
    /// </summary>
    /// <param name="rect"></param>
    public override void DoSettingsWindowContents(Rect rect)
    {
        base.DoSettingsWindowContents(rect);

        var rect2 = rect.ContractedBy(1);
        leftSideWidth = rect2.ContractedBy(10).width / 4;

        listing_Standard = new Listing_Standard();

        DrawOptions(rect2);
        DrawTabsList(rect2);
        Settings.Write();
    }

    /// <summary>
    ///     The title for the mod-settings
    /// </summary>
    /// <returns></returns>
    public override string SettingsCategory()
    {
        return "Change Stuff Properties";
    }


    private static void DrawButton(Action action, string text, Vector2 pos)
    {
        var rect = new Rect(pos.x, pos.y, buttonSize.x, buttonSize.y);
        if (!Widgets.ButtonText(rect, text, true, false, Color.white))
        {
            return;
        }

        SoundDefOf.Designate_DragStandard_Changed.PlayOneShotOnCamera();
        action();
    }


    private void DrawIcon(ThingDef thing, Rect rect)
    {
        Main.LogMessage($"Draw icon for {thing}");
        if (thing == null)
        {
            return;
        }

        var texture = thing.graphicData?.Graphic?.MatSingle?.mainTexture;
        if (thing.graphicData?.graphicClass == typeof(Graphic_Random))
        {
            texture = ((Graphic_Random)thing.graphicData.Graphic)?.FirstSubgraphic().MatSingle.mainTexture;
        }

        if (thing.graphicData?.graphicClass == typeof(Graphic_StackCount))
        {
            texture = ((Graphic_StackCount)thing.graphicData.Graphic)?.SubGraphicForStackCount(1, thing).MatSingle
                .mainTexture;
        }

        if (texture == null)
        {
            return;
        }

        var toolTip = $"{thing.LabelCap}\n{thing.description}";
        if (texture.width != texture.height)
        {
            var ratio = (float)texture.width / texture.height;

            if (ratio < 1)
            {
                rect.x += (rect.width - (rect.width * ratio)) / 2;
                rect.width *= ratio;
            }
            else
            {
                rect.y += (rect.height - (rect.height / ratio)) / 2;
                rect.height /= ratio;
            }
        }

        if (thing.graphicData?.color != null)
        {
            GUI.DrawTexture(rect, texture, ScaleMode.ScaleToFit, true, 1f, thing.graphicData.color, Vector4.zero,
                Vector4.zero);
        }
        else
        {
            GUI.DrawTexture(rect, texture);
        }

        TooltipHandler.TipRegion(rect, toolTip);
    }

    private void DrawOptions(Rect rect)
    {
        var optionsOuterContainer = rect.ContractedBy(10);
        optionsOuterContainer.x += leftSideWidth + columnSpacer;
        optionsOuterContainer.width -= leftSideWidth + columnSpacer;
        Widgets.DrawBoxSolid(optionsOuterContainer, Color.grey);
        var optionsInnerContainer = optionsOuterContainer.ContractedBy(1);
        Widgets.DrawBoxSolid(optionsInnerContainer, new ColorInt(42, 43, 44).ToColor);
        var frameRect = optionsInnerContainer.ContractedBy(10);
        frameRect.x = leftSideWidth + columnSpacer + 20;
        frameRect.y += 15;
        frameRect.height -= 15;
        var contentRect = frameRect;
        contentRect.x = 0;
        contentRect.y = 0;
        switch (SelectedDef)
        {
            case null:
                return;
            case "Settings":
            {
                listing_Standard.Begin(frameRect);
                Text.Font = GameFont.Medium;
                listing_Standard.Label("CSP.settings".Translate());
                Text.Font = GameFont.Small;
                listing_Standard.Gap();

                if (instance.Settings.HasCustomValues())
                {
                    var labelPoint = listing_Standard.Label("CSP.resetall.label".Translate(), -1F,
                        "CSP.resetall.tooltip".Translate());
                    DrawButton(() =>
                        {
                            Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                                "CSP.resetall.confirm".Translate(),
                                delegate { instance.Settings.ResetValues("all"); }));
                        }, "CSP.resetall.button".Translate(),
                        new Vector2(labelPoint.position.x + buttonSpacer, labelPoint.position.y));
                }

                listing_Standard.CheckboxLabeled("CSP.logging.label".Translate(), ref Settings.VerboseLogging,
                    "CSP.logging.tooltip".Translate());
                if (currentVersion != null)
                {
                    listing_Standard.Gap();
                    GUI.contentColor = Color.gray;
                    listing_Standard.Label("CSP.version.label".Translate(currentVersion));
                    GUI.contentColor = Color.white;
                }

                listing_Standard.End();
                break;
            }
            case "Commonality":
            {
                FloatScrollView(ref frameRect, ref instance.Settings.CustomCommonality,
                    Commonality.VanillaCommonalities, "commonality");
                break;
            }
            case "Mass":
            {
                FloatScrollView(ref frameRect, ref instance.Settings.CustomMass,
                    Mass.VanillaMasses, "mass");
                break;
            }
            case "MarketValue":
            {
                FloatScrollView(ref frameRect, ref instance.Settings.CustomMarketValue,
                    MarketValue.VanillaMarketValues, "marketvalue");
                break;
            }
        }
    }


    private void FloatScrollView(ref Rect frameRect, ref Dictionary<string, float> modifiedValues,
        Dictionary<string, float> vanillaValues, string header)
    {
        listing_Standard.Begin(frameRect);

        Text.Font = GameFont.Medium;

        var headerLabel = listing_Standard.Label($"CSP.{header}".Translate());
        TooltipHandler.TipRegion(new Rect(
            headerLabel.position,
            searchSize), $"CSP.{header}.tooltip".Translate());

        if (modifiedValues == null)
        {
            modifiedValues = new Dictionary<string, float>();
        }

        if (modifiedValues.Any())
        {
            DrawButton(() =>
                {
                    Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                        "CSP.resetone.confirm".Translate($"CSP.{header}".Translate().ToLower()),
                        delegate { instance.Settings.ResetValues(header); }));
                }, "CSP.reset.button".Translate(),
                new Vector2(headerLabel.position.x + headerLabel.width - buttonSize.x,
                    headerLabel.position.y));
        }

        Text.Font = GameFont.Small;

        searchText =
            Widgets.TextField(
                new Rect(headerLabel.position + new Vector2((frameRect.width / 3 * 2) - (searchSize.x / 2), 0),
                    searchSize),
                searchText);
        TooltipHandler.TipRegion(new Rect(
            headerLabel.position + new Vector2((frameRect.width / 3 * 2) - (searchSize.x / 2), 0),
            searchSize), "CSP.search".Translate());

        listing_Standard.End();

        var allStuff = Main.AllStuff;
        if (!string.IsNullOrEmpty(searchText))
        {
            allStuff = Main.AllStuff.Where(def =>
                    def.label.ToLower().Contains(searchText.ToLower()) || def.modContentPack.Name.ToLower()
                        .Contains(searchText.ToLower()) || def.defName.ToLower()
                        .Contains(searchText.ToLower()))
                .ToList();
        }

        var borderRect = frameRect;
        borderRect.y += headerLabel.y + 40;
        borderRect.height -= headerLabel.y + 40;
        var scrollContentRect = frameRect;
        scrollContentRect.height = allStuff.Count * 51f;
        scrollContentRect.width -= 20;
        scrollContentRect.x = 0;
        scrollContentRect.y = 0;

        var scrollListing = new Listing_Standard();
        Widgets.BeginScrollView(borderRect, ref scrollPosition, scrollContentRect);
        scrollListing.Begin(scrollContentRect);
        var alternate = false;
        foreach (var thingDef in allStuff)
        {
            var modInfo = thingDef.modContentPack?.Name;
            var rowRect = scrollListing.GetRect(50);
            alternate = !alternate;
            if (alternate)
            {
                Widgets.DrawBoxSolid(rowRect, alternateBackground);
            }

            var sliderRect = rowRect;
            sliderRect.x += iconSize.x;
            sliderRect.width -= iconSize.x;
            DrawIcon(thingDef, new Rect(rowRect.position, iconSize));

            var thingLabel = $"{thingDef.label.CapitalizeFirst()} ({thingDef.defName})";
            if (thingLabel.Length > 45)
            {
                thingLabel = $"{thingLabel.Substring(0, 42)}...";
            }

            if (modInfo is { Length: > 45 })
            {
                modInfo = $"{modInfo.Substring(0, 42)}...";
            }

            switch (header)
            {
                case "commonality":
                    if (thingDef.stuffProps.commonality !=
                        vanillaValues[thingDef.defName])
                    {
                        modifiedValues[thingDef.defName] =
                            thingDef.stuffProps.commonality;
                        GUI.color = Color.green;
                    }
                    else
                    {
                        if (modifiedValues.ContainsKey(thingDef.defName))
                        {
                            modifiedValues.Remove(thingDef.defName);
                        }
                    }

                    thingDef.stuffProps.commonality =
                        (float)Math.Round((decimal)Widgets.HorizontalSlider(
                            sliderRect,
                            thingDef.stuffProps.commonality, 0.001f,
                            2.5f, false,
                            thingDef.stuffProps.commonality.ToStringPercent(),
                            thingLabel,
                            modInfo), 4);
                    break;
                case "mass":
                    if (thingDef.BaseMass !=
                        vanillaValues[thingDef.defName])
                    {
                        modifiedValues[thingDef.defName] =
                            thingDef.BaseMass;
                        GUI.color = Color.green;
                    }
                    else
                    {
                        if (modifiedValues.ContainsKey(thingDef.defName))
                        {
                            modifiedValues.Remove(thingDef.defName);
                        }
                    }

                    thingDef.SetStatBaseValue(StatDefOf.Mass,
                        (float)Math.Round((decimal)Widgets.HorizontalSlider(
                            sliderRect,
                            thingDef.BaseMass, 0.001f,
                            2.5f, false,
                            thingDef.BaseMass.ToStringMass(),
                            thingLabel,
                            modInfo), 4));
                    break;
                case "marketvalue":
                    if (thingDef.BaseMarketValue !=
                        vanillaValues[thingDef.defName])
                    {
                        modifiedValues[thingDef.defName] =
                            thingDef.BaseMarketValue;
                        GUI.color = Color.green;
                    }
                    else
                    {
                        if (modifiedValues.ContainsKey(thingDef.defName))
                        {
                            modifiedValues.Remove(thingDef.defName);
                        }
                    }

                    thingDef.SetStatBaseValue(StatDefOf.MarketValue,
                        (float)Math.Round((decimal)Widgets.HorizontalSlider(
                            sliderRect,
                            thingDef.BaseMarketValue, 0.1f,
                            20f, false,
                            thingDef.BaseMarketValue.ToStringMoney(),
                            thingLabel,
                            modInfo), 2));
                    break;
            }

            GUI.color = Color.white;
        }

        scrollListing.End();

        Widgets.EndScrollView();
    }


    private void DrawTabsList(Rect rect)
    {
        var scrollContainer = rect.ContractedBy(10);
        scrollContainer.width = leftSideWidth;
        Widgets.DrawBoxSolid(scrollContainer, Color.grey);
        var innerContainer = scrollContainer.ContractedBy(1);
        Widgets.DrawBoxSolid(innerContainer, new ColorInt(42, 43, 44).ToColor);
        var tabFrameRect = innerContainer.ContractedBy(5);
        tabFrameRect.y += 15;
        tabFrameRect.height -= 15;
        var tabContentRect = tabFrameRect;
        tabContentRect.x = 0;
        tabContentRect.y = 0;
        tabContentRect.width -= 20;

        var listAddition = 24;


        tabContentRect.height = (settingTabs.Count * 25f) + listAddition;
        Widgets.BeginScrollView(tabFrameRect, ref tabsScrollPosition, tabContentRect);
        listing_Standard.Begin(tabContentRect);
        //Text.Font = GameFont.Tiny;
        foreach (var settingTab in settingTabs)
        {
            if (string.IsNullOrEmpty(settingTab))
            {
                listing_Standard.ListItemSelectable(null, Color.yellow, out _);
                continue;
            }

            if (instance.Settings.HasCustomValues(settingTab.ToLower()))
            {
                GUI.color = Color.green;
            }

            if (listing_Standard.ListItemSelectable($"CSP.{settingTab.ToLower()}".Translate(), Color.yellow,
                    out _, SelectedDef == settingTab))
            {
                SelectedDef = SelectedDef == settingTab ? null : settingTab;
            }

            GUI.color = Color.white;
        }

        listing_Standard.End();
        //Text.Font = GameFont.Small;
        Widgets.EndScrollView();
    }
}