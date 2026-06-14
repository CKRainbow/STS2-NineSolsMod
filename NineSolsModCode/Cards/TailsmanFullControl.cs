using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Powers;
using NineSolsMod.NineSolsModCode.Utils;
using NineSolsMod.NineSolsModCode.Variables;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Combat.SecondaryResources;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(YiCardPool))]
public class TailsmanFullControl() : NineSolsModQiCard(0, CardType.Skill,
    CardRarity.Rare, TargetType.AnyEnemy, 0, qiRequired: true)
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

    protected override void OnUpgrade()
    {
        DynamicVars["AdditionBonus"].UpgradeValueBy(10m);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        ModCardVars.Int("AdditionBonus", 20m),
        ..NineSolsModVarsFactory.FinishVar(
            100m,
            (card) =>
            {
                if (card is null) return 100m;
                if (!card.IsInCombat) return 100m;
                var qiAmount = SecondaryResourceCmd.Get(card.Owner, QiResource.QiId);
                return qiAmount * DynamicVars["AdditionBonus"].BaseValue + 100m;
            }),
    ];
}