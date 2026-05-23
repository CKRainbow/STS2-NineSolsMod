using STS2RitsuLib.Content;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Tags;

[RegisterOwnedCardTag(nameof(AzureSandCraft))]
[RegisterOwnedCardTag(nameof(TailsmanTransmutation))]
public class NineSolsModTags
{
    public static readonly string AzureSandCraft = ModContentRegistry.GetQualifiedCardTagId(MainFile.ModId, nameof(AzureSandCraft));
    public static readonly string TailsmanTransmutation = ModContentRegistry.GetQualifiedCardTagId(MainFile.ModId, nameof(TailsmanTransmutation));
}