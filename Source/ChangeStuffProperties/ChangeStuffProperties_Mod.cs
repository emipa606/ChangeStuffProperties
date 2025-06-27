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

    private static readonly Vector2 buttonSize = new(100f, 25f);

    private static readonly Vector2 searchSize = new(175f, 25f);

    private static readonly Vector2 iconSize = new(48f, 48f);

    private static readonly int buttonSpacer = 200;

    private static readonly float columnSpacer = 0.1f;

    private static float leftSideWidth;

    private static Listing_Standard listing_Standard;

    private static Vector2 tabsScrollPosition;

    private static string currentVersion;

    private static Vector2 scrollPosition;

    private static string searchText = "";

    private static readonly Color alternateBackground = new(0.1f, 0.1f, 0.1f, 0.5f);

    private static readonly List<string> settingTabs =
    [
        "Settings",
        null,
        "Commonality",
        "Mass",
        "MarketValue",
        "CleanlinessOffset",
        "BeautyOffset",
        "BeautyMultiplier",
        "WorkToBuildMultiplier",
        "WorkToMakeMultiplier",
        "SharpDamageMultiplier",
        "BluntDamageMultiplier",
        "StuffPowerArmorSharp",
        "StuffPowerArmorBlunt",
        "StuffPowerArmorHeat",
        "StuffPowerInsulationHeat",
        "StuffPowerInsulationCold",
        "Flammability",
        "FlammabilityFactor",
        "MaxHitPointsFactor"
    ];

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="content"></param>
    public ChangeStuffProperties_Mod(ModContentPack content)
        : base(content)
    {
        instance = this;
        Settings = GetSettings<ChangeStuffProperties_Settings>();
        SelectedDef = "Settings";
        currentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    /// <summary>
    ///     The instance-settings for the mod
    /// </summary>
    internal ChangeStuffProperties_Settings Settings { get; }

    private string SelectedDef { get; set; }

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

        drawOptions(rect2);
        drawTabsList(rect2);
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


    private static void drawButton(Action action, string text, Vector2 pos)
    {
        var rect = new Rect(pos.x, pos.y, buttonSize.x, buttonSize.y);
        if (!Widgets.ButtonText(rect, text, true, false, Color.white))
        {
            return;
        }

        SoundDefOf.Designate_DragStandard_Changed.PlayOneShotOnCamera();
        action();
    }


    private static void drawIcon(ThingDef thing, Rect rect)
    {
        if (thing == null)
        {
            return;
        }

        Texture texture = BaseContent.BadTex;
        try
        {
            texture = thing.graphicData?.Graphic?.MatSingle?.mainTexture;
            if (thing.graphicData?.graphicClass == typeof(Graphic_Random))
            {
                texture = ((Graphic_Random)thing.graphicData.Graphic)?.FirstSubgraphic()?.MatSingle?.mainTexture;
            }

            if (thing.graphicData?.graphicClass == typeof(Graphic_StackCount))
            {
                texture = ((Graphic_StackCount)thing.graphicData.Graphic)?.SubGraphicForStackCount(1, thing)?.MatSingle?
                    .mainTexture;
            }
        }
        catch (Exception)
        {
            // ignored
        }

        if (texture == null)
        {
            texture = BaseContent.BadTex;
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

    private void drawOptions(Rect rect)
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
                    drawButton(() =>
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
            case "CleanlinessOffset":
            {
                FloatScrollView(ref frameRect, ref instance.Settings.CustomCleanlinessOffsets,
                    CleanlinessOffset.VanillaCleanlinessOffsets, "cleanlinessoffset");
                break;
            }
            case "BeautyOffset":
            {
                FloatScrollView(ref frameRect, ref instance.Settings.CustomBeautyOffsets,
                    BeautyOffset.VanillaBeautyOffsets, "beautyoffset");
                break;
            }
            case "BeautyMultiplier":
            {
                FloatScrollView(ref frameRect, ref instance.Settings.CustomBeautyMultipliers,
                    BeautyMultiplier.VanillaBeautyMultipliers, "beautymultiplier");
                break;
            }
            case "WorkToBuildMultiplier":
            {
                FloatScrollView(ref frameRect, ref instance.Settings.CustomWorkToBuildMultipliers,
                    WorkToBuildMultiplier.VanillaWorkToBuildMultipliers, "worktobuildmultiplier");
                break;
            }
            case "WorkToMakeMultiplier":
            {
                FloatScrollView(ref frameRect, ref instance.Settings.CustomWorkToMakeMultipliers,
                    WorkToMakeMultiplier.VanillaWorkToMakeMultipliers, "worktomakemultiplier");
                break;
            }
            case "SharpDamageMultiplier":
            {
                FloatScrollView(ref frameRect, ref instance.Settings.CustomSharpDamageMultiplier,
                    SharpDamageMultiplier.VanillaSharpDamageMultiplier, "sharpdamagemultiplier");
                break;
            }
            case "BluntDamageMultiplier":
            {
                FloatScrollView(ref frameRect, ref instance.Settings.CustomBluntDamageMultiplier,
                    BluntDamageMultiplier.VanillaBluntDamageMultiplier, "bluntdamagemultiplier");
                break;
            }
            case "StuffPowerArmorSharp":
            {
                FloatScrollView(ref frameRect, ref instance.Settings.CustomStuffPower_Armor_Sharp,
                    StuffPower_Armor_Sharp.VanillaStuffPower_Armor_Sharp, "stuffpowerarmorsharp");
                break;
            }
            case "StuffPowerArmorBlunt":
            {
                FloatScrollView(ref frameRect, ref instance.Settings.CustomStuffPower_Armor_Blunt,
                    StuffPower_Armor_Blunt.VanillaStuffPower_Armor_Blunt, "stuffpowerarmorblunt");
                break;
            }
            case "StuffPowerArmorHeat":
            {
                FloatScrollView(ref frameRect, ref instance.Settings.CustomStuffPower_Armor_Heat,
                    StuffPower_Armor_Heat.VanillaStuffPower_Armor_Heat, "stuffpowerarmorheat");
                break;
            }
            case "StuffPowerInsulationHeat":
            {
                FloatScrollView(ref frameRect, ref instance.Settings.CustomStuffPower_Insulation_Heat,
                    StuffPower_Insulation_Heat.VanillaStuffPower_Insulation_Heat, "stuffpowerinsulationheat");
                break;
            }
            case "StuffPowerInsulationCold":
            {
                FloatScrollView(ref frameRect, ref instance.Settings.CustomStuffPower_Insulation_Heat,
                    StuffPower_Insulation_Cold.VanillaStuffPower_Insulation_Cold, "stuffpowerinsulationcold");
                break;
            }
            case "Flammability":
            {
                FloatScrollView(ref frameRect, ref instance.Settings.CustomFlammability,
                    Flammability.VanillaFlammability, "flammability");
                break;
            }
            case "FlammabilityFactor":
            {
                FloatScrollView(ref frameRect, ref instance.Settings.CustomFlammabilityFactor,
                    FlammabilityFactor.VanillaFlammabilityFactor, "flammabilityfactor");
                break;
            }
            case "MaxHitPointsFactor":
            {
                FloatScrollView(ref frameRect, ref instance.Settings.CustomMaxHitPointsFactor,
                    MaxHitPointsFactor.VanillaMaxHitPointsFactor, "maxhitpointsfactor");
                break;
            }
        }
    }


    private static void FloatScrollView(ref Rect frameRect, ref Dictionary<string, float> modifiedValues,
        Dictionary<string, float> vanillaValues, string header)
    {
        listing_Standard.Begin(frameRect);

        Text.Font = GameFont.Medium;

        var headerLabel = listing_Standard.Label($"CSP.{header}".Translate());
        TooltipHandler.TipRegion(new Rect(
            headerLabel.position,
            searchSize), $"CSP.{header}.tooltip".Translate());

        modifiedValues ??= new Dictionary<string, float>();

        if (modifiedValues.Any())
        {
            drawButton(() =>
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
                    def.label.ToLower().Contains(searchText.ToLower()) || def.modContentPack?.Name.ToLower()
                        .Contains(searchText.ToLower()) == true || def.defName.ToLower()
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
            drawIcon(thingDef, new Rect(rowRect.position, iconSize));

            var thingLabel = $"{thingDef.label.CapitalizeFirst()} ({thingDef.defName})";
            if (thingLabel.Length > 45)
            {
                thingLabel = $"{thingLabel[..42]}...";
            }

            if (modInfo is { Length: > 45 })
            {
                modInfo = $"{modInfo[..42]}...";
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
                        modifiedValues.Remove(thingDef.defName);
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
                        modifiedValues.Remove(thingDef.defName);
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
                        modifiedValues.Remove(thingDef.defName);
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
                case "cleanlinessoffset":
                    if (thingDef.stuffProps.statOffsets.GetStatOffsetFromList(StatDefOf.Cleanliness) !=
                        vanillaValues[thingDef.defName])
                    {
                        modifiedValues[thingDef.defName] =
                            thingDef.stuffProps.statOffsets.GetStatOffsetFromList(StatDefOf.Cleanliness);
                        GUI.color = Color.green;
                    }
                    else
                    {
                        modifiedValues.Remove(thingDef.defName);
                    }

                    thingDef.stuffProps.statOffsets ??= [];

                    if (thingDef.stuffProps.statOffsets.All(modifier => modifier.stat != StatDefOf.Cleanliness))
                    {
                        thingDef.stuffProps.statOffsets.Add(
                            new StatModifier { stat = StatDefOf.Cleanliness, value = 0 });
                    }

                    thingDef.stuffProps.statOffsets.First(modifier => modifier.stat == StatDefOf.Cleanliness).value =
                        (float)Math.Round((decimal)Widgets.HorizontalSlider(
                            sliderRect,
                            thingDef.stuffProps.statOffsets.GetStatOffsetFromList(StatDefOf.Cleanliness), 0,
                            5f, false,
                            thingDef.stuffProps.statOffsets.GetStatOffsetFromList(StatDefOf.Cleanliness)
                                .ToStringDecimalIfSmall(),
                            thingLabel,
                            modInfo), 4);
                    break;
                case "beautyoffset":
                    if (thingDef.stuffProps.statOffsets.GetStatOffsetFromList(StatDefOf.Beauty) !=
                        vanillaValues[thingDef.defName])
                    {
                        modifiedValues[thingDef.defName] =
                            thingDef.stuffProps.statOffsets.GetStatOffsetFromList(StatDefOf.Beauty);
                        GUI.color = Color.green;
                    }
                    else
                    {
                        modifiedValues.Remove(thingDef.defName);
                    }

                    thingDef.stuffProps.statOffsets ??= [];

                    if (thingDef.stuffProps.statOffsets.All(modifier => modifier.stat != StatDefOf.Beauty))
                    {
                        thingDef.stuffProps.statOffsets.Add(new StatModifier { stat = StatDefOf.Beauty, value = 0 });
                    }

                    thingDef.stuffProps.statOffsets.First(modifier => modifier.stat == StatDefOf.Beauty).value =
                        (float)Math.Round((decimal)Widgets.HorizontalSlider(
                            sliderRect,
                            thingDef.stuffProps.statOffsets.GetStatOffsetFromList(StatDefOf.Beauty), 0,
                            40f, false,
                            thingDef.stuffProps.statOffsets.GetStatOffsetFromList(StatDefOf.Beauty)
                                .ToStringDecimalIfSmall(),
                            thingLabel,
                            modInfo), 4);
                    break;
                case "beautymultiplier":
                    if (thingDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.Beauty) !=
                        vanillaValues[thingDef.defName])
                    {
                        modifiedValues[thingDef.defName] =
                            thingDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.Beauty);
                        GUI.color = Color.green;
                    }
                    else
                    {
                        modifiedValues.Remove(thingDef.defName);
                    }

                    thingDef.stuffProps.statFactors ??= [];

                    if (thingDef.stuffProps.statFactors.All(modifier => modifier.stat != StatDefOf.Beauty))
                    {
                        thingDef.stuffProps.statFactors.Add(new StatModifier { stat = StatDefOf.Beauty, value = 1f });
                    }

                    thingDef.stuffProps.statFactors.First(modifier => modifier.stat == StatDefOf.Beauty).value =
                        (float)Math.Round((decimal)Widgets.HorizontalSlider(
                            sliderRect,
                            thingDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.Beauty), 0,
                            6.5f, false,
                            thingDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.Beauty)
                                .ToStringPercent(),
                            thingLabel,
                            modInfo), 4);
                    break;
                case "worktobuildmultiplier":
                    if (thingDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.WorkToBuild) !=
                        vanillaValues[thingDef.defName])
                    {
                        modifiedValues[thingDef.defName] =
                            thingDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.WorkToBuild);
                        GUI.color = Color.green;
                    }
                    else
                    {
                        modifiedValues.Remove(thingDef.defName);
                    }

                    thingDef.stuffProps.statFactors ??= [];

                    if (thingDef.stuffProps.statFactors.All(modifier => modifier.stat != StatDefOf.WorkToBuild))
                    {
                        thingDef.stuffProps.statFactors.Add(new StatModifier
                            { stat = StatDefOf.WorkToBuild, value = 1f });
                    }

                    thingDef.stuffProps.statFactors.First(modifier => modifier.stat == StatDefOf.WorkToBuild).value =
                        (float)Math.Round((decimal)Widgets.HorizontalSlider(
                            sliderRect,
                            thingDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.WorkToBuild), 0,
                            10f, false,
                            thingDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.WorkToBuild)
                                .ToStringPercent(),
                            thingLabel,
                            modInfo), 4);
                    break;
                case "worktomakemultiplier":
                    if (thingDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.WorkToMake) !=
                        vanillaValues[thingDef.defName])
                    {
                        modifiedValues[thingDef.defName] =
                            thingDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.WorkToMake);
                        GUI.color = Color.green;
                    }
                    else
                    {
                        modifiedValues.Remove(thingDef.defName);
                    }

                    thingDef.stuffProps.statFactors ??= [];

                    if (thingDef.stuffProps.statFactors.All(modifier => modifier.stat != StatDefOf.WorkToMake))
                    {
                        thingDef.stuffProps.statFactors.Add(
                            new StatModifier { stat = StatDefOf.WorkToMake, value = 1f });
                    }

                    thingDef.stuffProps.statFactors.First(modifier => modifier.stat == StatDefOf.WorkToMake).value =
                        (float)Math.Round((decimal)Widgets.HorizontalSlider(
                            sliderRect,
                            thingDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.WorkToMake), 0,
                            6.5f, false,
                            thingDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.WorkToMake)
                                .ToStringPercent(),
                            thingLabel,
                            modInfo), 4);
                    break;
                case "sharpdamagemultiplier":
                    if (thingDef.GetStatValueAbstract(StatDefOf.SharpDamageMultiplier) !=
                        vanillaValues[thingDef.defName])
                    {
                        modifiedValues[thingDef.defName] =
                            thingDef.GetStatValueAbstract(StatDefOf.SharpDamageMultiplier);
                        GUI.color = Color.green;
                    }
                    else
                    {
                        modifiedValues.Remove(thingDef.defName);
                    }

                    thingDef.SetStatBaseValue(StatDefOf.SharpDamageMultiplier,
                        (float)Math.Round((decimal)Widgets.HorizontalSlider(
                            sliderRect,
                            thingDef.GetStatValueAbstract(StatDefOf.SharpDamageMultiplier), 0,
                            2.5f, false,
                            thingDef.GetStatValueAbstract(StatDefOf.SharpDamageMultiplier)
                                .ToStringPercent(),
                            thingLabel,
                            modInfo), 4));
                    break;
                case "bluntdamagemultiplier":
                    if (thingDef.GetStatValueAbstract(StatDefOf.BluntDamageMultiplier) !=
                        vanillaValues[thingDef.defName])
                    {
                        modifiedValues[thingDef.defName] =
                            thingDef.GetStatValueAbstract(StatDefOf.BluntDamageMultiplier);
                        GUI.color = Color.green;
                    }
                    else
                    {
                        modifiedValues.Remove(thingDef.defName);
                    }

                    thingDef.SetStatBaseValue(StatDefOf.BluntDamageMultiplier,
                        (float)Math.Round((decimal)Widgets.HorizontalSlider(
                            sliderRect,
                            thingDef.GetStatValueAbstract(StatDefOf.BluntDamageMultiplier), 0,
                            2.5f, false,
                            thingDef.GetStatValueAbstract(StatDefOf.BluntDamageMultiplier)
                                .ToStringPercent(),
                            thingLabel,
                            modInfo), 4));
                    break;
                case "stuffpowerarmorsharp":
                    if (thingDef.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Sharp) !=
                        vanillaValues[thingDef.defName])
                    {
                        modifiedValues[thingDef.defName] =
                            thingDef.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Sharp);
                        GUI.color = Color.green;
                    }
                    else
                    {
                        modifiedValues.Remove(thingDef.defName);
                    }

                    thingDef.SetStatBaseValue(StatDefOf.StuffPower_Armor_Sharp,
                        (float)Math.Round((decimal)Widgets.HorizontalSlider(
                            sliderRect,
                            thingDef.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Sharp), 0,
                            4f, false,
                            thingDef.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Sharp)
                                .ToStringPercent(),
                            thingLabel,
                            modInfo), 4));
                    break;
                case "stuffpowerarmorblunt":
                    if (thingDef.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Blunt) !=
                        vanillaValues[thingDef.defName])
                    {
                        modifiedValues[thingDef.defName] =
                            thingDef.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Blunt);
                        GUI.color = Color.green;
                    }
                    else
                    {
                        modifiedValues.Remove(thingDef.defName);
                    }

                    thingDef.SetStatBaseValue(StatDefOf.StuffPower_Armor_Blunt,
                        (float)Math.Round((decimal)Widgets.HorizontalSlider(
                            sliderRect,
                            thingDef.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Blunt), 0,
                            2.5f, false,
                            thingDef.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Blunt)
                                .ToStringPercent(),
                            thingLabel,
                            modInfo), 4));
                    break;
                case "stuffpowerarmorheat":
                    if (thingDef.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Heat) !=
                        vanillaValues[thingDef.defName])
                    {
                        modifiedValues[thingDef.defName] =
                            thingDef.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Heat);
                        GUI.color = Color.green;
                    }
                    else
                    {
                        modifiedValues.Remove(thingDef.defName);
                    }

                    thingDef.SetStatBaseValue(StatDefOf.StuffPower_Armor_Heat,
                        (float)Math.Round((decimal)Widgets.HorizontalSlider(
                            sliderRect,
                            thingDef.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Heat), 0,
                            6f, false,
                            thingDef.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Heat)
                                .ToStringPercent(),
                            thingLabel,
                            modInfo), 4));
                    break;
                case "stuffpowerinsulationheat":
                    if (thingDef.GetStatValueAbstract(StatDefOf.StuffPower_Insulation_Heat) !=
                        vanillaValues[thingDef.defName])
                    {
                        modifiedValues[thingDef.defName] =
                            thingDef.GetStatValueAbstract(StatDefOf.StuffPower_Insulation_Heat);
                        GUI.color = Color.green;
                    }
                    else
                    {
                        modifiedValues.Remove(thingDef.defName);
                    }

                    thingDef.SetStatBaseValue(StatDefOf.StuffPower_Insulation_Heat,
                        (float)Math.Round((decimal)Widgets.HorizontalSlider(
                            sliderRect,
                            thingDef.GetStatValueAbstract(StatDefOf.StuffPower_Insulation_Heat), 0,
                            100f, false,
                            thingDef.GetStatValueAbstract(StatDefOf.StuffPower_Insulation_Heat)
                                .ToStringTemperatureOffset(),
                            thingLabel,
                            modInfo), 4));
                    break;
                case "stuffpowerinsulationcold":
                    if (thingDef.GetStatValueAbstract(StatDefOf.StuffPower_Insulation_Cold) !=
                        vanillaValues[thingDef.defName])
                    {
                        modifiedValues[thingDef.defName] =
                            thingDef.GetStatValueAbstract(StatDefOf.StuffPower_Insulation_Cold);
                        GUI.color = Color.green;
                    }
                    else
                    {
                        modifiedValues.Remove(thingDef.defName);
                    }

                    thingDef.SetStatBaseValue(StatDefOf.StuffPower_Insulation_Cold,
                        (float)Math.Round((decimal)Widgets.HorizontalSlider(
                            sliderRect,
                            thingDef.GetStatValueAbstract(StatDefOf.StuffPower_Insulation_Cold), 0,
                            100f, false,
                            thingDef.GetStatValueAbstract(StatDefOf.StuffPower_Insulation_Cold)
                                .ToStringTemperatureOffset(),
                            thingLabel,
                            modInfo), 4));
                    break;
                case "flammability":
                    if (thingDef.BaseFlammability !=
                        vanillaValues[thingDef.defName])
                    {
                        modifiedValues[thingDef.defName] =
                            thingDef.BaseFlammability;
                        GUI.color = Color.green;
                    }
                    else
                    {
                        modifiedValues.Remove(thingDef.defName);
                    }

                    thingDef.SetStatBaseValue(StatDefOf.Flammability,
                        (float)Math.Round((decimal)Widgets.HorizontalSlider(
                            sliderRect,
                            thingDef.BaseFlammability, 0f,
                            2.5f, false,
                            thingDef.BaseFlammability.ToStringPercent(),
                            thingLabel,
                            modInfo), 4));
                    break;
                case "flammabilityfactor":
                    if (thingDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.Flammability) !=
                        vanillaValues[thingDef.defName])
                    {
                        modifiedValues[thingDef.defName] =
                            thingDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.Flammability);
                        GUI.color = Color.green;
                    }
                    else
                    {
                        modifiedValues.Remove(thingDef.defName);
                    }

                    thingDef.stuffProps.statFactors ??= [];

                    if (thingDef.stuffProps.statFactors.All(modifier => modifier.stat != StatDefOf.Flammability))
                    {
                        thingDef.stuffProps.statFactors.Add(new StatModifier
                            { stat = StatDefOf.Flammability, value = 1f });
                    }

                    thingDef.stuffProps.statFactors.First(modifier => modifier.stat == StatDefOf.Flammability).value =
                        (float)Math.Round((decimal)Widgets.HorizontalSlider(
                            sliderRect,
                            thingDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.Flammability), 0,
                            3.5f, false,
                            thingDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.Flammability)
                                .ToStringPercent(),
                            thingLabel,
                            modInfo), 4);
                    break;
                case "maxhitpointsfactor":
                    if (thingDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.MaxHitPoints) !=
                        vanillaValues[thingDef.defName])
                    {
                        modifiedValues[thingDef.defName] =
                            thingDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.MaxHitPoints);
                        GUI.color = Color.green;
                    }
                    else
                    {
                        modifiedValues.Remove(thingDef.defName);
                    }

                    thingDef.stuffProps.statFactors ??= [];

                    if (thingDef.stuffProps.statFactors.All(modifier => modifier.stat != StatDefOf.MaxHitPoints))
                    {
                        thingDef.stuffProps.statFactors.Add(new StatModifier
                            { stat = StatDefOf.MaxHitPoints, value = 1f });
                    }

                    thingDef.stuffProps.statFactors.First(modifier => modifier.stat == StatDefOf.MaxHitPoints).value =
                        (float)Math.Round((decimal)Widgets.HorizontalSlider(
                            sliderRect,
                            thingDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.MaxHitPoints), 0,
                            10f, false,
                            thingDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.MaxHitPoints)
                                .ToStringPercent(),
                            thingLabel,
                            modInfo), 4);
                    break;
            }

            GUI.color = Color.white;
        }

        scrollListing.End();

        Widgets.EndScrollView();
    }


    private void drawTabsList(Rect rect)
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

        const int listAddition = 24;


        tabContentRect.height = (settingTabs.Count * 25f) + listAddition;
        Widgets.BeginScrollView(tabFrameRect, ref tabsScrollPosition, tabContentRect);
        listing_Standard.Begin(tabContentRect);
        //Text.Font = GameFont.Tiny;
        foreach (var settingTab in settingTabs)
        {
            if (string.IsNullOrEmpty(settingTab))
            {
                listing_Standard.ListItemSelectable(null, Color.yellow);
                continue;
            }

            if (instance.Settings.HasCustomValues(settingTab.ToLower()))
            {
                GUI.color = Color.green;
            }

            if (listing_Standard.ListItemSelectable($"CSP.{settingTab.ToLower()}".Translate(), Color.yellow,
                    SelectedDef == settingTab))
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