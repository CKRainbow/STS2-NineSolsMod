using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Powers;
using NineSolsMod.NineSolsModCode.Utils;
using NineSolsMod.NineSolsModCode.Variables;

namespace NineSolsMod.NineSolsModCode.Cards;

[Pool(typeof(YiCardPool))]
public class TailsmanFlowWater() : NineSolsModCard(1, CardType.Skill,
    CardRarity.Basic, TargetType.AnyEnemy)
{
    // 只是颜色，并不影响能否被打出
    protected override bool ShouldGlowRedInternal => !Owner.Creature.HasPower<QiPower>();
    protected override bool IsPlayable => !Owner.Creature.HasPower<QiPower>();

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        var internalDamageAmount = play.Target.GetPower<InternalDamagePower>();
        if (internalDamageAmount is null)
        {
            return;
        }
        await NineSolsModCmd.Finish(0, this, play.Target, choiceContext, calculated: true);
        await NineSolsModCmd.CostQi(1, this, choiceContext, true);
    }

    protected override PileType GetResultPileType()
    {
        PileType resultPileType = base.GetResultPileType();
        if (resultPileType != PileType.Discard)
        {
            return resultPileType;
        }

        return PileType.Hand;
    }

    protected override void OnUpgrade()
    {
        DynamicVars[FinishVar.Key].UpgradeValueBy(50m);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new FinishVar(150m),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Retain
    ];
}