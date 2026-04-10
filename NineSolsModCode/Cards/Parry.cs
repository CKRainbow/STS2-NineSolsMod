using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Powers;

namespace NineSolsMod.NineSolsModCode.Cards;

[Pool(typeof(YiCardPool))]
public class Parry() : NineSolsModCard(1, CardType.Skill,
    CardRarity.Basic, TargetType.Self)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play, false);
        await PowerCmd.Apply<ParryPower>(Owner.Creature, DynamicVars["ParryPower"].BaseValue,
            Owner.Creature, this, false);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m);
        // 结果还是要这样调用吗
        DynamicVars["ParryPower"].UpgradeValueBy(1m);
    }

    public override bool GainsBlock => true;

    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(8m, ValueProp.Move),
        new PowerVar<ParryPower>(3m)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<ParryPower>(),
        HoverTipFactory.FromPower<InternalDamagePower>()
    ];
}