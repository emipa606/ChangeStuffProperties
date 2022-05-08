using System.Collections.Generic;
using ChangeStuffProperties.Settings;
using RimWorld;

namespace ChangeStuffProperties;

public static class BluntDamageMultiplier
{
    public static readonly Dictionary<string, float> VanillaBluntDamageMultiplier = new Dictionary<string, float>();

    static BluntDamageMultiplier()
    {
    }

    public static void Initialize()
    {
        saveVanillaBluntDamageMultiplierValues();
        setCustomBluntDamageMultiplierValues();
    }

    private static void saveVanillaBluntDamageMultiplierValues()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            VanillaBluntDamageMultiplier[thingDef.defName] =
                thingDef.GetStatValueAbstract(StatDefOf.BluntDamageMultiplier);
        }
    }

    private static void setCustomBluntDamageMultiplierValues()
    {
        if (ChangeStuffProperties_Mod.instance?.Settings?.CustomBluntDamageMultiplier == null)
        {
            return;
        }

        var counter = 0;
        foreach (var thingDef in Main.AllStuff)
        {
            if (!ChangeStuffProperties_Mod.instance.Settings.CustomBluntDamageMultiplier.ContainsKey(thingDef.defName))
            {
                continue;
            }

            thingDef.SetStatBaseValue(StatDefOf.BluntDamageMultiplier,
                ChangeStuffProperties_Mod.instance.Settings.CustomBluntDamageMultiplier[thingDef.defName]);
            counter++;
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom BluntDamageMultiplier for {counter} stufftypes.");
        }
    }

    public static void ResetBluntDamageMultiplierToVanillaRates()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            thingDef.SetStatBaseValue(StatDefOf.BluntDamageMultiplier, VanillaBluntDamageMultiplier[thingDef.defName]);
        }
    }
}