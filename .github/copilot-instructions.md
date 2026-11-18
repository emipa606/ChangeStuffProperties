# GitHub Copilot Instructions for Change Stuff Properties Mod

## Mod Overview and Purpose
Change Stuff Properties is a RimWorld mod that allows players to modify various properties of all stuff items, both vanilla and modded. The mod provides a wide range of customizable properties to enhance gameplay and offer more flexibility in managing resources and items. This mod is particularly useful for players who want to experiment with different in-game settings and balance their world.

## Key Features and Systems
The mod offers the following key features:
- **Commonality**: Adjust the likelihood of an item being found.
- **Mass**: Change the weight of items.
- **Market value**: Modify the value of items in trade.
- **Cleanliness offset**: Influence the cleanliness of items.
- **Beauty offset and multiplier**: Adjust the beauty rating of items.
- **Work to make/build multipliers**: Change the effort required to make or build items.
- **Sharp/Blunt damage multipliers**: Modify the damage dealt by weapons.
- **Armor power (sharp, blunt, heat)**: Adjust the protection offered by items.
- **Heat insulation and Cold insulation**: Influence the temperature resistance of items.
- **Flammability and flammability multiplier**: Change the likelihood and intensity of items catching fire.
- **Max hitpoints multiplier**: Modify the durability of items.

## Coding Patterns and Conventions
- **Encapsulation**: Each property (e.g., Commonality, Mass) is encapsulated in its own class.
- **Initialization**: Each class has an `Initialize` method that sets up the property.
- **Reset to Vanilla**: There is a method (`ResetToVanillaRates`) to revert the property values to their vanilla defaults.
- **Saving Vanilla Values**: A method (`saveVanillaValues`) is used to store the original values for resetting.
- **Setting Custom Values**: A method (`setCustomValues`) is used to apply user-defined custom values.

## XML Integration
The mod does not directly modify XML files. Instead, it uses Harmony patching to alter the behavior of the game's code at runtime. This approach avoids the need for XML changes and allows for more dynamic and flexible modifications.

## Harmony Patching
The mod uses Harmony, a popular RimWorld modding library, to patch the game's code. This allows the mod to inject custom logic into the game's existing functions without altering the original codebase. Key Harmony patches are used to:
- Modify property values during item generation and usage.
- Ensure that custom values persist across game saves and reloads.

## Suggestions for Copilot
- **Context-aware Code Snippets**: Provide snippets that are specific to RimWorld modding, particularly those involving Harmony patching and property modification.
- **Template Classes**: Offer templates for creating new property classes following the established patterns (e.g., `Commonality.cs`, `BeautyMultiplier.cs`).
- **Initialization and Reset Methods**: Generate methods for initializing and resetting properties to their default values.
- **XML and Harmony Integration**: Provide guidance on integrating Harmony patches with existing XML structures, even though the mod itself does not use XML directly.

## Example C# Code Snippet
Here is an example of a C# code snippet for creating a new property class:

csharp
using System;
using RimWorld;
using Verse;

namespace ChangeStuffProperties
{
    public class CustomProperty
    {
        public CustomProperty()
        {
            Initialize();
        }

        public void Initialize()
        {
            // Save vanilla values
            saveVanillaValues();
        }

        public void ResetToVanillaRates()
        {
            // Reset to vanilla values
            setCustomValues(vanillaValues);
        }

        private void saveVanillaValues()
        {
            // Save the original values
            vanillaValues = GetCurrentValues();
        }

        private void setCustomValues(float[] values)
        {
            // Apply custom values
            SetCurrentValues(values);
        }

        private float[] GetCurrentValues()
        {
            // Retrieve the current values
            return new float[] { /* current values */ };
        }

        private void SetCurrentValues(float[] values)
        {
            // Set the new values
        }

        private float[] vanillaValues;
    }
}


## Example Harmony Patch Snippet
Here is an example of a Harmony patch snippet:

csharp
using HarmonyLib;
using Verse;

namespace ChangeStuffProperties
{
    [HarmonyPatch(typeof(ThingDef), "GetStatValueAbstract")]
    public static class CustomPropertyPatch
    {
        public static void Postfix(ThingDef __instance, StatDef stat, ref float __result)
        {
            if (stat == StatDefOf.MarketValue)
            {
                __result *= CustomPropertyManager.GetMarketValueMultiplier(__instance);
            }
        }
    }
}


By following these instructions, GitHub Copilot can be a valuable tool for developers working on the Change Stuff Properties mod, enhancing productivity and ensuring code quality.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

