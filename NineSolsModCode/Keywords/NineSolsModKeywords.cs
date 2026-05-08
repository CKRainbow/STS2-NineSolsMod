
using STS2RitsuLib.Content;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Keywords;

[RegisterOwnedCardKeyword(nameof(AzureSandCraft))]
class NineSolsModKeywords
{
    public static readonly string AzureSandCraft = ModContentRegistry.GetQualifiedKeywordId(MainFile.ModId, nameof(AzureSandCraft));
}