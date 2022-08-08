using System.Collections.Generic;
using System.Linq;
using ChangeStuffProperties.Settings;
using RimWorld;

namespace ChangeStuffProperties;

public static class MaxHitPointsFactor
{
    public static readonly Dictionary<string, float> VanillaMaxHitPointsFactor = new Dictionary<string, float>();

    static MaxHitPointsFactor()
    {
    }

    public static void Initialize()
    {
        saveVanillaMaxHitPointsFactorValues();
        setCustomMaxHitPointsFactorValues();
    }

    private static void saveVanillaMaxHitPointsFactorValues()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            VanillaMaxHitPointsFactor[thingDef.defName] =
                thingDef.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.MaxHitPoints);
        }
    }

    private static void setCustomMaxHitPointsFactorValues()
    {
        if (ChangeStuffProperties_Mod.instance?.Settings?.CustomMaxHitPointsFactor == null)
        {
            return;
        }

        var counter = 0;
        foreach (var thingDef in Main.AllStuff)
        {
            if (!ChangeStuffProperties_Mod.instance.Settings.CustomMaxHitPointsFactor.ContainsKey(thingDef.defName))
            {
                continue;
            }

            if (ChangeStuffProperties_Mod.instance.Settings.CustomMaxHitPointsFactor[thingDef.defName] == 1f)
            {
                thingDef.stuffProps.statFactors.RemoveAll(modifier => modifier.stat == StatDefOf.MaxHitPoints);
                continue;
            }

            if (thingDef.stuffProps.statFactors == null)
            {
                thingDef.stuffProps.statFactors = new List<StatModifier>();
            }

            if (thingDef.stuffProps.statFactors.All(modifier => modifier.stat != StatDefOf.MaxHitPoints))
            {
                thingDef.stuffProps.statFactors.Add(new StatModifier
                {
                    stat = StatDefOf.MaxHitPoints,
                    value = ChangeStuffProperties_Mod.instance.Settings.CustomMaxHitPointsFactor[thingDef.defName]
                });
                continue;
            }

            thingDef.stuffProps.statFactors.First(modifier => modifier.stat == StatDefOf.MaxHitPoints).value =
                ChangeStuffProperties_Mod.instance.Settings.CustomMaxHitPointsFactor[thingDef.defName];

            counter++;
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom MaxHitPoints factor for {counter} stufftypes.");
        }
    }

    public static void ResetMaxHitPointsFactorToVanillaRates()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            if (VanillaMaxHitPointsFactor[thingDef.defName] == 1f)
            {
                thingDef.stuffProps.statFactors?.RemoveAll(modifier => modifier.stat == StatDefOf.MaxHitPoints);
                continue;
            }

            if (thingDef.stuffProps.statFactors == null)
            {
                thingDef.stuffProps.statFactors = new List<StatModifier>();
            }

            if (thingDef.stuffProps.statFactors.All(modifier => modifier.stat != StatDefOf.MaxHitPoints))
            {
                thingDef.stuffProps.statFactors.Add(new StatModifier
                    { stat = StatDefOf.MaxHitPoints, value = VanillaMaxHitPointsFactor[thingDef.defName] });
                continue;
            }

            thingDef.stuffProps.statFactors.First(modifier => modifier.stat == StatDefOf.MaxHitPoints).value =
                VanillaMaxHitPointsFactor[thingDef.defName];
        }
    }
}