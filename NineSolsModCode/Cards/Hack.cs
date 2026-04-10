using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.ValueProps;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Powers;

namespace NineSolsMod.NineSolsModCode.Cards;

[Pool(typeof(YiCardPool))]
public class Hack() : NineSolsModCard(2, CardType.Skill,
    CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<ArtifactPower>(),
        HoverTipFactory.FromPower<VulnerablePower>(),
        HoverTipFactory.FromPower<StrengthPower>(),
        StunIntent.GetStaticHoverTip()
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("VulnerablePower", 2m),
        new DynamicVar("GainStrength", 4m)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var target = play.Target;

        ArgumentNullException.ThrowIfNull(target);

        if (target.HasPower<ArtifactPower>())
        {
            await PowerCmd.Remove<ArtifactPower>(target);
            await CreatureCmd.Stun(target, null);
        }
        else
        {
            await PowerCmd.Apply<VulnerablePower>(target, DynamicVars["VulnerablePower"].IntValue, Owner.Creature, this, false);
            await PowerCmd.Apply<FeedingFrenzyPower>(Owner.Creature, DynamicVars["GainStrength"].IntValue, Owner.Creature, this, false);
        }


    }

    protected override void OnUpgrade()
    {
        DynamicVars["GainStrength"].UpgradeValueBy(2);
        DynamicVars["VulnerablePower"].UpgradeValueBy(1);
    }

}