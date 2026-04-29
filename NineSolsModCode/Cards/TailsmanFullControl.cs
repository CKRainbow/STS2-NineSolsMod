using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Powers;
using NineSolsMod.NineSolsModCode.Utils;
using NineSolsMod.NineSolsModCode.Variables;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(YiCardPool))]
public class TailsmanFullControl() : NineSolsModCard(0, CardType.Skill,
    CardRarity.Rare, TargetType.AnyEnemy)
{
    // 只是颜色，并不影响能否被打出
    protected override bool ShouldGlowRedInternal => !Owner.Creature.HasPower<QiPower>();
    protected override bool IsPlayable => Owner.Creature.HasPower<QiPower>();

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
        await NineSolsModCmd.CostQi(5, this, choiceContext, false);
    }

    protected override void OnUpgrade()
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CalculationBaseVar(100m),
        new CalculationExtraVar(100m),
        new CalculatedFinishVar().WithMultiplier((CardModel card, Creature? _)=>
        {
            var qiPower = card.Owner.Creature.GetPower<QiPower>();
            if (qiPower is null)
            {
                return 0;
            }
            var multiplier = 0m;
            for(int i = 0; i < qiPower?.Amount; i++)
            {
                multiplier += 0.2m;
                multiplier *= 1.2m;
            }
            return multiplier;
        })
    ];
}