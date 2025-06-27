using System.Collections.Generic;
using ChangeStuffProperties.Settings;
using RimWorld;

namespace ChangeStuffProperties;

public static class StuffPower_Armor_Blunt
{
    public static readonly Dictionary<string, float> VanillaStuffPower_Armor_Blunt = new();

    static StuffPower_Armor_Blunt()
    {
    }

    public static void Initialize()
    {
        saveVanillaStuffPower_Armor_BluntValues();
        setCustomStuffPower_Armor_BluntValues();
    }

    private static void saveVanillaStuffPower_Armor_BluntValues()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            VanillaStuffPower_Armor_Blunt[thingDef.defName] =
                thingDef.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Blunt);
        }
    }

    private static void setCustomStuffPower_Armor_BluntValues()
    {
        if (ChangeStuffProperties_Mod.instance?.Settings?.CustomStuffPower_Armor_Blunt == null)
        {
            return;
        }

        var counter = 0;
        foreach (var thingDef in Main.AllStuff)
        {
            if (!ChangeStuffProperties_Mod.instance.Settings.CustomStuffPower_Armor_Blunt.ContainsKey(thingDef.defName))
            {
                continue;
            }

            thingDef.SetStatBaseValue(StatDefOf.StuffPower_Armor_Blunt,
                ChangeStuffProperties_Mod.instance.Settings.CustomStuffPower_Armor_Blunt[thingDef.defName]);
            counter++;
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom StuffPower_Armor_Blunt for {counter} stufftypes.");
        }
    }

    public static void ResetStuffPower_Armor_BluntToVanillaRates()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            thingDef.SetStatBaseValue(StatDefOf.StuffPower_Armor_Blunt,
                VanillaStuffPower_Armor_Blunt[thingDef.defName]);
        }
    }
}