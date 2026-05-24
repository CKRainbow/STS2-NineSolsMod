using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Utils;
using NineSolsMod.NineSolsModCode.Variables;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(YiCardPool))]
public class ShadowStrike() : NineSolsModCard(1, CardType.Attack,
    CardRarity.Common, TargetType.AnyEnemy)
{

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        // 保证一定有目标
        ArgumentNullException.ThrowIfNull(play.Target, "cardPlay.Target");
        // 通过链式调用，生成伤害命令
        await NineSolsModCmd.Finish(DynamicVars.Damage.BaseValue, this, play.Target, choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
        DynamicVars[NineSolsModVarsFactory.FinishKey].UpgradeValueBy(20m);
    }

    // Tags 和 Keywords 的区别是？
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];

    // 用处是？
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(6m, ValueProp.Move),
        ..NineSolsModVarsFactory.FinishVar(120m)
    ];
}