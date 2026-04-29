using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.ValueProps;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Powers;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(YiCardPool))]
public class Insight() : NineSolsModCard(2, CardType.Skill,
    CardRarity.Uncommon, TargetType.AnyEnemy)
{
    // TODO: 这个是否可以改成非固定值
    public override bool GainsBlock => true;

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<ParryPower>(),
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("TempAgile", 3m),
        new DynamicVar("ParryPower", 3m)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var target = play.Target;
        ArgumentNullException.ThrowIfNull(target);

        var monster = target.Monster;
        ArgumentNullException.ThrowIfNull(monster);

        if (monster.IntendsToAttack)
        {
            var intents = monster.NextMove.Intents;
            var attackTotal = 0m;
            foreach (var intent in intents)
            {
                if (intent is AttackIntent attackIntent)
                {
                    if (attackIntent.DamageCalc is not null)
                    {
                        attackTotal += attackIntent.GetTotalDamage([Owner.Creature], target);
                    }
                }
            }

            await CreatureCmd.GainBlock(Owner.Creature, attackTotal, ValueProp.Move, play, false);
            await PowerCmd.Apply<ParryPower>(choiceContext, Owner.Creature, DynamicVars["ParryPower"].BaseValue, Owner.Creature, this, false);
        }
        else
        {

        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["TempAgile"].UpgradeValueBy(2m);
    }

}