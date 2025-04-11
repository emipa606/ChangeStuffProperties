using System.Collections.Generic;
using System.Linq;
using ChangeStuffProperties.Settings;
using RimWorld;

namespace ChangeStuffProperties;

public static class WorkToBuildMultiplier
{
    public static readonly Dictionary<string, float> VanillaWorkToBuildMultipliers = new Dictionary<string, float>();

    static WorkToBuildMultiplier()
    {
    }

    public static void Initialize()
    {
        saveVanillaWorkToBuildMultiplierValues();
        setCustomWorkToBuildMultiplierValues();
    }

    private static void saveVanillaWorkToBuildMultiplierValues()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            VanillaWorkToBuildMultipliers[thingDef.defName] =
                thingDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.WorkToBuild);
        }
    }

    private static void setCustomWorkToBuildMultiplierValues()
    {
        if (ChangeStuffProperties_Mod.instance?.Settings?.CustomWorkToBuildMultipliers == null)
        {
            return;
        }

        var counter = 0;
        foreach (var thingDef in Main.AllStuff)
        {
            if (!ChangeStuffProperties_Mod.instance.Settings.CustomWorkToBuildMultipliers.TryGetValue(thingDef.defName,
                    out var multiplier))
            {
                continue;
            }

            if (multiplier == 1f)
            {
                thingDef.stuffProps.statFactors.RemoveAll(modifier => modifier.stat == StatDefOf.WorkToBuild);
                continue;
            }

            if (thingDef.stuffProps.statFactors == null)
            {
                thingDef.stuffProps.statFactors = [];
            }

            if (thingDef.stuffProps.statFactors.All(modifier => modifier.stat != StatDefOf.WorkToBuild))
            {
                thingDef.stuffProps.statFactors.Add(new StatModifier
                {
                    stat = StatDefOf.WorkToBuild,
                    value = ChangeStuffProperties_Mod.instance.Settings.CustomWorkToBuildMultipliers[thingDef.defName]
                });
                continue;
            }

            thingDef.stuffProps.statFactors.First(modifier => modifier.stat == StatDefOf.WorkToBuild).value =
                ChangeStuffProperties_Mod.instance.Settings.CustomWorkToBuildMultipliers[thingDef.defName];

            counter++;
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom WorkToBuildmultiplier for {counter} stufftypes.");
        }
    }

    public static void ResetWorkToBuildMultiplierToVanillaRates()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            if (VanillaWorkToBuildMultipliers[thingDef.defName] == 1f)
            {
                thingDef.stuffProps.statFactors?.RemoveAll(modifier => modifier.stat == StatDefOf.WorkToBuild);
                continue;
            }

            if (thingDef.stuffProps.statFactors == null)
            {
                thingDef.stuffProps.statFactors = [];
            }

            if (thingDef.stuffProps.statFactors.All(modifier => modifier.stat != StatDefOf.WorkToBuild))
            {
                thingDef.stuffProps.statFactors.Add(new StatModifier
                    { stat = StatDefOf.WorkToBuild, value = VanillaWorkToBuildMultipliers[thingDef.defName] });
                continue;
            }

            thingDef.stuffProps.statFactors.First(modifier => modifier.stat == StatDefOf.WorkToBuild).value =
                VanillaWorkToBuildMultipliers[thingDef.defName];
        }
    }
}