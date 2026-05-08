using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
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
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    protected override object? InitInternalData()
    {
        return new Data();
    }

    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
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