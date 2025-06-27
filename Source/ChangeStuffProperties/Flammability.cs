using System.Collections.Generic;
using ChangeStuffProperties.Settings;
using RimWorld;

namespace ChangeStuffProperties;

public static class Flammability
{
    public static readonly Dictionary<string, float> VanillaFlammability = new();

    static Flammability()
    {
    }

    public static void Initialize()
    {
        saveVanillaFlammabilityValues();
        setCustomFlammabilityValues();
    }

    private static void saveVanillaFlammabilityValues()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            VanillaFlammability[thingDef.defName] = thingDef.BaseFlammability;
        }
    }

    private static void setCustomFlammabilityValues()
    {
        if (ChangeStuffProperties_Mod.instance?.Settings?.CustomFlammability == null)
        {
            return;
        }

        var counter = 0;
        foreach (var thingDef in Main.AllStuff)
        {
            if (!ChangeStuffProperties_Mod.instance.Settings.CustomFlammability.ContainsKey(thingDef.defName))
            {
                continue;
            }

            thingDef.SetStatBaseValue(StatDefOf.Flammability,
                ChangeStuffProperties_Mod.instance.Settings.CustomFlammability[thingDef.defName]);
            counter++;
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom flammability for {counter} stufftypes.");
        }
    }

    public static void ResetFlammabilityToVanillaRates()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            thingDef.SetStatBaseValue(StatDefOf.Flammability, VanillaFlammability[thingDef.defName]);
        }
    }
}