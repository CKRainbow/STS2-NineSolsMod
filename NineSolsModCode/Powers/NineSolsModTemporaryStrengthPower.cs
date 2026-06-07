using Godot;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Combat.Powers;
using STS2RitsuLib.Scaffolding.Content;

namespace NineSolsMod.NineSolsModCode.Powers;

public abstract class NineSolsModTemporaryStrengthPower<T> : ModTemporaryAppliedPowerTemplate<T, StrengthPower> where T : AbstractModel
{
    public override PowerAssetProfile AssetProfile => new(
        IconPath: _PackedIconPath,
        BigIconPath: _BigIconPath
    );

    //Loads from NineSolsMod/images/powers/your_power.png
    private string _PackedIconPath
    {
        get
        {
            var path = $"res://{MainFile.ModId}/images/powers/{GetType().Name}.png";
            return ResourceLoader.Exists(path) ? path : $"res://{MainFile.ModId}/images/powers/power.png";
        }
    }

    private string _BigIconPath
    {
        get
        {
            var path = $"res://{MainFile.ModId}/images/powers/{GetType().Name}_big.png";
            return ResourceLoader.Exists(path) ? path : $"res://{MainFile.ModId}/images/powers/power_big.png";
        }
    }

    public override LocString Description => new("powers", IsPositive ? "TEMPORARY_STRENGTH_POWER.description" : "TEMPORARY_STRENGTH_POWER_DOWN.description");
    protected override string SmartDescriptionLocKey => IsPositive ? "TEMPORARY_STRENGTH_POWER.description" : "TEMPORARY_STRENGTH_POWER_DOWN.description";
}