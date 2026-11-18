using System.Collections.Generic;
using System.Linq;
using ChangeStuffProperties.Settings;
using Verse;

namespace ChangeStuffProperties;

[StaticConstructorOnStartup]
public static class Main
{
    static Main()
    {
        ChangeStuffProperties_Settings.Initialize();
    }

    public static List<ThingDef> AllStuff
    {
        get
        {
            if (field == null || field.Count == 0)
            {
                field = (from stuff in DefDatabase<ThingDef>.AllDefsListForReading
                    where stuff.IsStuff
                    orderby stuff.label
                    select stuff).ToList();
            }

            return field;
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