using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using NineSolsMod.NineSolsModCode.Utils;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
public class TransmuteUntoQiPower : NineSolsModPower, IFinishListener
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override Color AmountLabelColor => _normalAmountLabelColor;

    public async Task AfterFinish(AfterFinishContext context)
    {
        if (Owner == null || context.SourceCard.Owner.Creature != Owner) return;

        Flash();
        await PowerCmd.Apply<QiPower>(context.ChoiceContext ?? new ThrowingPlayerChoiceContext(), Owner, Amount, Owner, null, false);
    }
}
