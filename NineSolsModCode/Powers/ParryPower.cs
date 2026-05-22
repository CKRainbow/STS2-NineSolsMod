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
using NineSolsMod.NineSolsModCode.Utils;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
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

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
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
        if (props != ValueProp.Move)
            return;

        var hasUnboundedCounter = Owner.HasPower<UnboundedCounterPower>();

        if (!result.WasFullyBlocked && !hasUnboundedCounter)
            return;

        var internalDamageAmount = Amount * DynamicVars["InternalDamagePerAmount"].BaseValue;

        MainFile.Logger.Info($"target.Block: {target.Block}, result.BlockedDamage: {result.BlockedDamage}");

        var isPerfectParry = target.Block == 0 && result.BlockedDamage == result.TotalDamage || hasUnboundedCounter;

        if (isPerfectParry)
        {
            internalDamageAmount *= DynamicVars["PerfectParryMult"].BaseValue;
            await PowerCmd.Apply<InternalDamagePower>(choiceContext, CombatState.HittableEnemies, internalDamageAmount, target, null, false);
        }
        else
        {
            await PowerCmd.Apply<InternalDamagePower>(choiceContext, target, internalDamageAmount, target, null, false);
            await PowerCmd.Apply<InternalDamagePower>(choiceContext, dealer, internalDamageAmount, target, null, false);
        }

        await PowerCmd.Apply<QiPower>(choiceContext, target, 1, target, null, true);

        await Hooks.ParryHook.AfterParry(CombatState, new AfterParryContext
        {
            ParryingCreature = target,
            AttackingCreature = dealer,
            SourceCard = cardSource,
            IsPerfectParry = isPerfectParry,
            ChoiceContext = choiceContext,
            DamageResult = result,
        });
    }

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (participants.Contains(Owner))
        {
            Flash();
            await PowerCmd.Remove(this);
        }
    }
}