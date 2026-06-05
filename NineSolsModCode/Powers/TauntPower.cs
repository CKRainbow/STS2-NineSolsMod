using Godot;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
public class TauntPower : NineSolsModPower
{
    private class Data
    {
        public MoveState? state;
        public decimal? damageMultiplier;
    }

    // 是否会被人工阻挡？可能还需要考虑，目前先设置为不会
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override Color AmountLabelColor => _normalAmountLabelColor;

    // 可以施加多个，每个都是独立实例
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        ModCardVars.Computed("DamageMultiplier", 0m, (_) => GetInternalData<Data>().damageMultiplier ?? 0m),
    ];


    protected override object? InitInternalData()
    {
        return new Data();
    }

    public override Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
    {
        ArgumentNullException.ThrowIfNull(cardSource);

        if (target.Monster is null)
            return Task.CompletedTask;

        GetInternalData<Data>().state = target.Monster.NextMove;

        var moveId = target.Monster.NextMove.Id;
        if (moveId.EndsWith("_MOVE"))
        {
            moveId = moveId[..^"_MOVE".Length];
        }

        var locString = SmartDescription;
        locString.Add("State", new LocString("monsters", $"{target.Monster.Id.Entry}.moves.{moveId}.title"));

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
            return 1m;
        var internalData = GetInternalData<Data>();
        var state = internalData.state;
        var damageMultiplier = internalData.damageMultiplier ?? 100m;
        if (Owner.Monster is null || state is null)
            return 1m;
        if (state != Owner.Monster.NextMove)
            return 1m;
        return 1m * damageMultiplier / 100m;
    }

    public void SetDamageMultiplier(decimal damageMultiplier)
    {
        GetInternalData<Data>().damageMultiplier = damageMultiplier;
    }

}