using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Powers;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(YiCardPool))]
public class FusangNourish() : NineSolsModCard(2, CardType.Skill,
    CardRarity.Rare, TargetType.Self)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState is null)
        {
            return;
        }

        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
        await PowerCmd.Remove<InternalDamagePower>(Owner.Creature);
        var card = CombatState.CreateCard<AzureSand>(Owner);
        if (IsUpgraded)
        {
            CardCmd.Upgrade(card, CardPreviewStyle.HorizontalLayout);
        }
        // TODO: 感觉好像给苍砂不太合理
        var addResult = await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Draw, Owner, CardPilePosition.Random);
        if (LocalContext.IsMe(Owner))
        {
            CardCmd.PreviewCardPileAdd(addResult, 0.6f, CardPreviewStyle.HorizontalLayout);
            await Cmd.Wait(1f, false);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Heal.UpgradeValueBy(2m);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new HealVar(5m),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<InternalDamagePower>()
    ];
}