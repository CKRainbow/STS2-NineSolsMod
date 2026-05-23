using Godot;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models.Powers;
using NineSolsMod.NineSolsModCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.HoverTips;
using NineSolsMod.NineSolsModCode.HoverTips;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
public class StatisJadePower : NineSolsModPower, IFinishListener
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override Color AmountLabelColor => _normalAmountLabelColor;

    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        NineSolsModHoverTipFactory.Finish()
    ];


    public async Task AfterFinish(AfterFinishContext context)
    {
        if (context.SourceCard.Owner.Creature == Owner)
        {
            var playerChoiceContext = context.ChoiceContext ?? new ThrowingPlayerChoiceContext();
            foreach (var target in context.DamagedTargets)
            {
                await PowerCmd.Apply<WeakPower>(playerChoiceContext, target, Amount, Owner, context.SourceCard, false);
            }
        }
    }
}