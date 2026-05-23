
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace NineSolsMod.NineSolsModCode.HoverTips;

public static class NineSolsModHoverTipFactory
{
    public static IHoverTip Finish()
    {
        var titleLoc = new LocString("static_hover_tips", "Finish.title");
        var descriptionLoc = new LocString("static_hover_tips", "Finish.description");
        var dummyVar = new DynamicVar("Finish", 0m);
        titleLoc.Add(dummyVar);
        descriptionLoc.Add(dummyVar);
        return new HoverTip(titleLoc, descriptionLoc);
    }
}