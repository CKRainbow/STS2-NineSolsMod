using STS2RitsuLib.Content;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Tags;

[RegisterOwnedCardTag(nameof(AzureSandCraft))]
// [RegisterOwnedCardTag(nameof(Heavy2))] // 添加更多就新加这个特性
public class NineSolsModTags
{
    public static readonly string AzureSandCraft = ModContentRegistry.GetQualifiedCardTagId(MainFile.ModId, nameof(AzureSandCraft));
}