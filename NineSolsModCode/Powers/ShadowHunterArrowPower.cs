using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;
using NineSolsMod.NineSolsModCode.Cards;

namespace NineSolsMod.NineSolsModCode.Powers;

public class ShadowHunterArrowPower : NineSolsModPower
{
    private class Data
    {
        public CardModel? relatedCard;
    }

    // 是否会被人工阻挡？可能还需要考虑，目前先设置为不会
    public override PowerType Type => PowerType.None;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override Color AmountLabelColor => _normalAmountLabelColor;

    // 可以施加多个，每个都是独立实例
    public override bool IsInstanced => true;

    protected override object? InitInternalData()
    {
        return new Data();
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromCard(GetInternalData<Data>().relatedCard!)
    ];

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        ArgumentNullException.ThrowIfNull(cardSource);

        GetInternalData<Data>().relatedCard = cardSource;

        return Task.CompletedTask;
    }

    public bool MatchCard(CardModel card)
    {
        return GetInternalData<Data>().relatedCard == card;
    }
}