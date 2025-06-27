using System.Collections.Generic;
using System.Linq;
using ChangeStuffProperties.Settings;
using RimWorld;

namespace ChangeStuffProperties;

public static class BeautyMultiplier
{
    public static readonly Dictionary<string, float> VanillaBeautyMultipliers = new();

    static BeautyMultiplier()
    {
    }

    public static void Initialize()
    {
        saveVanillaBeautyMultiplierValues();
        setCustomBeautyMultiplierValues();
    }

    private static void saveVanillaBeautyMultiplierValues()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            VanillaBeautyMultipliers[thingDef.defName] =
                thingDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.Beauty);
        }
    }

    private static void setCustomBeautyMultiplierValues()
    {
        if (ChangeStuffProperties_Mod.instance?.Settings?.CustomBeautyMultipliers == null)
        {
            return;
        }

        var counter = 0;
        foreach (var thingDef in Main.AllStuff)
        {
            if (!ChangeStuffProperties_Mod.instance.Settings.CustomBeautyMultipliers.TryGetValue(thingDef.defName,
                    out var multiplier))
            {
                continue;
            }

            if (multiplier == 1f)
            {
                thingDef.stuffProps.statFactors.RemoveAll(modifier => modifier.stat == StatDefOf.Beauty);
                continue;
            }

            thingDef.stuffProps.statFactors ??= [];

            if (thingDef.stuffProps.statFactors.All(modifier => modifier.stat != StatDefOf.Beauty))
            {
                thingDef.stuffProps.statFactors.Add(new StatModifier
                {
                    stat = StatDefOf.Beauty,
                    value = ChangeStuffProperties_Mod.instance.Settings.CustomBeautyMultipliers[thingDef.defName]
                });
                continue;
            }

            thingDef.stuffProps.statFactors.First(modifier => modifier.stat == StatDefOf.Beauty).value =
                ChangeStuffProperties_Mod.instance.Settings.CustomBeautyMultipliers[thingDef.defName];

            counter++;
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom beautymultiplier for {counter} stufftypes.");
        }
    }

    public static void ResetBeautyMultiplierToVanillaRates()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            if (VanillaBeautyMultipliers[thingDef.defName] == 1f)
            {
                thingDef.stuffProps.statFactors?.RemoveAll(modifier => modifier.stat == StatDefOf.Beauty);
                continue;
            }

            thingDef.stuffProps.statFactors ??= [];

            if (thingDef.stuffProps.statFactors.All(modifier => modifier.stat != StatDefOf.Beauty))
            {
                thingDef.stuffProps.statFactors.Add(new StatModifier
                    { stat = StatDefOf.Beauty, value = VanillaBeautyMultipliers[thingDef.defName] });
                continue;
            }

            thingDef.stuffProps.statFactors.First(modifier => modifier.stat == StatDefOf.Beauty).value =
                VanillaBeautyMultipliers[thingDef.defName];
        }
    }
}