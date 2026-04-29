using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Powers;
using NineSolsMod.NineSolsModCode.Variables;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(YiCardPool))]
public class TaiChiKick() : NineSolsModCard(1, CardType.Attack,
    CardRarity.Common, TargetType.AnyEnemy)
{

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        // 保证一定有目标
        ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
        // 通过链式调用，生成伤害命令
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_slash", null, null)
            .Execute(choiceContext);
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play, false);
        await PowerCmd.Apply<InternalDamagePower>(choiceContext, play.Target, DynamicVars[InternalDamageVar.Key].BaseValue, Owner.Creature, this, false);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
        DynamicVars.Block.UpgradeValueBy(2m);
        DynamicVars[InternalDamageVar.Key].UpgradeValueBy(2m);
    }

    public override bool GainsBlock => true;

    // 用处是？
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(4m, ValueProp.Move),
        new BlockVar(4m, ValueProp.Move),
        new InternalDamageVar(4m)
    ];
}