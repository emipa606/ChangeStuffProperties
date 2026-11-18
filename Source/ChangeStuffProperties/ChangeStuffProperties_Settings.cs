using System.Collections.Generic;
using System.Linq;
using Verse;

namespace ChangeStuffProperties;

public class ChangeStuffProperties_Settings : ModSettings
{
    public Dictionary<string, float> CustomBeautyMultipliers;
    public Dictionary<string, float> CustomBeautyOffsets;
    public Dictionary<string, float> CustomBluntDamageMultiplier;
    public Dictionary<string, float> CustomCleanlinessOffsets;
    public Dictionary<string, float> CustomCommonality;
    public Dictionary<string, float> CustomFlammability;
    public Dictionary<string, float> CustomFlammabilityFactor;
    public Dictionary<string, float> CustomMarketValue;
    public Dictionary<string, float> CustomMass;

    public Dictionary<string, float> CustomMaxHitPointsFactor;
    public Dictionary<string, float> CustomSharpDamageMultiplier;
    public Dictionary<string, float> CustomStuffPower_Armor_Blunt;
    public Dictionary<string, float> CustomStuffPower_Armor_Heat;
    public Dictionary<string, float> CustomStuffPower_Armor_Sharp;
    public Dictionary<string, float> CustomStuffPower_Insulation_Cold;
    public Dictionary<string, float> CustomStuffPower_Insulation_Heat;

    public Dictionary<string, float> CustomWorkToBuildMultipliers;

    public Dictionary<string, float> CustomWorkToMakeMultipliers;

    public bool VerboseLogging;
    private List<string> customBeautyMultipliersKeys;

    private List<float> customBeautyMultipliersValues;
    private List<string> customBeautyOffsetsKeys;

    private List<float> customBeautyOffsetsValues;
    private List<string> customBluntDamageMultiplierKeys;

    private List<float> customBluntDamageMultiplierValues;
    private List<string> customCleanlinessOffsetsKeys;

    private List<float> customCleanlinessOffsetsValues;

    private List<string> customCommonalityKeys;

    private List<float> customCommonalityValues;
    private List<string> customFlammabilityFactorKeys;

    private List<float> customFlammabilityFactorValues;
    private List<string> customFlammabilityKeys;

    private List<float> customFlammabilityValues;

    private List<string> customMarketValueKeys;

    private List<float> customMarketValueValues;
    private List<string> customMassKeys;

    private List<float> customMassValues;
    private List<string> customMaxHitPointsFactorKeys;
    private List<float> customMaxHitPointsFactorValues;
    private List<string> customSharpDamageMultiplierKeys;

    private List<float> customSharpDamageMultiplierValues;
    private List<string> customStuffPower_Armor_BluntKeys;

    private List<float> customStuffPower_Armor_BluntValues;
    private List<string> customStuffPower_Armor_HeatKeys;

    private List<float> customStuffPower_Armor_HeatValues;
    private List<string> customStuffPower_Armor_SharpKeys;

    private List<float> customStuffPower_Armor_SharpValues;
    private List<string> customStuffPower_Insulation_ColdKeys;

    private List<float> customStuffPower_Insulation_ColdValues;
    private List<string> customStuffPower_Insulation_HeatKeys;

    private List<float> customStuffPower_Insulation_HeatValues;
    private List<string> customWorkToBuildMultipliersKeys;
    private List<float> customWorkToBuildMultipliersValues;
    private List<string> customWorkToMakeMultipliersKeys;
    private List<float> customWorkToMakeMultipliersValues;

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
        Scribe_Collections.Look(ref CustomCleanlinessOffsets, "CustomCleanlinessOffsets",
            LookMode.Value,
            LookMode.Value,
            ref customCleanlinessOffsetsKeys, ref customCleanlinessOffsetsValues);
        Scribe_Collections.Look(ref CustomBeautyOffsets, "CustomBeautyOffsets",
            LookMode.Value,
            LookMode.Value,
            ref customBeautyOffsetsKeys, ref customBeautyOffsetsValues);
        Scribe_Collections.Look(ref CustomBeautyMultipliers, "CustomBeautyMultipliers",
            LookMode.Value,
            LookMode.Value,
            ref customBeautyMultipliersKeys, ref customBeautyMultipliersValues);
        Scribe_Collections.Look(ref CustomWorkToBuildMultipliers, "CustomWorkToBuildMultipliers",
            LookMode.Value,
            LookMode.Value,
            ref customWorkToBuildMultipliersKeys, ref customWorkToBuildMultipliersValues);
        Scribe_Collections.Look(ref CustomWorkToMakeMultipliers, "CustomWorkToMakeMultipliers",
            LookMode.Value,
            LookMode.Value,
            ref customWorkToMakeMultipliersKeys, ref customWorkToMakeMultipliersValues);
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
        Scribe_Collections.Look(ref CustomStuffPower_Armor_Blunt, "CustomStuffPower_Armor_Blunt",
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
        Scribe_Collections.Look(ref CustomStuffPower_Insulation_Cold, "CustomStuffPower_Insulation_Cold",
            LookMode.Value,
            LookMode.Value,
            ref customStuffPower_Insulation_ColdKeys, ref customStuffPower_Insulation_ColdValues);
    }

    public static void Initialize()
    {
        Commonality.Initialize();
        Mass.Initialize();
        MarketValue.Initialize();
        CleanlinessOffset.Initialize();
        BeautyOffset.Initialize();
        BeautyMultiplier.Initialize();
        WorkToBuildMultiplier.Initialize();
        WorkToMakeMultiplier.Initialize();
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
            customCommonalityKeys = [];
            customCommonalityValues = [];
            CustomCommonality = new Dictionary<string, float>();
            Commonality.ResetCommonalityToVanillaRates();
        }

        if (valueLabel is "mass" or "all")
        {
            customMassKeys = [];
            customMassValues = [];
            CustomMass = new Dictionary<string, float>();
            Mass.ResetMassToVanillaRates();
        }

        if (valueLabel is "marketvalue" or "all")
        {
            customMarketValueKeys = [];
            customMarketValueValues = [];
            CustomMarketValue = new Dictionary<string, float>();
            MarketValue.ResetMarketValueToVanillaRates();
        }

        if (valueLabel is "cleanlinessoffset" or "all")
        {
            customCleanlinessOffsetsKeys = [];
            customCleanlinessOffsetsValues = [];
            CustomCleanlinessOffsets = new Dictionary<string, float>();
            CleanlinessOffset.ResetCleanlinessOffsetToVanillaRates();
        }

        if (valueLabel is "beautyoffset" or "all")
        {
            customBeautyOffsetsKeys = [];
            customBeautyOffsetsValues = [];
            CustomBeautyOffsets = new Dictionary<string, float>();
            BeautyOffset.ResetBeautyOffsetToVanillaRates();
        }

        if (valueLabel is "beautymultiplier" or "all")
        {
            customBeautyMultipliersKeys = [];
            customBeautyMultipliersValues = [];
            CustomBeautyMultipliers = new Dictionary<string, float>();
            BeautyMultiplier.ResetBeautyMultiplierToVanillaRates();
        }

        if (valueLabel is "worktobuildmultiplier" or "all")
        {
            customWorkToBuildMultipliersKeys = [];
            customWorkToBuildMultipliersValues = [];
            CustomWorkToBuildMultipliers = new Dictionary<string, float>();
            WorkToBuildMultiplier.ResetWorkToBuildMultiplierToVanillaRates();
        }

        if (valueLabel is "worktomakemultiplier" or "all")
        {
            customWorkToMakeMultipliersKeys = [];
            customWorkToMakeMultipliersValues = [];
            CustomWorkToMakeMultipliers = new Dictionary<string, float>();
            WorkToMakeMultiplier.ResetWorkToMakeMultiplierToVanillaRates();
        }

        if (valueLabel is "flammability" or "all")
        {
            customFlammabilityKeys = [];
            customFlammabilityValues = [];
            CustomFlammability = new Dictionary<string, float>();
            Flammability.ResetFlammabilityToVanillaRates();
        }

        if (valueLabel is "flammabilityfactor" or "all")
        {
            customFlammabilityFactorKeys = [];
            customFlammabilityFactorValues = [];
            CustomFlammabilityFactor = new Dictionary<string, float>();
            FlammabilityFactor.ResetFlammabilityFactorToVanillaRates();
        }

        if (valueLabel is "maxhitpointsfactor" or "all")
        {
            customMaxHitPointsFactorKeys = [];
            customMaxHitPointsFactorValues = [];
            CustomMaxHitPointsFactor = new Dictionary<string, float>();
            MaxHitPointsFactor.ResetMaxHitPointsFactorToVanillaRates();
        }

        if (valueLabel is "sharpdamagemultiplier" or "all")
        {
            customSharpDamageMultiplierKeys = [];
            customSharpDamageMultiplierValues = [];
            CustomSharpDamageMultiplier = new Dictionary<string, float>();
            SharpDamageMultiplier.ResetSharpDamageMultiplierToVanillaRates();
        }

        if (valueLabel is "bluntdamagemultiplier" or "all")
        {
            customBluntDamageMultiplierKeys = [];
            customBluntDamageMultiplierValues = [];
            CustomBluntDamageMultiplier = new Dictionary<string, float>();
            BluntDamageMultiplier.ResetBluntDamageMultiplierToVanillaRates();
        }

        if (valueLabel is "stuffpowerarmorsharp" or "all")
        {
            customStuffPower_Armor_SharpKeys = [];
            customStuffPower_Armor_SharpValues = [];
            CustomStuffPower_Armor_Sharp = new Dictionary<string, float>();
            StuffPower_Armor_Sharp.ResetStuffPower_Armor_SharpToVanillaRates();
        }

        if (valueLabel is "stuffpowerarmorblunt" or "all")
        {
            customStuffPower_Armor_BluntKeys = [];
            customStuffPower_Armor_BluntValues = [];
            CustomStuffPower_Armor_Blunt = new Dictionary<string, float>();
            StuffPower_Armor_Blunt.ResetStuffPower_Armor_BluntToVanillaRates();
        }

        if (valueLabel is "stuffpowerarmorheat" or "all")
        {
            customStuffPower_Armor_HeatKeys = [];
            customStuffPower_Armor_HeatValues = [];
            CustomStuffPower_Armor_Heat = new Dictionary<string, float>();
            StuffPower_Armor_Heat.ResetStuffPower_Armor_HeatToVanillaRates();
        }

        if (valueLabel is "stuffpowerinsulationheat" or "all")
        {
            customStuffPower_Insulation_HeatKeys = [];
            customStuffPower_Insulation_HeatValues = [];
            CustomStuffPower_Insulation_Heat = new Dictionary<string, float>();
            StuffPower_Insulation_Heat.ResetStuffPower_Insulation_HeatToVanillaRates();
        }

        if (valueLabel is not ("stuffpowerinsulationcold" or "all"))
        {
            return;
        }

        customStuffPower_Insulation_ColdKeys = [];
        customStuffPower_Insulation_ColdValues = [];
        CustomStuffPower_Insulation_Cold = new Dictionary<string, float>();
        StuffPower_Insulation_Cold.ResetStuffPower_Insulation_ColdToVanillaRates();
    }

    public bool HasCustomValues(string type = null)
    {
        switch (type)
        {
            case null or "commonality" when CustomCommonality?.Any() == true:
            case null or "mass" when CustomMass?.Any() == true:
            case null or "marketvalue" when CustomMarketValue?.Any() == true:
            case null or "cleanlinessoffset" when CustomCleanlinessOffsets?.Any() == true:
            case null or "beautyoffset" when CustomBeautyOffsets?.Any() == true:
            case null or "beautymultiplier" when CustomBeautyMultipliers?.Any() == true:
            case null or "worktobuildmultiplier" when CustomWorkToBuildMultipliers?.Any() == true:
            case null or "worktomakemultiplier" when CustomWorkToMakeMultipliers?.Any() == true:
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