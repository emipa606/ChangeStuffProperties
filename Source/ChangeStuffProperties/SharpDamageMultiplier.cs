using System.Collections.Generic;
using ChangeStuffProperties.Settings;
using RimWorld;

namespace ChangeStuffProperties;

public static class SharpDamageMultiplier
{
    public static readonly Dictionary<string, float> VanillaSharpDamageMultiplier = new Dictionary<string, float>();

    static SharpDamageMultiplier()
    {
    }

    public static void Initialize()
    {
        saveVanillaSharpDamageMultiplierValues();
        setCustomSharpDamageMultiplierValues();
    }

    private static void saveVanillaSharpDamageMultiplierValues()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            VanillaSharpDamageMultiplier[thingDef.defName] =
                thingDef.GetStatValueAbstract(StatDefOf.SharpDamageMultiplier);
        }
    }

    private static void setCustomSharpDamageMultiplierValues()
    {
        if (ChangeStuffProperties_Mod.instance?.Settings?.CustomSharpDamageMultiplier == null)
        {
            return;
        }

        var counter = 0;
        foreach (var thingDef in Main.AllStuff)
        {
            if (!ChangeStuffProperties_Mod.instance.Settings.CustomSharpDamageMultiplier.ContainsKey(thingDef.defName))
            {
                continue;
            }

            thingDef.SetStatBaseValue(StatDefOf.SharpDamageMultiplier,
                ChangeStuffProperties_Mod.instance.Settings.CustomSharpDamageMultiplier[thingDef.defName]);
            counter++;
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom SharpDamageMultiplier for {counter} stufftypes.");
        }
    }

    public static void ResetSharpDamageMultiplierToVanillaRates()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            thingDef.SetStatBaseValue(StatDefOf.SharpDamageMultiplier, VanillaSharpDamageMultiplier[thingDef.defName]);
        }
    }
}