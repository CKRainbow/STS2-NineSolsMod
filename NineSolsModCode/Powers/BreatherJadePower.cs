using Godot;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
public class BreatherJadePower : NineSolsModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override Color AmountLabelColor => _normalAmountLabelColor;

    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<InternalDamagePower>(),
    ];

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != Owner)
        {
            return;
        }

        if (cardPlay.Card.Type != CardType.Attack)
        {
            return;
        }

        var internalDamagePower = Owner.GetPower<InternalDamagePower>();
        if (internalDamagePower is null)
        {
            return;
        }

        Flash();
        await PowerCmd.ModifyAmount(choiceContext, internalDamagePower, -Amount, Owner, cardPlay.Card, false);
    }

}