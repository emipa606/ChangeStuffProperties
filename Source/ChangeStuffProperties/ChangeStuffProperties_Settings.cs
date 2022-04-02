using System.Collections.Generic;
using System.Linq;
using Verse;

namespace ChangeStuffProperties;

public class ChangeStuffProperties_Settings : ModSettings
{
    public Dictionary<string, float> CustomCommonality;

    private List<string> customCommonalityKeys;

    private List<float> customCommonalityValues;
    public Dictionary<string, float> CustomMarketValue;

    private List<string> customMarketValueKeys;

    private List<float> customMarketValueValues;
    public Dictionary<string, float> CustomMass;
    private List<string> customMassKeys;

    private List<float> customMassValues;
    public bool VerboseLogging;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref VerboseLogging, "VerboseLogging");
        Scribe_Collections.Look(ref CustomCommonality, "CustomCommonality",
            LookMode.Value,
            LookMode.Value,
            ref customCommonalityKeys, ref customCommonalityValues);
        Scribe_Collections.Look(ref CustomMass, "CustomMass",
            LookMode.Value,
            LookMode.Value,
            ref customMassKeys, ref customMassValues);
        Scribe_Collections.Look(ref CustomMarketValue, "CustomMarketValue",
            LookMode.Value,
            LookMode.Value,
            ref customMarketValueKeys, ref customMarketValueValues);
    }

    public void Initialize()
    {
        Commonality.Initialize();
        Mass.Initialize();
        MarketValue.Initialize();
    }

    public void ResetValues(string valueLabel)
    {
        if (valueLabel is "commonality" or "all")
        {
            customCommonalityKeys = new List<string>();
            customCommonalityValues = new List<float>();
            CustomCommonality = new Dictionary<string, float>();
            Commonality.ResetCommonalityToVanillaRates();
        }

        if (valueLabel is "mass" or "all")
        {
            customMassKeys = new List<string>();
            customMassValues = new List<float>();
            CustomMass = new Dictionary<string, float>();
            Mass.ResetMassToVanillaRates();
        }

        if (valueLabel is "marketvalue" or "all")
        {
            customMarketValueKeys = new List<string>();
            customMarketValueValues = new List<float>();
            CustomMarketValue = new Dictionary<string, float>();
            MarketValue.ResetMarketValueToVanillaRates();
        }
    }

    public bool HasCustomValues(string type = null)
    {
        switch (type)
        {
            case null or "commonality" when CustomCommonality?.Any() == true:
            case null or "mass" when CustomMass?.Any() == true:
            case null or "marketvalue" when CustomMarketValue?.Any() == true:
                return true;
            default:
                return false;
        }
    }
}