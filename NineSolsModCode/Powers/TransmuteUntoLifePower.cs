using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using NineSolsMod.NineSolsModCode.Utils;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
public class TransmuteUntoLifePower : NineSolsModPower, IFinishListener
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override Color AmountLabelColor => _normalAmountLabelColor;

    public async Task AfterFinish(AfterFinishContext context)
    {
        if (Owner == null || context.SourceCard.Owner.Creature != Owner) return;

        var internalDamagePower = Owner.GetPower<InternalDamagePower>();
        if (internalDamagePower != null && internalDamagePower.Amount > 0)
        {
            Flash();
            await PowerCmd.ModifyAmount(context.ChoiceContext ?? new ThrowingPlayerChoiceContext(), internalDamagePower, -Amount, Owner, null);
        }
    }
}
