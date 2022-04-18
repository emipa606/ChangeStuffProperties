using System.Collections.Generic;
using System.Linq;
using Verse;

namespace ChangeStuffProperties;

public class ChangeStuffProperties_Settings : ModSettings
{
    private List<string> customBeautyMultiplierKeys;
    public Dictionary<string, float> CustomBeautyMultipliers;

    private List<float> customBeautyMultiplierValues;
    private List<string> customBeautyOffsetKeys;
    public Dictionary<string, float> CustomBeautyOffsets;

    private List<float> customBeautyOffsetValues;
    public Dictionary<string, float> CustomCommonality;

    private List<string> customCommonalityKeys;

    private List<float> customCommonalityValues;
    public Dictionary<string, float> CustomFlammability;
    public Dictionary<string, float> CustomFlammabilityFactor;
    private List<string> customFlammabilityFactorKeys;

    private List<float> customFlammabilityFactorValues;
    private List<string> customFlammabilityKeys;

    private List<float> customFlammabilityValues;
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
        Scribe_Collections.Look(ref CustomBeautyOffsets, "CustomBeautyOffsets",
            LookMode.Value,
            LookMode.Value,
            ref customBeautyOffsetKeys, ref customBeautyOffsetValues);
        Scribe_Collections.Look(ref CustomBeautyOffsets, "CustomBeautyMultiplier",
            LookMode.Value,
            LookMode.Value,
            ref customBeautyMultiplierKeys, ref customBeautyMultiplierValues);
        Scribe_Collections.Look(ref CustomFlammability, "CustomFlammability",
            LookMode.Value,
            LookMode.Value,
            ref customFlammabilityKeys, ref customFlammabilityValues);
        Scribe_Collections.Look(ref CustomFlammabilityFactor, "CustomFlammabilityFactor",
            LookMode.Value,
            LookMode.Value,
            ref customFlammabilityFactorKeys, ref customFlammabilityFactorValues);
    }

    public void Initialize()
    {
        Commonality.Initialize();
        Mass.Initialize();
        MarketValue.Initialize();
        BeautyOffset.Initialize();
        BeautyMultiplier.Initialize();
        Flammability.Initialize();
        FlammabilityFactor.Initialize();
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

        if (valueLabel is "beautyoffset" or "all")
        {
            customBeautyOffsetKeys = new List<string>();
            customBeautyOffsetValues = new List<float>();
            CustomBeautyOffsets = new Dictionary<string, float>();
            BeautyOffset.ResetBeautyOffsetToVanillaRates();
        }

        if (valueLabel is "beautymultiplier" or "all")
        {
            customBeautyMultiplierKeys = new List<string>();
            customBeautyMultiplierValues = new List<float>();
            CustomBeautyMultipliers = new Dictionary<string, float>();
            BeautyMultiplier.ResetBeautyMultiplierToVanillaRates();
        }

        if (valueLabel is "flammability" or "all")
        {
            customFlammabilityKeys = new List<string>();
            customFlammabilityValues = new List<float>();
            CustomFlammability = new Dictionary<string, float>();
            Flammability.ResetFlammabilityToVanillaRates();
        }

        if (valueLabel is "flammabilityFactor" or "all")
        {
            customFlammabilityFactorKeys = new List<string>();
            customFlammabilityFactorValues = new List<float>();
            CustomFlammabilityFactor = new Dictionary<string, float>();
            FlammabilityFactor.ResetFlammabilityFactorToVanillaRates();
        }
    }

    public bool HasCustomValues(string type = null)
    {
        switch (type)
        {
            case null or "commonality" when CustomCommonality?.Any() == true:
            case null or "mass" when CustomMass?.Any() == true:
            case null or "marketvalue" when CustomMarketValue?.Any() == true:
            case null or "beautyoffset" when CustomBeautyOffsets?.Any() == true:
            case null or "beautymultiplier" when CustomBeautyMultipliers?.Any() == true:
            case null or "flammability" when CustomFlammability?.Any() == true:
            case null or "flammabilityFactor" when CustomFlammabilityFactor?.Any() == true:
                return true;
            default:
                return false;
        }
    }
}