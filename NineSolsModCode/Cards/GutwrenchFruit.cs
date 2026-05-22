using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using NineSolsMod.NineSolsModCode.Character;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(YiCardPool))]
public class GutwrenchFruit() : NineSolsModCard(1, CardType.Skill,
    CardRarity.Common, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target, nameof(play.Target));
        await PowerCmd.Apply<VulnerablePower>(choiceContext, play.Target, DynamicVars["VulnerablePower"].BaseValue, Owner.Creature, this, false);
        await PowerCmd.Apply<WeakPower>(choiceContext, play.Target, DynamicVars["WeakPower"].BaseValue, Owner.Creature, this, false);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["VulnerablePower"].UpgradeValueBy(1m);
        DynamicVars["WeakPower"].UpgradeValueBy(1m);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<VulnerablePower>(1m),
        new PowerVar<WeakPower>(2m),
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<VulnerablePower>(),
        HoverTipFactory.FromPower<WeakPower>()
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust,
        CardKeyword.Retain
    ];
}