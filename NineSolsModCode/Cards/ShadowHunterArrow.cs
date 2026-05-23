using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using NineSolsMod.NineSolsModCode.Powers;
using NineSolsMod.NineSolsModCode.Tags;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(TokenCardPool))]
public class ShadowHunterArrow() : NineSolsModCard(1, CardType.Attack,
    CardRarity.Token, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target, "play.Target");
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
            .Execute(choiceContext);
        if (!play.IsAutoPlay)
        {
            await PowerCmd.Apply<ShadowHunterArrowPower>(choiceContext, play.Target, 1, Owner.Creature, this, true);
        }
    }

    public override async Task AfterAutoPrePlayPhaseEnteredEarly(PlayerChoiceContext choiceContext, Player player)
    {
        if (CombatState is null)
            return;

        CardPile? pile = Pile;
        if (pile is not null && pile.Type == PileType.Exhaust)
        {
            if (player == Owner)
            {
                foreach (var enemy in CombatState.Enemies)
                {
                    var powers = enemy.GetPowerInstances<ShadowHunterArrowPower>();
                    foreach (var power in powers)
                    {
                        if (power is null)
                        {
                            continue;
                        }
                        if (power.MatchCard(this))
                        {
                            await CardCmd.AutoPlay(choiceContext, this, enemy, AutoPlayType.Default, false, false);
                        }
                    }
                }
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust,
        CardKeyword.Retain,
    ];

    protected override HashSet<CardTag> CanonicalTags => [
        NineSolsModTags.AzureSandCraft.GetModCardTag()
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(10m, ValueProp.Move),
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
    ];
}