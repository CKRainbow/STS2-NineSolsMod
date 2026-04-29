using Godot;
using STS2RitsuLib.Scaffolding.Content;

namespace NineSolsMod.NineSolsModCode.Relics;

public abstract class NineSolsModRelic : ModRelicTemplate
{
    public override RelicAssetProfile AssetProfile => new(
        IconPath: _PackedIconPath,
        IconOutlinePath: _PackedIconOutlinePath,
        BigIconPath: _BigIconPath
    );

    private string _PackedIconPath
    {
        get
        {
            var path = $"res://{MainFile.ModId}/images/relics/{GetType().Name}.png";
            return ResourceLoader.Exists(path) ? path : $"res://{MainFile.ModId}/images/relics/relic.png";
        }
    }

    private string _PackedIconOutlinePath
    {
        get
        {
            var path = $"res://{MainFile.ModId}/images/relics/{GetType().Name}_outline.png";
            return ResourceLoader.Exists(path) ? path : $"res://{MainFile.ModId}/images/relics/relic_outline.png";
        }
    }

    private string _BigIconPath
    {
        get
        {
            var path = $"res://{MainFile.ModId}/images/relics/big/{GetType().Name}.png";
            return ResourceLoader.Exists(path) ? path : $"res://{MainFile.ModId}/images/relics/relic.png";
        }
    }
}