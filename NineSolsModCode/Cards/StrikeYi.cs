using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using NineSolsMod.NineSolsModCode.Cards;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Extensions;

namespace NineSolsMod.NineSolsModCode.Cards;

[Pool(typeof(YiCardPool))]
public class StrikeYi() : NineSolsModCard(1, CardType.Attack,
    CardRarity.Basic, TargetType.AnyEnemy)
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
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }

    // Tags 和 Keywords 的区别是？
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];

    // 用处是？
    protected override IEnumerable<DynamicVar> CanonicalVars
    {
        get
        {
            // ValueProp.Move 是做什么的？
            yield return new DamageVar(6m, ValueProp.Move);
        }
    }
}