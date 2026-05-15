using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.ValueProps;
using NineSolsMod.NineSolsModCode.Character;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(YiCardPool))]
public class WenWuBlock() : NineSolsModCard(1, CardType.Skill,
    CardRarity.Common, TargetType.Self)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play, false);

        if (CombatState is null)
            return;

        var card = CombatState.CreateCard<WenWu>(Owner);
        if (IsUpgraded)
        {
            CardCmd.Upgrade(card, CardPreviewStyle.HorizontalLayout);
        }
        var addResult = await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Draw, Owner, CardPilePosition.Random);
        if (LocalContext.IsMe(Owner))
        {
            CardCmd.PreviewCardPileAdd(addResult, 0.6f, CardPreviewStyle.HorizontalLayout);
            await Cmd.Wait(1f, false);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(1m);
    }

    public override bool GainsBlock => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(6m, ValueProp.Move),
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromCard<WenWu>()
    ];
}
