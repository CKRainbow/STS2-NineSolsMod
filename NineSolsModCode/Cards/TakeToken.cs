using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.ValueProps;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Powers;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(YiCardPool))]
public class TakeToken() : NineSolsModCard(2, CardType.Skill,
    CardRarity.Rare, TargetType.Self)
{
    // protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    // [
    //     HoverTipFactory.FromPower<ParryPower>(),
    // ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        // 将抽牌堆和弃牌堆中的所有 Token 卡加入手牌
        IEnumerable<CardModel> enumerable = PileType.Discard.GetPile(Owner).Cards.Where(Filter).Concat(PileType.Draw.GetPile(Owner).Cards.Where(Filter)).ToList();
        foreach (CardModel item in enumerable)
        {
            await CardPileCmd.Add(item, PileType.Hand);
        }
    }

    private bool Filter(CardModel card)
    {
        return card.Pool == ModelDb.CardPool<TokenCardPool>();
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

}