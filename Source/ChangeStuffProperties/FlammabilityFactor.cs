using System.Collections.Generic;
using System.Linq;
using ChangeStuffProperties.Settings;
using RimWorld;

namespace ChangeStuffProperties;

public static class FlammabilityFactor
{
    public static readonly Dictionary<string, float> VanillaFlammabilityFactor = new Dictionary<string, float>();

    static FlammabilityFactor()
    {
    }

    public static void Initialize()
    {
        saveVanillaFlammabilityFactorValues();
        setCustomFlammabilityFactorValues();
    }

    private static void saveVanillaFlammabilityFactorValues()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            VanillaFlammabilityFactor[thingDef.defName] =
                thingDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.Flammability);
        }
    }

    private static void setCustomFlammabilityFactorValues()
    {
        if (ChangeStuffProperties_Mod.instance?.Settings?.CustomFlammabilityFactor == null)
        {
            return;
        }

        var counter = 0;
        foreach (var thingDef in Main.AllStuff)
        {
            if (!ChangeStuffProperties_Mod.instance.Settings.CustomFlammabilityFactor.ContainsKey(thingDef.defName))
            {
                continue;
            }

            if (ChangeStuffProperties_Mod.instance.Settings.CustomFlammabilityFactor[thingDef.defName] == 1f)
            {
                thingDef.stuffProps.statFactors.RemoveAll(modifier => modifier.stat == StatDefOf.Flammability);
                continue;
            }

            if (thingDef.stuffProps.statFactors == null)
            {
                thingDef.stuffProps.statFactors = new List<StatModifier>();
            }

            if (thingDef.stuffProps.statFactors.All(modifier => modifier.stat != StatDefOf.Flammability))
            {
                thingDef.stuffProps.statFactors.Add(new StatModifier
                {
                    stat = StatDefOf.Flammability,
                    value = ChangeStuffProperties_Mod.instance.Settings.CustomFlammabilityFactor[thingDef.defName]
                });
                continue;
            }

            thingDef.stuffProps.statFactors.First(modifier => modifier.stat == StatDefOf.Flammability).value =
                ChangeStuffProperties_Mod.instance.Settings.CustomFlammabilityFactor[thingDef.defName];

            counter++;
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom flammability factor for {counter} stufftypes.");
        }
    }

    public static void ResetFlammabilityFactorToVanillaRates()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            if (VanillaFlammabilityFactor[thingDef.defName] == 1f)
            {
                thingDef.stuffProps.statFactors?.RemoveAll(modifier => modifier.stat == StatDefOf.Flammability);
                continue;
            }

            if (thingDef.stuffProps.statFactors == null)
            {
                thingDef.stuffProps.statFactors = new List<StatModifier>();
            }

            if (thingDef.stuffProps.statFactors.All(modifier => modifier.stat != StatDefOf.Flammability))
            {
                thingDef.stuffProps.statFactors.Add(new StatModifier
                    { stat = StatDefOf.Flammability, value = VanillaFlammabilityFactor[thingDef.defName] });
                continue;
            }

            thingDef.stuffProps.statFactors.First(modifier => modifier.stat == StatDefOf.Flammability).value =
                VanillaFlammabilityFactor[thingDef.defName];
        }
    }
}