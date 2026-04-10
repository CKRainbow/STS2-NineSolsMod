using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace NineSolsMod.NineSolsModCode.Powers;

public class InternalDamagePower : NineSolsModPower
{
    private decimal _effectiveAmount = 0m;

    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override Color AmountLabelColor => _normalAmountLabelColor;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("DamagePerAmount", 1m)
    ];

    public override decimal ModifyHpLostAfterOsty(Creature target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource)
    {
        ArgumentNullException.ThrowIfNull(target);
        if (target != base.Owner)
        {
            return amount;
        }
        // TODO: 我们是否需要在乎这个？这个是说是否是任何伤害都可触发吗？
        // if (!props.IsPoweredAttack())
        // {
        //     return 1m;
        // }
        var damagePerAmount = base.DynamicVars["DamagePerAmount"].BaseValue;
        var internalDamageAmount = base.Amount;
        _effectiveAmount = Math.Min(internalDamageAmount, amount);
        return _effectiveAmount * damagePerAmount + amount;
    }

    public override async Task AfterModifyingHpLostAfterOsty()
    {
        // 受到攻击后，减去对应数量的层数，但好像不应该在这写
        await PowerCmd.ModifyAmount(this, -_effectiveAmount, null, null);
        _effectiveAmount = 0m;
    }

}