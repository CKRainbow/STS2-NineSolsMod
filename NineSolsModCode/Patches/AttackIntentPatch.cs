using System.Collections.Generic;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;

namespace NineSolsMod.NineSolsModCode.Patches;

[HarmonyPatch]
public static class AttackIntentPatch
{
    private static int GetModifiedHitCount(AttackIntent intent, Creature owner)
    {
        if (owner.Monster == null || owner.CombatState == null) return intent.Repeats;
        var dummyCmd = new AttackCommand(0).FromMonster(owner.Monster);
        return (int)Hook.ModifyAttackHitCount(owner.CombatState, dummyCmd, intent.Repeats);
    }

    [HarmonyPatch(typeof(AttackIntent), "GetIntentDescription")]
    [HarmonyPostfix]
    public static void AttackIntent_GetIntentDescription_Postfix(AttackIntent __instance, ref LocString __result, IEnumerable<Creature> targets, Creature owner)
    {
        int modifiedHitCount = GetModifiedHitCount(__instance, owner);
        if (modifiedHitCount != __instance.Repeats)
        {
            __result.Add("Repeat", modifiedHitCount);
        }
    }

    [HarmonyPatch(typeof(SingleAttackIntent), nameof(SingleAttackIntent.GetIntentLabel))]
    [HarmonyPostfix]
    public static void SingleAttackIntent_GetIntentLabel_Postfix(SingleAttackIntent __instance, ref LocString __result, IEnumerable<Creature> targets, Creature owner)
    {
        int modifiedHitCount = GetModifiedHitCount(__instance, owner);
        if (modifiedHitCount > 1)
        {
            LocString intentLabelFormat = new LocString("intents", "FORMAT_DAMAGE_MULTI");
            float num = __instance.GetSingleDamage(targets, owner);
            intentLabelFormat.Add("Damage", (int)num);
            intentLabelFormat.Add("Repeat", modifiedHitCount);
            __result = intentLabelFormat;
        }
    }

    [HarmonyPatch(typeof(SingleAttackIntent), nameof(SingleAttackIntent.GetTotalDamage))]
    [HarmonyPostfix]
    public static void SingleAttackIntent_GetTotalDamage_Postfix(SingleAttackIntent __instance, ref int __result, IEnumerable<Creature> targets, Creature owner)
    {
        int modifiedHitCount = GetModifiedHitCount(__instance, owner);
        if (modifiedHitCount != __instance.Repeats)
        {
            __result = __instance.GetSingleDamage(targets, owner) * modifiedHitCount;
        }
    }

    [HarmonyPatch(typeof(MultiAttackIntent), nameof(MultiAttackIntent.GetIntentLabel))]
    [HarmonyPostfix]
    public static void MultiAttackIntent_GetIntentLabel_Postfix(MultiAttackIntent __instance, ref LocString __result, IEnumerable<Creature> targets, Creature owner)
    {
        int modifiedHitCount = GetModifiedHitCount(__instance, owner);
        if (modifiedHitCount == 1)
        {
            LocString intentLabelFormat = new LocString("intents", "FORMAT_DAMAGE_SINGLE");
            float num = __instance.GetSingleDamage(targets, owner);
            intentLabelFormat.Add("Damage", (int)num);
            __result = intentLabelFormat;
        }
        else if (modifiedHitCount != __instance.Repeats)
        {
            __result.Add("Repeat", modifiedHitCount);
        }
    }

    [HarmonyPatch(typeof(MultiAttackIntent), nameof(MultiAttackIntent.GetTotalDamage))]
    [HarmonyPostfix]
    public static void MultiAttackIntent_GetTotalDamage_Postfix(MultiAttackIntent __instance, ref int __result, IEnumerable<Creature> targets, Creature owner)
    {
        int modifiedHitCount = GetModifiedHitCount(__instance, owner);
        if (modifiedHitCount != __instance.Repeats)
        {
            __result = __instance.GetSingleDamage(targets, owner) * modifiedHitCount;
        }
    }
}
