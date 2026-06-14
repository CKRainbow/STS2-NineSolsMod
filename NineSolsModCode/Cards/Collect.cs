using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using NineSolsMod.NineSolsModCode.Character;
using STS2RitsuLib.Combat.SecondaryResources;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(YiCardPool))]
public class Collect() : NineSolsModQiCard(1, CardType.Skill,
    CardRarity.Uncommon, TargetType.Self, 1)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState is null)
        {
            return;
        }

        List<CardModel> cards = [
            CombatState.CreateCard<DarkSteel>(Owner),
            CombatState.CreateCard<DarkSteel>(Owner),
        ];

        foreach (var card in cards)
        {
            if (IsUpgraded)
            {
                CardCmd.Upgrade(card, CardPreviewStyle.HorizontalLayout);
            }
        }

        var addResults = await CardPileCmd.AddGeneratedCardsToCombat(cards, PileType.Draw, Owner, CardPilePosition.Random);
        if (LocalContext.IsMe(Owner))
        {
            CardCmd.PreviewCardPileAdd(addResults, 0.6f, CardPreviewStyle.HorizontalLayout);
            await Cmd.Wait(1f, false);
        }

        var ledger = play.SecondaryResources();
        if (ledger.Activated(QiResource.OptionalQiUseId))
        {
            await PowerCmd.Apply<DrawCardsNextTurnPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade() { }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        ..base.CanonicalVars,
        new PowerVar<DrawCardsNextTurnPower>(1m)
    ];


    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromCard<DarkSteel>(IsUpgraded),
    ];
}