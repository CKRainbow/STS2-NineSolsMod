using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using NineSolsMod.NineSolsModCode.Character;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Cards;

[RegisterCard(typeof(YiCardPool))]
public class SwiftRise() : NineSolsModCard(1, CardType.Skill,
    CardRarity.Common, TargetType.Self)
{
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play, false);
        if (DamageReceivedLastTurn)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play, false);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2m);
    }

    public override bool GainsBlock => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(7m, ValueProp.Move),
    ];

    protected override bool ShouldGlowGoldInternal => DamageReceivedLastTurn;

    private bool DamageReceivedLastTurn => CombatManager.Instance.History.Entries.OfType<DamageReceivedEntry>().Any(
        (DamageReceivedEntry entry) => entry.Receiver == Owner.Creature && entry.RoundNumber == CombatState!.RoundNumber - 1
    );
}