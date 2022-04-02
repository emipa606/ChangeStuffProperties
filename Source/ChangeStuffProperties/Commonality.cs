using System.Collections.Generic;
using ChangeStuffProperties.Settings;

namespace ChangeStuffProperties;

public static class Commonality
{
    public static readonly Dictionary<string, float> VanillaCommonalities = new Dictionary<string, float>();

    static Commonality()
    {
    }

    public static void Initialize()
    {
        saveVanillaCommonalityValues();
        setCustomCommonalityValues();
    }

    private static void saveVanillaCommonalityValues()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            VanillaCommonalities[thingDef.defName] = thingDef.stuffProps.commonality;
        }
    }

    private static void setCustomCommonalityValues()
    {
        if (ChangeStuffProperties_Mod.instance?.Settings?.CustomCommonality == null)
        {
            return;
        }

        var counter = 0;
        foreach (var thingDef in Main.AllStuff)
        {
            if (!ChangeStuffProperties_Mod.instance.Settings.CustomCommonality.ContainsKey(thingDef.defName))
            {
                continue;
            }

            thingDef.stuffProps.commonality =
                ChangeStuffProperties_Mod.instance.Settings.CustomCommonality[thingDef.defName];
            counter++;
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom commonality for {counter} stufftypes.");
        }
    }

    public static void ResetCommonalityToVanillaRates()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            thingDef.stuffProps.commonality = VanillaCommonalities[thingDef.defName];
        }
    }
}