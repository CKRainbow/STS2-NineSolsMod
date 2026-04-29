using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using NineSolsMod.NineSolsModCode.Keywords;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(TokenCardPool))]
public class AzureSandArmor() : NineSolsModCard(1, CardType.Skill,
    CardRarity.Token, TargetType.Self)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<PlatingPower>(choiceContext, Owner.Creature, DynamicVars["PlatingPower"].IntValue, Owner.Creature, this, false);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["PlatingPower"].UpgradeValueBy(2);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<PlatingPower>(6)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust,
        CardKeyword.Retain
    ];

    protected override IEnumerable<string> RegisteredKeywordIds => [
        NineSolsModKeywords.AzureSandCraft
    ];


    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<PlatingPower>()
    ];
}