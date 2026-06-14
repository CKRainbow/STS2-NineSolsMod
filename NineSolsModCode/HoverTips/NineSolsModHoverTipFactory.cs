
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using NineSolsMod.NineSolsModCode.Variables;
using STS2RitsuLib.Combat.SecondaryResources;

namespace NineSolsMod.NineSolsModCode.HoverTips;

public static class NineSolsModHoverTipFactory
{
    public static IHoverTip Finish()
    {
        var titleLoc = new LocString("static_hover_tips", "NINESOLSMOD-FINISH.title");
        var descriptionLoc = new LocString("static_hover_tips", "NINESOLSMOD-FINISH.description");
        var dummyVar = new DynamicVar(NineSolsModVarsFactory.FinishKey, 0m);
        titleLoc.Add(dummyVar);
        descriptionLoc.Add(dummyVar);
        return new HoverTip(titleLoc, descriptionLoc);
    }

    public static IHoverTip Qi(AbstractModel @abstract)
    {
        if (@abstract is CardModel card)
        {
            return ModSecondaryResourceRegistry.CreateHoverTip(QiResource.QiId, card.IsInCombat ? SecondaryResourceCmd.Get(card.Owner, QiResource.QiId) : 0);
        }
        else if (@abstract is PowerModel power)
        {
            return ModSecondaryResourceRegistry.CreateHoverTip(QiResource.QiId, power.Owner.IsPlayer ? SecondaryResourceCmd.Get(power.Owner.Player!, QiResource.QiId) : 0);
        }
        else if (@abstract is PotionModel potion)
        {
            return ModSecondaryResourceRegistry.CreateHoverTip(QiResource.QiId, SecondaryResourceCmd.Get(potion.Owner, QiResource.QiId));
        }
        else
        {
            return ModSecondaryResourceRegistry.CreateHoverTip(QiResource.QiId, 0);
        }
    }
}