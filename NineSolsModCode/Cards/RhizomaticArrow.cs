using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Powers;
using NineSolsMod.NineSolsModCode.Utils;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(YiCardPool))]
public class RhizomaticArrow() : NineSolsModQiCard(2, CardType.Skill,
    CardRarity.Rare, TargetType.AllEnemies, 3, qiOnly: true)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(CombatState);

        await NineSolsModCmd.CostQi(qiCost, this, choiceContext, true);

        var currentHp = Owner.Creature.CurrentHp;

        await PowerCmd.Apply<InternalDamagePower>(choiceContext, Owner.Creature, currentHp / 2, Owner.Creature, this, false);
        await PowerCmd.Apply<InternalDamagePower>(choiceContext, CombatState.HittableEnemies, ((ComputedDynamicVar)DynamicVars["TotalInternalDamage"]).Calculate(), Owner.Creature, this, false);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Multiplier"].UpgradeValueBy(2);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        ..base.CanonicalVars,
        ModCardVars.Int("Multiplier", 6),
        ModCardVars.Computed("TotalInternalDamage", 0, (_) => {
           return Owner.Creature.CurrentHp / 2 * DynamicVars["Multiplier"].IntValue;
        }),
    ];


    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<InternalDamagePower>(),
    ];
}