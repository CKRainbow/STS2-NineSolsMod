using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Powers;
using NineSolsMod.NineSolsModCode.Utils;
using NineSolsMod.NineSolsModCode.Variables;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(YiCardPool))]
public class TailsmanFlowWater() : NineSolsModQiCard(1, CardType.Skill,
    CardRarity.Uncommon, TargetType.AnyEnemy, 1, qiRequired: true)
{
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
        await NineSolsModCmd.Finish(0, this, play.Target, choiceContext);
    }

    protected override PileType GetResultPileTypeForCardPlay()
    {
        PileType resultPileTypeForCardPlay = base.GetResultPileTypeForCardPlay();
        if (resultPileTypeForCardPlay != PileType.Discard)
        {
            return resultPileTypeForCardPlay;
        }
        return PileType.Hand;
    }

    protected override void OnUpgrade()
    {
        DynamicVars[NineSolsModVarsFactory.FinishKey].UpgradeValueBy(50m);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        ..NineSolsModVarsFactory.FinishVar(150m)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Retain
    ];
}