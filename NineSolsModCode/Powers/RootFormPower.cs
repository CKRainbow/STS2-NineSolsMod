using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
public class RootFormPower : NineSolsModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override Color AmountLabelColor => _normalAmountLabelColor;

    public override async Task BeforeTurnEndEarly(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (CombatManager.Instance.IsOverOrEnding)
            return;
        if (side != Owner.Side)
            return;
        if (Owner.IsDead)
            return;

        var internalDamagePower = Owner.GetPower<InternalDamagePower>();

        if (internalDamagePower is null)
            return;

        var internalDamageAmount = internalDamagePower.Amount;

        var effectiveAmount = Math.Min(internalDamageAmount, Amount);

        if (effectiveAmount > 0)
        {
            Flash();
            await PowerCmd.ModifyAmount(choiceContext, internalDamagePower, -effectiveAmount, Owner, null);
        }
    }

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power is not InternalDamagePower)
            return;

        if (power.Owner != Owner)
            return;

        if (Owner.IsDead)
            return;

        if (amount >= 0)
            return;

        await CreatureCmd.GainBlock(Owner, -amount, ValueProp.Unpowered, null, false);
    }
}