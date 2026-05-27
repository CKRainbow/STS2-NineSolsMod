using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using NineSolsMod.NineSolsModCode.Cards;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
public class HexachremVaultPower : NineSolsModPower
{
    private class Data
    {
        public bool isUpgraded = false;
    }
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override Color AmountLabelColor => _normalAmountLabelColor;

    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromCard<QianmoNetwork>(GetInternalData<Data>().isUpgraded)
    ];

    protected override object InitInternalData()
    {
        return new Data();
    }

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (player != Owner.Player)
        {
            return;
        }
        if (CombatManager.Instance.IsOverOrEnding)
        {
            return;
        }
        var cardToAdd = combatState.CreateCard<QianmoNetwork>(player);
        if (GetInternalData<Data>().isUpgraded)
        {
            CardCmd.Upgrade(cardToAdd);
        }
        var addResult = await CardPileCmd.AddGeneratedCardToCombat(cardToAdd, PileType.Discard, player, CardPilePosition.Random);
        if (LocalContext.IsMe(player))
        {
            CardCmd.PreviewCardPileAdd(addResult, 0.6f, CardPreviewStyle.HorizontalLayout);
            await Cmd.Wait(1f, false);
        }
    }

    public void Upgrade()
    {
        GetInternalData<Data>().isUpgraded = true;
    }
}