using Godot;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using NineSolsMod.NineSolsModCode.Variables;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
public class HealthThiefJadePower : NineSolsModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override Color AmountLabelColor => _normalAmountLabelColor;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new FinishVar(100)
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        new HoverTip(
            new LocString("static_hover_tips", "NINESOLSMOD-FINISH.title"),
            new LocString("static_hover_tips", "NINESOLSMOD-FINISH.description")
        )
    ];
}