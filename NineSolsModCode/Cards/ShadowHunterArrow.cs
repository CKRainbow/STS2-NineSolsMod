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
using NineSolsMod.NineSolsModCode.Keywords;
using NineSolsMod.NineSolsModCode.Powers;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(TokenCardPool))]
public class ShadowHunterArrow() : NineSolsModCard(1, CardType.Attack,
    CardRarity.Token, TargetType.AnyEnemy)
{
    private bool _exhaustedPlay = false;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target, "play.Target");
        // NCombatRoom? instance = NCombatRoom.Instance;
        // NCreature? ncreature = instance?.GetCreatureNode(play.Target);
        // if (ncreature is not null)
        // {
        //     NLargeMagicMissileVfx? nlargeMagicMissileVfx = NLargeMagicMissileVfx.Create(ncreature.GetBottomOfHitbox(), new Color("50b598"));
        //     NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(nlargeMagicMissileVfx);
        //     await Cmd.Wait(nlargeMagicMissileVfx.WaitTime, false);
        // }
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
            .Execute(choiceContext);
        if (!_exhaustedPlay)
        {
            await PowerCmd.Apply<ShadowHunterArrowPower>(choiceContext, play.Target, 1, Owner.Creature, this, true);
        }
        _exhaustedPlay = false;
    }

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        CardPile? pile = Pile;
        if (pile is not null && pile.Type == PileType.Exhaust)
        {
            if (player == Owner)
            {
                Creature? target = null;
                foreach (var enemy in combatState.Enemies)
                {
                    var power = enemy.GetPower<ShadowHunterArrowPower>();
                    if (power is null)
                    {
                        continue;
                    }
                    if (power.MatchCard(this))
                    {
                        target = enemy;
                    }
                }
                if (target is not null)
                {
                    _exhaustedPlay = true;
                    await CardCmd.AutoPlay(choiceContext, this, target, AutoPlayType.Default, false, false);
                }
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
    }


    protected override IEnumerable<string> RegisteredKeywordIds => [
        NineSolsModKeywords.AzureSandCraft
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust,
        CardKeyword.Retain
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(10m, ValueProp.Move),
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
    ];
}