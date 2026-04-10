using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using NineSolsMod.NineSolsModCode.Cards;
using NineSolsMod.NineSolsModCode.Character;

namespace NineSolsMod.NineSolsModCode.Cards;

[Pool(typeof(YiCardPool))]
public class DefendYi() : NineSolsModCard(1, CardType.Skill,
    CardRarity.Basic, TargetType.Self)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play, false);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m);
    }

    public override bool GainsBlock => true;

    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];

    protected override IEnumerable<DynamicVar> CanonicalVars
    {
        get
        {
            yield return new BlockVar(5m, ValueProp.Move);
        }
    }
}