using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.ValueProps;
using NineSolsMod.NineSolsModCode.Cards;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Powers;
using NineSolsMod.NineSolsModCode.Tags;

namespace NineSolsMod.NineSolsModCode.Cards;

[Pool(typeof(TokenCardPool))]
public class AzureSand() : NineSolsModCard(0, CardType.Skill,
    CardRarity.Token, TargetType.Self)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState is null)
            return;

        List<CardModel> azureCards = [
            CombatState.CreateCard<CloudPiercingArrow>(Owner),
            CombatState.CreateCard<ThunderBusterArrow>(Owner),
            CombatState.CreateCard<ShadowHunterArrow>(Owner),
            CombatState.CreateCard<AzureSandArmor>(Owner)
        ];

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
            await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, true, CardPilePosition.Bottom);
        }
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromCard<CloudPiercingArrow>(IsUpgraded),
        HoverTipFactory.FromCard<ThunderBusterArrow>(IsUpgraded),
        HoverTipFactory.FromCard<ShadowHunterArrow>(IsUpgraded),
        HoverTipFactory.FromCard<AzureSandArmor>(IsUpgraded )
    ];
}