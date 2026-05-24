
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using NineSolsMod.NineSolsModCode.Powers;
using STS2RitsuLib.Cards.DynamicVars;

namespace NineSolsMod.NineSolsModCode.Variables;

public static class NineSolsModVarsFactory
{
    public const string FinishKey = "NineSolsMod-Finish";
    public const string FinishDamageKey = "NineSolsMod-FinishDamage";
    public const string OverflowKey = "NineSolsMod-Overflow";
    public const string QiCostKey = "NineSolsMod-QiCost";
    public const string DeviationKey = "NineSolsMod-Deviation";
    public const string InternalDamageKey = "NineSolsMod-InternalDamage";

    public static DynamicVar OverflowVar(decimal baseValue)
    {
        return ModCardVars.Int(OverflowKey, baseValue).WithSharedTooltip(OverflowKey.ToUpperInvariant());
    }

    public static DynamicVar DeviationVar(decimal baseValue)
    {
        return ModCardVars.Int(DeviationKey, baseValue).WithSharedTooltip(DeviationKey.ToUpperInvariant());
    }

    public static DynamicVar InternalDamageVar(decimal baseValue)
    {
        return ModCardVars.Int(InternalDamageKey, baseValue).WithSharedTooltip(InternalDamageKey.ToUpperInvariant());
    }

    public static IEnumerable<DynamicVar> FinishVar(decimal value)
    {
        yield return ModCardVars.Int(FinishKey, value).WithSharedTooltip(FinishKey.ToUpperInvariant());
        yield return ModCardVars.Computed(FinishDamageKey, 0,
             (card, target) =>
             {
                 if (card is null || target is null) return 0m;
                 var creature = card.Owner.Creature;
                 var finishVar = card.DynamicVars[FinishKey];
                 var finishMult = finishVar.BaseValue / 100m;
                 var internalAmount = target.GetPowerAmount<InternalDamagePower>();
                 return finishMult * internalAmount;
             }
         );
    }

    public static IEnumerable<DynamicVar> FinishVar(decimal baseValue, Func<CardModel?, decimal> currentValueFactory, Func<CardModel?, CardPreviewMode, Creature?, bool, decimal>? previewValueFactory = null)
    {
        yield return ModCardVars.Computed(FinishKey, baseValue, currentValueFactory, previewValueFactory).WithSharedTooltip(FinishKey.ToUpperInvariant());
        yield return ModCardVars.Computed(FinishDamageKey, 0,
             (card, target) =>
             {
                 if (card is null || target is null) return 0m;
                 var creature = card.Owner.Creature;
                 CalculatedVar finishVar = (CalculatedVar)card.DynamicVars[FinishKey];
                 var finishMult = finishVar.Calculate(target) / 100m;
                 var internalAmount = target.GetPowerAmount<InternalDamagePower>();
                 return finishMult * internalAmount;
             }
         );
    }

    public static IEnumerable<DynamicVar> FinishVar(decimal baseValue, Func<CardModel?, Creature?, decimal> currentValueFactory, Func<CardModel?, CardPreviewMode, Creature?, bool, decimal>? previewValueFactory = null)
    {
        yield return ModCardVars.Computed(FinishKey, baseValue, currentValueFactory, previewValueFactory).WithSharedTooltip(FinishKey.ToUpperInvariant());
        yield return ModCardVars.Computed(FinishDamageKey, 0,
             (card, target) =>
             {
                 if (card is null || target is null) return 0m;
                 var creature = card.Owner.Creature;
                 CalculatedVar finishVar = (CalculatedVar)card.DynamicVars[FinishKey];
                 var finishMult = finishVar.Calculate(target) / 100m;
                 var internalAmount = target.GetPowerAmount<InternalDamagePower>();
                 return finishMult * internalAmount;
             }
         );
    }

    public static DynamicVar OverflowVar(decimal baseValue, Func<CardModel?, decimal> currentValueFactory, Func<CardModel?, CardPreviewMode, Creature?, bool, decimal>? previewValueFactory = null)
    {
        return ModCardVars.Computed(OverflowKey, baseValue, currentValueFactory, previewValueFactory).WithSharedTooltip(OverflowKey.ToUpperInvariant());
    }

    public static DynamicVar OverflowVar(decimal baseValue, Func<CardModel?, Creature?, decimal> currentValueFactory, Func<CardModel?, CardPreviewMode, Creature?, bool, decimal>? previewValueFactory = null)
    {
        return ModCardVars.Computed(OverflowKey, baseValue, currentValueFactory, previewValueFactory).WithSharedTooltip(OverflowKey.ToUpperInvariant());
    }

    public static DynamicVar QiCostVar(decimal amount)
    {
        return ModCardVars.Int(QiCostKey, amount).WithSharedTooltip(QiCostKey.ToUpperInvariant());
    }


}