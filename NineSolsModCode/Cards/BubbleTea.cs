using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Powers;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(YiCardPool))]
public class BubbleTea() : NineSolsModCard(1, CardType.Skill,
    CardRarity.Uncommon, TargetType.Self)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState is null)
        {
            return;
        }

        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
        var internalDamagePower = Owner.Creature.GetPower<InternalDamagePower>();
        if (internalDamagePower is null)
        {
            return;
        }
        await PowerCmd.ModifyAmount(choiceContext, internalDamagePower, -DynamicVars["InternalHeal"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Heal.UpgradeValueBy(1m);
        DynamicVars["InternalHeal"].UpgradeValueBy(2m);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new HealVar(3m),
        ModCardVars.Int("InternalHeal", 5m)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust,
        CardKeyword.Retain
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<InternalDamagePower>(),
    ];
}