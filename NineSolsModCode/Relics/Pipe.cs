using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Powers;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Relics;

[RegisterRelic(typeof(YiRelicPool))]
// 暂时将初始遗物设置为这个
[RegisterCharacterStarterRelic(typeof(Yi))]
public class Pipe() : NineSolsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Common;

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<InternalDamagePower>()
    ];

    public override async Task AfterSideTurnStart(CombatSide side, ICombatState combatState)
    {
        if (side == Owner.Creature.Side)
        {
            var internalDamage = Owner.Creature.GetPower<InternalDamagePower>();
            if (internalDamage is not null)
            {
                await PowerCmd.ModifyAmount(new ThrowingPlayerChoiceContext(), internalDamage, -3, null, null, false);
            }
        }
    }
}