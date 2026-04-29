using Godot;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace NineSolsMod.NineSolsModCode.Powers;

public abstract class NineSolsModPower : ModPowerTemplate
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
}