using Godot;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
public class TauntPower : NineSolsModPower
{
    private class Data
    {
        public MoveState? state;
    }

    // 是否会被人工阻挡？可能还需要考虑，目前先设置为不会
    public override PowerType Type => PowerType.None;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override Color AmountLabelColor => _normalAmountLabelColor;

    // 可以施加多个，每个都是独立实例
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    protected override object? InitInternalData()
    {
        return new Data();
    }

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        ArgumentNullException.ThrowIfNull(cardSource);

        if (Owner.Monster is null)
            return Task.CompletedTask;

        GetInternalData<Data>().state = Owner.Monster.NextMove;

        return Task.CompletedTask;
    }

    public override int ModifyAttackHitCount(AttackCommand attack, int hitCount)
    {
        if (Owner != attack.Attacker)
            return hitCount;
        var state = GetInternalData<Data>().state;
        if (Owner.Monster is null || state is null)
            return hitCount;
        if (state != Owner.Monster.NextMove)
            return hitCount;
        return hitCount * 2;
    }

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (Owner != dealer)
            return amount;
        var state = GetInternalData<Data>().state;
        if (Owner.Monster is null || state is null)
            return amount;
        if (state != Owner.Monster.NextMove)
            return amount;
        return amount * Amount / 100m;
    }

}