using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Powers;
using NineSolsMod.NineSolsModCode.Relics;

namespace NineSolsMod.NineSolsModCode.Relics;

[Pool(typeof(YiRelicPool))]
public class Pipe() : NineSolsModRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Common;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<InternalDamagePower>()
    ];

    public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side == base.Owner.Creature.Side)
        {
            var internalDamage = base.Owner.Creature.GetPower<InternalDamagePower>();
            if (internalDamage is not null)
            {
                await PowerCmd.ModifyAmount(internalDamage, -3, null, null, false);
            }
        }
    }
}