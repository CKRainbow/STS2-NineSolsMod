using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Powers;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(YiCardPool))]
public class Taunt() : NineSolsModCard(1, CardType.Skill,
    CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var target = play.Target;
        ArgumentNullException.ThrowIfNull(target);

        var monster = target.Monster;
        ArgumentNullException.ThrowIfNull(monster);

        if (!monster.IntendsToAttack)
            return;

        await PowerCmd.Apply<TauntPower>(choiceContext, target, DynamicVars["TauntPower"].IntValue, Owner.Creature, this, false);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["TauntPower"].UpgradeValueBy(-10m);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<TauntPower>(50)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
}
