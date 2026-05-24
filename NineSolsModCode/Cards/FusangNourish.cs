using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Powers;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(YiCardPool))]
public class FusangNourish() : NineSolsModCard(2, CardType.Skill,
    CardRarity.Rare, TargetType.Self)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState is null)
        {
            return;
        }

        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
        await PowerCmd.Remove<InternalDamagePower>(Owner.Creature);
        await PowerCmd.Apply<QiPower>(choiceContext, Owner.Creature, DynamicVars["QiGain"].BaseValue, Owner.Creature, this, false);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Heal.UpgradeValueBy(3m);
        DynamicVars["QiGain"].UpgradeValueBy(1);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new HealVar(5m),
        ModCardVars.Int("QiGain", 1)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<InternalDamagePower>(),
        HoverTipFactory.FromPower<QiPower>()
    ];
}