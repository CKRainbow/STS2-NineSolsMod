using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace NineSolsMod.NineSolsModCode.Powers;

public class ParryPower : NineSolsModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override Color AmountLabelColor => _normalAmountLabelColor;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("InternalDamagePerAmount", 1m),
        new DynamicVar("PerfectParryMult", 2m),
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<InternalDamagePower>()
    ];

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props,
        Creature? dealer, CardModel? cardSource)
    {
        if (dealer is null)
            return;
        if (target != Owner)
            return;

        var hasUnboundedCounter = Owner.HasPower<UnboundedCounterPower>();

        if (!result.WasFullyBlocked && !hasUnboundedCounter)
            return;

        var internalDamageAmount = Amount * DynamicVars["InternalDamagePerAmount"].BaseValue;


        MainFile.Logger.Info($"target.Block: {target.Block}, result.BlockedDamage: {result.BlockedDamage}");

        if ((target.Block == 0 && result.BlockedDamage == result.TotalDamage) || hasUnboundedCounter)
        {
            internalDamageAmount *= DynamicVars["PerfectParryMult"].BaseValue;
            await PowerCmd.Apply<InternalDamagePower>(CombatState.HittableEnemies, internalDamageAmount, target, null, false);
        }
        else
        {
            await PowerCmd.Apply<InternalDamagePower>(target, internalDamageAmount, dealer, null, false);
            await PowerCmd.Apply<InternalDamagePower>(dealer, internalDamageAmount, target, null, false);
        }

        await PowerCmd.Apply<QiPower>(target, 1, null, null, true);
    }

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, CombatState combatState)
    {
        if (side == base.Owner.Side)
        {
            base.Flash();
            await PowerCmd.Remove(this);
        }
    }
}