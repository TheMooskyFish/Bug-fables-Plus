using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;

namespace BFPlus.Patches.BattleControlTranspilers
{
    /// <summary>
    /// Always set commandsuccess to true for DoCommand's LongRandomBar for COMMAND menucode<br/>
    /// Note that it will always succeed even if failing one or all inputs
    /// </summary>
    [HarmonyPatch(typeof(BattleControl), "DoCommand", MethodType.Enumerator)]
    public class PatchDoCommandLongRandomBar
    {
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
            .MatchForward(true,
                new CodeMatch(OpCodes.Ldc_R4, 1f),
                new CodeMatch(OpCodes.Blt),
                new CodeMatch(OpCodes.Br))
            .InsertAndAdvance(
                new CodeInstruction(OpCodes.Ldloc_1),
                new CodeInstruction(OpCodes.Ldc_I4_1),
                new CodeInstruction(OpCodes.Stfld, 
                    AccessTools.Field(typeof(BattleControl), nameof(BattleControl.commandsuccess))))
            .InstructionEnumeration();
        }
    }
}