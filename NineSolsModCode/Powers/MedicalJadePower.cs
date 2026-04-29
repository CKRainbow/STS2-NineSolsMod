using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
public class MedicalJadePower : NineSolsModPower//, IHealAmountModifier
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override Color AmountLabelColor => _normalAmountLabelColor;

    // FIXME: 目前先不实现，期待未来的API支持
    // public decimal ModifyHealMultiplicative(Creature creature, decimal amount)
    // {
    //     if (creature != Owner)
    //     {
    //         return 1m;
    //     }
    //     return 1m + Amount / 100m;
    // }

}