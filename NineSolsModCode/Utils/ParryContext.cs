using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace NineSolsMod.NineSolsModCode.Utils;

// public sealed record BeforeParryContext
// {
//     public required PlayerChoiceContext ChoiceContext { get; init; }
//     public required Creature ParryingCreature { get; init; }
//     public Creature? Applier { get; init; }
//     public CardModel? SourceCard { get; init; }
//     public required Creature AttackingCreature { get; init; }
//     public required decimal IncomingDamage { get; init; }
//     public required decimal ExistingBlock { get; init; }
// }

public sealed record AfterParryContext
{
    public required PlayerChoiceContext ChoiceContext { get; init; }
    public required Creature ParryingCreature { get; init; }
    public Creature? Applier { get; init; }
    public CardModel? SourceCard { get; init; }
    public required Creature AttackingCreature { get; init; }
    public required bool IsPerfectParry { get; init; } = false;
    public required DamageResult DamageResult { get; init; }
}

