
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using NineSolsMod.NineSolsModCode.Variables;

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
}