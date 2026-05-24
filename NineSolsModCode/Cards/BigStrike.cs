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
public class BigStrike() : NineSolsModCard(2, CardType.Attack,
    CardRarity.Common, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_slash", null, null)
            .Execute(choiceContext);
        await NineSolsModCmd.Deviation(this, Owner.Creature, choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(8m);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(24m, ValueProp.Move),
        NineSolsModVarsFactory.DeviationVar(3m)
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<InternalDamagePower>()
    ];
}
