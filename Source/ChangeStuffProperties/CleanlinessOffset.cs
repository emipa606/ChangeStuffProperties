using System.Collections.Generic;
using System.Linq;
using ChangeStuffProperties.Settings;
using RimWorld;

namespace ChangeStuffProperties;

public static class CleanlinessOffset
{
    public static readonly Dictionary<string, float> VanillaCleanlinessOffsets = new Dictionary<string, float>();

    static CleanlinessOffset()
    {
    }

    public static void Initialize()
    {
        saveVanillaCleanlinessOffsetValues();
        setCustomCleanlinessOffsetValues();
    }

    private static void saveVanillaCleanlinessOffsetValues()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            VanillaCleanlinessOffsets[thingDef.defName] =
                thingDef.stuffProps.statOffsets.GetStatOffsetFromList(StatDefOf.Cleanliness);
        }
    }

    private static void setCustomCleanlinessOffsetValues()
    {
        if (ChangeStuffProperties_Mod.instance?.Settings?.CustomCleanlinessOffsets == null)
        {
            return;
        }

        var counter = 0;
        foreach (var thingDef in Main.AllStuff)
        {
            if (!ChangeStuffProperties_Mod.instance.Settings.CustomCleanlinessOffsets.TryGetValue(thingDef.defName,
                    out var offset))
            {
                continue;
            }

            if (offset == 0)
            {
                thingDef.stuffProps.statOffsets.RemoveAll(modifier => modifier.stat == StatDefOf.Cleanliness);
                continue;
            }

            if (thingDef.stuffProps.statOffsets == null)
            {
                thingDef.stuffProps.statOffsets = [];
            }

            if (thingDef.stuffProps.statOffsets.All(modifier => modifier.stat != StatDefOf.Cleanliness))
            {
                thingDef.stuffProps.statOffsets.Add(new StatModifier
                {
                    stat = StatDefOf.Cleanliness,
                    value = ChangeStuffProperties_Mod.instance.Settings.CustomCleanlinessOffsets[thingDef.defName]
                });
                continue;
            }

            thingDef.stuffProps.statOffsets.First(modifier => modifier.stat == StatDefOf.Cleanliness).value =
                ChangeStuffProperties_Mod.instance.Settings.CustomCleanlinessOffsets[thingDef.defName];

            counter++;
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom cleanlinessoffset for {counter} stufftypes.");
        }
    }

    public static void ResetCleanlinessOffsetToVanillaRates()
    {
        foreach (var thingDef in Main.AllStuff)
        {
            if (VanillaCleanlinessOffsets[thingDef.defName] == 0)
            {
                thingDef.stuffProps.statOffsets?.RemoveAll(modifier => modifier.stat == StatDefOf.Cleanliness);
                continue;
            }

            if (thingDef.stuffProps.statOffsets == null)
            {
                thingDef.stuffProps.statOffsets = [];
            }

            if (thingDef.stuffProps.statOffsets.All(modifier => modifier.stat != StatDefOf.Cleanliness))
            {
                thingDef.stuffProps.statOffsets.Add(new StatModifier
                    { stat = StatDefOf.Cleanliness, value = VanillaCleanlinessOffsets[thingDef.defName] });
                continue;
            }

            thingDef.stuffProps.statOffsets.First(modifier => modifier.stat == StatDefOf.Cleanliness).value =
                VanillaCleanlinessOffsets[thingDef.defName];
        }
    }
}