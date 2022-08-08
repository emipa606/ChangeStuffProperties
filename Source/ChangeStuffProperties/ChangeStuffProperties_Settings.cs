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
    public Dictionary<string, float> CustomBluntDamageMultiplier;
    private List<string> customBluntDamageMultiplierKeys;

    private List<float> customBluntDamageMultiplierValues;
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

    public Dictionary<string, float> CustomMaxHitPointsFactor;
    private List<string> customMaxHitPointsFactorKeys;
    private List<float> customMaxHitPointsFactorValues;
    public Dictionary<string, float> CustomSharpDamageMultiplier;
    private List<string> customSharpDamageMultiplierKeys;

    private List<float> customSharpDamageMultiplierValues;
    public Dictionary<string, float> CustomStuffPower_Armor_Blunt;
    private List<string> customStuffPower_Armor_BluntKeys;

    private List<float> customStuffPower_Armor_BluntValues;
    public Dictionary<string, float> CustomStuffPower_Armor_Heat;
    private List<string> customStuffPower_Armor_HeatKeys;

    private List<float> customStuffPower_Armor_HeatValues;
    public Dictionary<string, float> CustomStuffPower_Armor_Sharp;
    private List<string> customStuffPower_Armor_SharpKeys;

    private List<float> customStuffPower_Armor_SharpValues;
    public Dictionary<string, float> CustomStuffPower_Insulation_Cold;
    private List<string> customStuffPower_Insulation_ColdKeys;

    private List<float> customStuffPower_Insulation_ColdValues;
    public Dictionary<string, float> CustomStuffPower_Insulation_Heat;
    private List<string> customStuffPower_Insulation_HeatKeys;

    private List<float> customStuffPower_Insulation_HeatValues;
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
        Scribe_Collections.Look(ref CustomMaxHitPointsFactor, "CustomMaxHitPointsFactor",
            LookMode.Value,
            LookMode.Value,
            ref customMaxHitPointsFactorKeys, ref customMaxHitPointsFactorValues);
        Scribe_Collections.Look(ref CustomSharpDamageMultiplier, "CustomSharpDamageMultiplier",
            LookMode.Value,
            LookMode.Value,
            ref customSharpDamageMultiplierKeys, ref customSharpDamageMultiplierValues);
        Scribe_Collections.Look(ref CustomBluntDamageMultiplier, "CustomBluntDamageMultiplier",
            LookMode.Value,
            LookMode.Value,
            ref customBluntDamageMultiplierKeys, ref customBluntDamageMultiplierValues);
        Scribe_Collections.Look(ref CustomStuffPower_Armor_Sharp, "CustomStuffPower_Armor_Sharp",
            LookMode.Value,
            LookMode.Value,
            ref customStuffPower_Armor_SharpKeys, ref customStuffPower_Armor_SharpValues);
        Scribe_Collections.Look(ref CustomStuffPower_Armor_Sharp, "CustomStuffPower_Armor_Blunt",
            LookMode.Value,
            LookMode.Value,
            ref customStuffPower_Armor_BluntKeys, ref customStuffPower_Armor_BluntValues);
        Scribe_Collections.Look(ref CustomStuffPower_Armor_Heat, "CustomStuffPower_Armor_Heat",
            LookMode.Value,
            LookMode.Value,
            ref customStuffPower_Armor_HeatKeys, ref customStuffPower_Armor_HeatValues);
        Scribe_Collections.Look(ref CustomStuffPower_Insulation_Heat, "CustomStuffPower_Insulation_Heat",
            LookMode.Value,
            LookMode.Value,
            ref customStuffPower_Insulation_HeatKeys, ref customStuffPower_Insulation_HeatValues);
        Scribe_Collections.Look(ref CustomStuffPower_Armor_Heat, "CustomStuffPower_Insulation_Cold",
            LookMode.Value,
            LookMode.Value,
            ref customStuffPower_Insulation_ColdKeys, ref customStuffPower_Insulation_ColdValues);
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
        MaxHitPointsFactor.Initialize();
        SharpDamageMultiplier.Initialize();
        BluntDamageMultiplier.Initialize();
        StuffPower_Armor_Sharp.Initialize();
        StuffPower_Armor_Blunt.Initialize();
        StuffPower_Armor_Heat.Initialize();
        StuffPower_Insulation_Heat.Initialize();
        StuffPower_Insulation_Cold.Initialize();
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

        if (valueLabel is "flammabilityfactor" or "all")
        {
            customFlammabilityFactorKeys = new List<string>();
            customFlammabilityFactorValues = new List<float>();
            CustomFlammabilityFactor = new Dictionary<string, float>();
            FlammabilityFactor.ResetFlammabilityFactorToVanillaRates();
        }

        if (valueLabel is "maxhitpoints" or "all")
        {
            customMaxHitPointsFactorKeys = new List<string>();
            customMaxHitPointsFactorValues = new List<float>();
            CustomMaxHitPointsFactor = new Dictionary<string, float>();
            FlammabilityFactor.ResetFlammabilityFactorToVanillaRates();
        }

        if (valueLabel is "sharpdamagemultiplier" or "all")
        {
            customSharpDamageMultiplierKeys = new List<string>();
            customSharpDamageMultiplierValues = new List<float>();
            CustomSharpDamageMultiplier = new Dictionary<string, float>();
            SharpDamageMultiplier.ResetSharpDamageMultiplierToVanillaRates();
        }

        if (valueLabel is "bluntdamagemultiplier" or "all")
        {
            customBluntDamageMultiplierKeys = new List<string>();
            customBluntDamageMultiplierValues = new List<float>();
            CustomBluntDamageMultiplier = new Dictionary<string, float>();
            BluntDamageMultiplier.ResetBluntDamageMultiplierToVanillaRates();
        }

        if (valueLabel is "stuffpowerarmorsharp" or "all")
        {
            customStuffPower_Armor_SharpKeys = new List<string>();
            customStuffPower_Armor_SharpValues = new List<float>();
            CustomStuffPower_Armor_Sharp = new Dictionary<string, float>();
            StuffPower_Armor_Sharp.ResetStuffPower_Armor_SharpToVanillaRates();
        }

        if (valueLabel is "stuffpowerarmorblunt" or "all")
        {
            customStuffPower_Armor_BluntKeys = new List<string>();
            customStuffPower_Armor_BluntValues = new List<float>();
            CustomStuffPower_Armor_Blunt = new Dictionary<string, float>();
            StuffPower_Armor_Blunt.ResetStuffPower_Armor_BluntToVanillaRates();
        }

        if (valueLabel is "stuffpowerarmorheat" or "all")
        {
            customStuffPower_Armor_HeatKeys = new List<string>();
            customStuffPower_Armor_HeatValues = new List<float>();
            CustomStuffPower_Armor_Heat = new Dictionary<string, float>();
            StuffPower_Armor_Heat.ResetStuffPower_Armor_HeatToVanillaRates();
        }

        if (valueLabel is "stuffpowerinsulationheat" or "all")
        {
            customStuffPower_Insulation_HeatKeys = new List<string>();
            customStuffPower_Insulation_HeatValues = new List<float>();
            CustomStuffPower_Insulation_Heat = new Dictionary<string, float>();
            StuffPower_Insulation_Heat.ResetStuffPower_Insulation_HeatToVanillaRates();
        }

        if (valueLabel is "stuffpowerinsulationcold" or "all")
        {
            customStuffPower_Insulation_ColdKeys = new List<string>();
            customStuffPower_Insulation_ColdValues = new List<float>();
            CustomStuffPower_Insulation_Cold = new Dictionary<string, float>();
            StuffPower_Insulation_Cold.ResetStuffPower_Insulation_ColdToVanillaRates();
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
            case null or "flammabilityfactor" when CustomFlammabilityFactor?.Any() == true:
            case null or "maxhitpointsfactor" when CustomMaxHitPointsFactor?.Any() == true:
            case null or "sharpdamagemultiplier" when CustomSharpDamageMultiplier?.Any() == true:
            case null or "bluntdamagemultiplier" when CustomBluntDamageMultiplier?.Any() == true:
            case null or "stuffpowerarmorsharp" when CustomStuffPower_Armor_Sharp?.Any() == true:
            case null or "stuffpowerarmorblunt" when CustomStuffPower_Armor_Blunt?.Any() == true:
            case null or "stuffpowerarmorheat" when CustomStuffPower_Armor_Heat?.Any() == true:
            case null or "stuffpowerarmorheat" when CustomStuffPower_Insulation_Heat?.Any() == true:
            case null or "stuffpowerarmorheat" when CustomStuffPower_Insulation_Cold?.Any() == true:
                return true;
            default:
                return false;
        }
    }
}