using System.Collections.Generic;
using System.Linq;
using ChangeStuffProperties.Settings;
using Verse;

namespace ChangeStuffProperties;

[StaticConstructorOnStartup]
public static class Main
{
    private static List<ThingDef> allStuff;

    static Main()
    {
        ChangeStuffProperties_Mod.instance.Settings.Initialize();
    }

    public static List<ThingDef> AllStuff
    {
        get
        {
            if (allStuff == null || allStuff.Count == 0)
            {
                allStuff = (from stuff in DefDatabase<ThingDef>.AllDefsListForReading
                    where stuff.IsStuff
                    orderby stuff.label
                    select stuff).ToList();
            }

            return allStuff;
        }
    }


    public static void LogMessage(string message, bool forced = false, bool warning = false)
    {
        if (warning)
        {
            Log.Warning($"[ChangeStuffProperties]: {message}");
            return;
        }

        if (!forced && !ChangeStuffProperties_Mod.instance.Settings.VerboseLogging)
        {
            return;
        }

        Log.Message($"[ChangeStuffProperties]: {message}");
    }
}