using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Powers;
using NineSolsMod.NineSolsModCode.Utils;
using NineSolsMod.NineSolsModCode.Variables;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(YiCardPool))]
public class RandomQiStrike() : NineSolsModCard(1, CardType.Attack,
    CardRarity.Common, TargetType.AnyEnemy)
{
    protected override bool ShouldGlowGoldInternal => Owner.Creature.GetPower<InternalDamagePower>() is not null && Owner.Creature.GetPower<InternalDamagePower>()!.Amount >= DynamicVars["QiCost"].IntValue;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState is null)
        {
            return;
        }

        int hitCount = DynamicVars.Repeat.IntValue;
        var qiCostRequired = DynamicVars["QiCost"].IntValue;
        var qiCostPaid = await NineSolsModCmd.CostQi(qiCostRequired, this, choiceContext, true);
        if (qiCostPaid >= qiCostRequired)
        {
            hitCount += 1;
        }

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(hitCount).FromCard(this)
                .TargetingRandomOpponents(CombatState, true)
                .WithHitFx("vfx/vfx_attack_slash", null, null)
                .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1m);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(3m, ValueProp.Move),
        new RepeatVar(3),
        NineSolsModVarsFactory.QiCostVar(1)
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<QiPower>()
    ];
}
