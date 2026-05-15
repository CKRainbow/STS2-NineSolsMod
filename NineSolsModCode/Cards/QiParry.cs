using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Powers;
using NineSolsMod.NineSolsModCode.Utils;
using NineSolsMod.NineSolsModCode.Variables;
using STS2RitsuLib.Interop.AutoRegistration;
using ParryPower = NineSolsMod.NineSolsModCode.Powers.ParryPower;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(YiCardPool))]
public class QiParry() : NineSolsModCard(1, CardType.Skill,
    CardRarity.Common, TargetType.Self)
{
    protected override bool ShouldGlowGoldInternal => Owner.Creature.GetPower<InternalDamagePower>() is not null && Owner.Creature.GetPower<InternalDamagePower>()!.Amount >= DynamicVars["QiCost"].IntValue;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<ParryPower>(choiceContext, Owner.Creature, DynamicVars["ParryPower"].BaseValue, Owner.Creature, this, false);
        var qiCostRequired = DynamicVars["QiCost"].IntValue;
        var qiCostPaid = await NineSolsModCmd.CostQi(qiCostRequired, this, choiceContext, true);
        if (qiCostPaid >= qiCostRequired)
        {
            await PowerCmd.Apply<DrawCardsNextTurnPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["ParryPower"].UpgradeValueBy(3m);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<ParryPower>(6m),
        NineSolsModVarsFactory.QiCostVar(1)
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<ParryPower>(),
        HoverTipFactory.FromPower<InternalDamagePower>(),
        HoverTipFactory.FromPower<QiPower>()
    ];
}
