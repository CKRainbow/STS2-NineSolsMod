using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using NineSolsMod.NineSolsModCode.HoverTips;
using NineSolsMod.NineSolsModCode.Powers;
using NineSolsMod.NineSolsModCode.Tags;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(TokenCardPool))]
public class TransmuteUntoWealth() : NineSolsModCard(0, CardType.Power, CardRarity.Token, TargetType.Self), IChoosable
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<TransmuteUntoWealthPower>(choiceContext, Owner.Creature, DynamicVars["GoldAmount"].BaseValue, Owner.Creature, this, false);
    }

    public async Task OnChoose(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<TransmuteUntoWealthPower>(choiceContext, Owner.Creature, DynamicVars["GoldAmount"].BaseValue, Owner.Creature, this, false);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("GoldAmount", 30m),
    ];

    protected override HashSet<CardTag> CanonicalTags => [
        NineSolsModTags.TailsmanTransmutation.GetModCardTag()
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        NineSolsModHoverTipFactory.Finish()
    ];
}
