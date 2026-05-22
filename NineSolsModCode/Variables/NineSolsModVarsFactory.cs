
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using NineSolsMod.NineSolsModCode.Powers;
using STS2RitsuLib.Cards.DynamicVars;

namespace NineSolsMod.NineSolsModCode.Variables;

public static class NineSolsModVarsFactory
{
    public static IEnumerable<DynamicVar> FinishVar(decimal value)
    {
        yield return ModCardVars.Int("Finish", value).WithSharedTooltip("Finish");
        yield return ModCardVars.Computed("FinishDamage", 0,
             (card, target) =>
             {
                 if (card is null || target is null) return 0m;
                 var creature = card.Owner.Creature;
                 var finishVar = card.DynamicVars["Finish"];
                 var finishMult = finishVar.BaseValue / 100m;
                 var internalAmount = target.GetPowerAmount<InternalDamagePower>();
                 return finishMult * internalAmount;
             }
         );
    }

    public static IEnumerable<DynamicVar> FinishVar(decimal baseValue, Func<CardModel?, decimal> currentValueFactory, Func<CardModel?, CardPreviewMode, Creature?, bool, decimal>? previewValueFactory = null)
    {
        yield return ModCardVars.Computed("Finish", baseValue, currentValueFactory, previewValueFactory).WithSharedTooltip("Finish");
        yield return ModCardVars.Computed("FinishDamage", 0,
             (card, target) =>
             {
                 if (card is null || target is null) return 0m;
                 var creature = card.Owner.Creature;
                 CalculatedVar finishVar = (CalculatedVar)card.DynamicVars["Finish"];
                 var finishMult = finishVar.Calculate(target) / 100m;
                 var internalAmount = target.GetPowerAmount<InternalDamagePower>();
                 return finishMult * internalAmount;
             }
         );
    }

    public static IEnumerable<DynamicVar> FinishVar(decimal baseValue, Func<CardModel?, Creature?, decimal> currentValueFactory, Func<CardModel?, CardPreviewMode, Creature?, bool, decimal>? previewValueFactory = null)
    {
        yield return ModCardVars.Computed("Finish", baseValue, currentValueFactory, previewValueFactory).WithSharedTooltip("Finish");
        yield return ModCardVars.Computed("FinishDamage", 0,
             (card, target) =>
             {
                 if (card is null || target is null) return 0m;
                 var creature = card.Owner.Creature;
                 CalculatedVar finishVar = (CalculatedVar)card.DynamicVars["Finish"];
                 var finishMult = finishVar.Calculate(target) / 100m;
                 var internalAmount = target.GetPowerAmount<InternalDamagePower>();
                 return finishMult * internalAmount;
             }
         );
    }

    public static DynamicVar OverflowVar(decimal baseValue, Func<CardModel?, decimal> currentValueFactory, Func<CardModel?, CardPreviewMode, Creature?, bool, decimal>? previewValueFactory = null)
    {
        return ModCardVars.Computed("Overflow", baseValue, currentValueFactory, previewValueFactory).WithSharedTooltip("Overflow");
    }

    public static DynamicVar OverflowVar(decimal baseValue, Func<CardModel?, Creature?, decimal> currentValueFactory, Func<CardModel?, CardPreviewMode, Creature?, bool, decimal>? previewValueFactory = null)
    {
        return ModCardVars.Computed("Overflow", baseValue, currentValueFactory, previewValueFactory).WithSharedTooltip("Overflow");
    }

    public static DynamicVar QiCostVar(decimal amount)
    {
        return ModCardVars.Int("QiCost", amount).WithSharedTooltip("QiCost");
    }


}