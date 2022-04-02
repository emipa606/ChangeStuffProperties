using System.Collections.Generic;
using ChangeStuffProperties.Settings;
using RimWorld;

namespace ChangeStuffProperties;

public static class Mass
{
    public static readonly Dictionary<string, float> VanillaMasses = new Dictionary<string, float>();

    static Mass()
    {
    }

    public static void Initialize()
    {
        saveVanillaMassValues();
        setCustomMassValues();
    }

    private static void saveVanillaMassValues()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            VanillaMasses[thingDef.defName] = thingDef.BaseMass;
        }
    }

    private static void setCustomMassValues()
    {
        if (ChangeStuffProperties_Mod.instance?.Settings?.CustomMass == null)
        {
            return;
        }

        var counter = 0;
        foreach (var thingDef in Main.AllStuff)
        {
            if (!ChangeStuffProperties_Mod.instance.Settings.CustomMass.ContainsKey(thingDef.defName))
            {
                continue;
            }

            thingDef.SetStatBaseValue(StatDefOf.Mass,
                ChangeStuffProperties_Mod.instance.Settings.CustomMass[thingDef.defName]);
            counter++;
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom mass for {counter} stufftypes.");
        }
    }

    public static void ResetMassToVanillaRates()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            thingDef.SetStatBaseValue(StatDefOf.Mass, VanillaMasses[thingDef.defName]);
        }
    }
}