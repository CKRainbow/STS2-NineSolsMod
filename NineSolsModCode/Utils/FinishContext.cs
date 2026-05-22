using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace NineSolsMod.NineSolsModCode.Utils;

public sealed record BeforeFinishContext
{
    public required PlayerChoiceContext? ChoiceContext { get; init; }
    public required CardModel SourceCard { get; init; }
    public required Creature Target { get; init; }
    public required decimal FinishMult { get; init; }
    public required decimal InternalDamageAmount { get; init; }
    public required List<Creature> TargetsToDamage { get; set; }
}

public sealed record AfterFinishContext
{
    public required PlayerChoiceContext? ChoiceContext { get; init; }
    public required CardModel SourceCard { get; init; }
    public required Creature Target { get; init; }
    public required decimal FinishMult { get; init; }
    public required decimal InternalDamageAmount { get; init; }
    public required IEnumerable<Creature> DamagedTargets { get; init; }
}
