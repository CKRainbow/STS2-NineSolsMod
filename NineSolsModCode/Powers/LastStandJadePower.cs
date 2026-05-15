using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
public class LastStandJadePower : NineSolsModPower
{
    // TODO: 最好能够在满足要求时有一些特效，像遗物一样，不过目前好像还没有这个功能
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override Color AmountLabelColor => _normalAmountLabelColor;

    private bool IsActivated => (Owner.MaxHp - Owner.CurrentHp - Owner.GetPowerAmount<InternalDamagePower>()) <= 0.5m * Owner.MaxHp;

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (!props.IsPoweredAttack())
        {
            return 1m;
        }

        if (cardSource is null)
        {
            return 1m;
        }

        if (dealer is null)
        {
            return 1m;
        }

        if (dealer != Owner && !Owner.Pets.Contains(dealer))
        {
            return 1m;
        }

        if (target == null)
        {
            return 1m;
        }

        if (!IsActivated)
        {
            return 1m;
        }

        return (100m + Amount) / 100m;
    }
}