using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Tags;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(YiCardPool))]
public class TailsmanTransmutation() : NineSolsModCard(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (Owner.Creature.CombatState is null)
            return;

        var transmutationCards = ModelDb.AllCards.Where(
            card => card.Tags.Contains(NineSolsModTags.TailsmanTransmutation.GetModCardTag()) && card is IChoosable
        ).Select(card => Owner.Creature.CombatState.CreateCard(card, Owner)).ToList();

        CardModel? cardModel = (await CardSelectCmd.FromSimpleGrid(
            choiceContext,
            transmutationCards,
            Owner,
            new CardSelectorPrefs(SelectionScreenPrompt, 1)
        )).ToList().FirstOrDefault();
        if (cardModel != null && cardModel is IChoosable choosableCard)
        {
            await choosableCard.OnChoose(choiceContext, play);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    protected override IEnumerable<IHoverTip> AdditionalHoverTips
    {
        get
        {
            var transmutationCards = ModelDb.AllCards.Where(
                card => card.Tags.Contains(NineSolsModTags.TailsmanTransmutation.GetModCardTag())
            ).ToList();
            foreach (var card in transmutationCards)
            {
                yield return HoverTipFactory.FromCard(card);
            }
        }
    }

}