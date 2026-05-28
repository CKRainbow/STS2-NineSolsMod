using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Powers;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(YiCardPool))]
public class MultipleHit() : NineSolsModCard(1, CardType.Attack,
    CardRarity.Uncommon, TargetType.AllEnemies)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Card.CombatState is null) return;

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                    .FromCard(this)
                    .WithHitCount((int)((ComputedDynamicVar)DynamicVars["TotalRepeat"]).Calculate())
                    .TargetingAllOpponents(play.Card.CombatState)
                    .WithHitFx("vfx/vfx_attack_blunt", null, "heavy_attack.mp3")
                    .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(4m, ValueProp.Move),
        ModCardVars.Computed("TotalRepeat", 1, (card) => {
            if (card is null) return 1;
            return 1 + CombatManager.Instance.History.Entries.OfType<PowerReceivedEntry>().Count(
                    (entry) => {
                        return entry.Actor == card.Owner.Creature && entry.Power is InternalDamagePower;
                    }
            );
        })
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<InternalDamagePower>(),
    ];
}
