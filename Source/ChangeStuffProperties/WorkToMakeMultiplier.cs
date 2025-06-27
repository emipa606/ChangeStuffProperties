using System.Collections.Generic;
using System.Linq;
using ChangeStuffProperties.Settings;
using RimWorld;

namespace ChangeStuffProperties;

public static class WorkToMakeMultiplier
{
    public static readonly Dictionary<string, float> VanillaWorkToMakeMultipliers = new();

    static WorkToMakeMultiplier()
    {
    }

    public static void Initialize()
    {
        saveVanillaWorkToMakeMultiplierValues();
        setCustomWorkToMakeMultiplierValues();
    }

    private static void saveVanillaWorkToMakeMultiplierValues()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            VanillaWorkToMakeMultipliers[thingDef.defName] =
                thingDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.WorkToMake);
        }
    }

    private static void setCustomWorkToMakeMultiplierValues()
    {
        if (ChangeStuffProperties_Mod.instance?.Settings?.CustomWorkToMakeMultipliers == null)
        {
            return;
        }

        var counter = 0;
        foreach (var thingDef in Main.AllStuff)
        {
            if (!ChangeStuffProperties_Mod.instance.Settings.CustomWorkToMakeMultipliers.TryGetValue(thingDef.defName,
                    out var multiplier))
            {
                continue;
            }

            if (multiplier == 1f)
            {
                thingDef.stuffProps.statFactors.RemoveAll(modifier => modifier.stat == StatDefOf.WorkToMake);
                continue;
            }

            thingDef.stuffProps.statFactors ??= [];

            if (thingDef.stuffProps.statFactors.All(modifier => modifier.stat != StatDefOf.WorkToMake))
            {
                thingDef.stuffProps.statFactors.Add(new StatModifier
                {
                    stat = StatDefOf.WorkToMake,
                    value = ChangeStuffProperties_Mod.instance.Settings.CustomWorkToMakeMultipliers[thingDef.defName]
                });
                continue;
            }

            thingDef.stuffProps.statFactors.First(modifier => modifier.stat == StatDefOf.WorkToMake).value =
                ChangeStuffProperties_Mod.instance.Settings.CustomWorkToMakeMultipliers[thingDef.defName];

            counter++;
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom WorkToMakemultiplier for {counter} stufftypes.");
        }
    }

    public static void ResetWorkToMakeMultiplierToVanillaRates()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            if (VanillaWorkToMakeMultipliers[thingDef.defName] == 1f)
            {
                thingDef.stuffProps.statFactors?.RemoveAll(modifier => modifier.stat == StatDefOf.WorkToMake);
                continue;
            }

            thingDef.stuffProps.statFactors ??= [];

            if (thingDef.stuffProps.statFactors.All(modifier => modifier.stat != StatDefOf.WorkToMake))
            {
                thingDef.stuffProps.statFactors.Add(new StatModifier
                    { stat = StatDefOf.WorkToMake, value = VanillaWorkToMakeMultipliers[thingDef.defName] });
                continue;
            }

            thingDef.stuffProps.statFactors.First(modifier => modifier.stat == StatDefOf.WorkToMake).value =
                VanillaWorkToMakeMultipliers[thingDef.defName];
        }
    }
}