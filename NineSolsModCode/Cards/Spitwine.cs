using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using NineSolsMod.NineSolsModCode.Character;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(YiCardPool))]
public class Spitwine() : NineSolsModCard(1, CardType.Skill,
    CardRarity.Rare, TargetType.Self)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<PoisonPower>(choiceContext, Owner.Creature, DynamicVars["PoisonPower"].IntValue, Owner.Creature, this, false);
        await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars["MaxHealth"].IntValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["MaxHealth"].UpgradeValueBy(1m);
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<PoisonPower>(3m),
        ModCardVars.Int("MaxHealth", 2m)
    ];
}