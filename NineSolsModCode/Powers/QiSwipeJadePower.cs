using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using NineSolsMod.NineSolsModCode.Utils;
using NineSolsMod.NineSolsModCode.Variables;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
public class QiSwipeJadePower : NineSolsModPower
{
    private class Data
    {
        public readonly Dictionary<CardModel, int> amountsForPlayedCards = new Dictionary<CardModel, int>();
    }

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override Color AmountLabelColor => _normalAmountLabelColor;

    /// <summary>
    /// 伤害倍率由 Amount 控制
    /// </summary>
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DeviationVar(2m)
    ];

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (!props.IsPoweredAttack())
        {
            return 1m;
        }

        if (cardSource is null)
        {
            return 1m;
        }

        if (dealer is null)
        {
            return 1m;
        }

        if (dealer != Owner && !Owner.Pets.Contains(dealer))
        {
            return 1m;
        }

        if (target == null)
        {
            return 1m;
        }

        return (100m + Amount) / 100m;
    }

    protected override object InitInternalData()
    {
        return new Data();
    }

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player)
        {
            return Task.CompletedTask;
        }
        if (cardPlay.Card.Type != CardType.Attack)
        {
            return Task.CompletedTask;
        }

        GetInternalData<Data>().amountsForPlayedCards.Add(cardPlay.Card, Amount);
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {

        if (GetInternalData<Data>().amountsForPlayedCards.Remove(cardPlay.Card, out var value))
        {
            Flash();
            await NineSolsModCmd.Deviation(Owner, DynamicVars[DeviationVar.Key].IntValue);
        }
    }

}