
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using NineSolsMod.NineSolsModCode.Variables;
using STS2RitsuLib.Cards.DynamicVars;

public static class NineSolsModVarsFactory
{
    public static DynamicVar FinishVar(decimal baseValue, Func<CardModel?, decimal> currentValueFactory, Func<CardModel?, CardPreviewMode, Creature?, bool, decimal>? previewValueFactory = null)
    {
        return ModCardVars.Computed("Finish", baseValue, currentValueFactory, previewValueFactory).WithSharedTooltip("Finish");
    }
}