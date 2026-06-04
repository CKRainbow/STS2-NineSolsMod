using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Powers;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(YiCardPool))]
public class ComputingPowerOverclock() : NineSolsModCard(1, CardType.Power,
    CardRarity.Rare, TargetType.Self)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<ComputingPowerOverclockPower>(choiceContext, Owner.Creature, DynamicVars["ComputingPowerOverclockPower"].IntValue, Owner.Creature, this, false);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<ComputingPowerOverclockPower>(1)
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<ComputingPowerOverclockPower>()
    ];

}