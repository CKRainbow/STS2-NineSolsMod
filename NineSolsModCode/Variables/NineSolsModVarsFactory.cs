
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using NineSolsMod.NineSolsModCode.Variables;
using STS2RitsuLib.Cards.DynamicVars;

namespace NineSolsMod.NineSolsModCode.Variables;

public static class NineSolsModVarsFactory
{
    public static DynamicVar FinishVar(decimal baseValue, Func<CardModel?, decimal> currentValueFactory, Func<CardModel?, CardPreviewMode, Creature?, bool, decimal>? previewValueFactory = null)
    {
        return ModCardVars.Computed("Finish", baseValue, currentValueFactory, previewValueFactory).WithSharedTooltip("Finish");
    }

    public static DynamicVar FinishVar(decimal baseValue, Func<CardModel?, Creature?, decimal> currentValueFactory, Func<CardModel?, CardPreviewMode, Creature?, bool, decimal>? previewValueFactory = null)
    {
        return ModCardVars.Computed("Finish", baseValue, currentValueFactory, previewValueFactory).WithSharedTooltip("Finish");
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