using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using NineSolsMod.NineSolsModCode.Tags;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(TokenCardPool))]
public class AzureSand() : NineSolsModCard(0, CardType.Skill,
    CardRarity.Token, TargetType.Self)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState is null)
            return;

        var azureCards = Owner.Character.CardPool.GetUnlockedCards(
            Owner.UnlockState,
            Owner.RunState.CardMultiplayerConstraint
        ).Where(
            card => card.Tags.Contains(NineSolsModTags.AzureSandCraft.GetModCardTag())
        ).ToList();

        if (IsUpgraded)
        {
            foreach (var card in azureCards)
            {
                CardCmd.Upgrade(card, CardPreviewStyle.HorizontalLayout);
            }
        }

        CardModel? cardModel = await CardSelectCmd.FromChooseACardScreen(choiceContext, azureCards, Owner, false);
        if (cardModel != null)
        {
            await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, Owner, CardPilePosition.Bottom);
        }
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromCard<CloudPiercingArrow>(IsUpgraded),
        HoverTipFactory.FromCard<ThunderBusterArrow>(IsUpgraded),
        HoverTipFactory.FromCard<ShadowHunterArrow>(IsUpgraded),
        HoverTipFactory.FromCard<AzureSandArmor>(IsUpgraded )
    ];
}