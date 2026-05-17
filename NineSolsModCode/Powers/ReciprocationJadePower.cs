using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using NineSolsMod.NineSolsModCode.Utils;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
public class ReciprocationJadePower : NineSolsModPower, IParryListener
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override Color AmountLabelColor => _normalAmountLabelColor;


    public async Task AfterParry(AfterParryContext context)
    {
        if (context.ParryingCreature != Owner)
            return;

        var internalDamagePower = Owner.GetPower<InternalDamagePower>();
        if (internalDamagePower is null)
            return;

        var amountToRemove = Math.Min(Amount, internalDamagePower.Amount);

        await PowerCmd.ModifyAmount(context.ChoiceContext, internalDamagePower, -amountToRemove, Owner, context.SourceCard);
        await PowerCmd.Apply<InternalDamagePower>(context.ChoiceContext, context.AttackingCreature, amountToRemove, Owner, context.SourceCard, false);
    }

}