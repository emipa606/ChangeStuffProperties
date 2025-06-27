using System.Collections.Generic;
using System.Linq;
using ChangeStuffProperties.Settings;
using RimWorld;

namespace ChangeStuffProperties;

public static class BeautyOffset
{
    public static readonly Dictionary<string, float> VanillaBeautyOffsets = new();

    static BeautyOffset()
    {
    }

    public static void Initialize()
    {
        saveVanillaBeautyOffsetValues();
        setCustomBeautyOffsetValues();
    }

    private static void saveVanillaBeautyOffsetValues()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            VanillaBeautyOffsets[thingDef.defName] =
                thingDef.stuffProps.statOffsets.GetStatOffsetFromList(StatDefOf.Beauty);
        }
    }

    private static void setCustomBeautyOffsetValues()
    {
        if (ChangeStuffProperties_Mod.instance?.Settings?.CustomBeautyOffsets == null)
        {
            return;
        }

        var counter = 0;
        foreach (var thingDef in Main.AllStuff)
        {
            if (!ChangeStuffProperties_Mod.instance.Settings.CustomBeautyOffsets.TryGetValue(thingDef.defName,
                    out var offset))
            {
                continue;
            }

            if (offset == 0)
            {
                thingDef.stuffProps.statOffsets.RemoveAll(modifier => modifier.stat == StatDefOf.Beauty);
                continue;
            }

            thingDef.stuffProps.statOffsets ??= [];

            if (thingDef.stuffProps.statOffsets.All(modifier => modifier.stat != StatDefOf.Beauty))
            {
                thingDef.stuffProps.statOffsets.Add(new StatModifier
                {
                    stat = StatDefOf.Beauty,
                    value = ChangeStuffProperties_Mod.instance.Settings.CustomBeautyOffsets[thingDef.defName]
                });
                continue;
            }

            thingDef.stuffProps.statOffsets.First(modifier => modifier.stat == StatDefOf.Beauty).value =
                ChangeStuffProperties_Mod.instance.Settings.CustomBeautyOffsets[thingDef.defName];

            counter++;
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom beautyoffset for {counter} stufftypes.");
        }
    }

    public static void ResetBeautyOffsetToVanillaRates()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            if (VanillaBeautyOffsets[thingDef.defName] == 0)
            {
                thingDef.stuffProps.statOffsets?.RemoveAll(modifier => modifier.stat == StatDefOf.Beauty);
                continue;
            }

            thingDef.stuffProps.statOffsets ??= [];

            if (thingDef.stuffProps.statOffsets.All(modifier => modifier.stat != StatDefOf.Beauty))
            {
                thingDef.stuffProps.statOffsets.Add(new StatModifier
                    { stat = StatDefOf.Beauty, value = VanillaBeautyOffsets[thingDef.defName] });
                continue;
            }

            thingDef.stuffProps.statOffsets.First(modifier => modifier.stat == StatDefOf.Beauty).value =
                VanillaBeautyOffsets[thingDef.defName];
        }
    }
}