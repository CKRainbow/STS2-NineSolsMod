using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Powers;
using NineSolsMod.NineSolsModCode.Utils;
using NineSolsMod.NineSolsModCode.Variables;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(YiCardPool))]
public class DeviationStrike() : NineSolsModCard(1, CardType.Skill,
    CardRarity.Common, TargetType.Self)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await NineSolsModCmd.Deviation(this, Owner.Creature, choiceContext);
        await PowerCmd.Apply<InternalDamagePower>(choiceContext, CombatState!.HittableEnemies, DynamicVars[NineSolsModVarsFactory.InternalDamageKey].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars[NineSolsModVarsFactory.InternalDamageKey].UpgradeValueBy(5m);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        NineSolsModVarsFactory.DeviationVar(2m),
        NineSolsModVarsFactory.InternalDamageVar(13m)
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<InternalDamagePower>()
    ];
}
