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
public class Meditation() : NineSolsModCard(0, CardType.Skill,
    CardRarity.Common, TargetType.Self)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var internalDamage = Owner.Creature.GetPower<InternalDamagePower>();
        if (internalDamage is not null)
        {
            var removeAmount = Math.Min(internalDamage.Amount, DynamicVars["RemoveAmount"].BaseValue);
            await PowerCmd.ModifyAmount(choiceContext, internalDamage, -removeAmount, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["RemoveAmount"].UpgradeValueBy(2m);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("RemoveAmount", 4m),
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<InternalDamagePower>()
    ];
}
