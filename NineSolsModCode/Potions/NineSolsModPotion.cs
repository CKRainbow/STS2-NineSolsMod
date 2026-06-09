using Godot;
using STS2RitsuLib.Scaffolding.Content;

namespace NineSolsMod.NineSolsModCode.Potions;

public abstract class NineSolsModPotion : ModPotionTemplate
{
    public override PotionAssetProfile AssetProfile => new(
        ImagePath: _ImagePath,
        OutlinePath: _OutlinePath
    );

    private string _ImagePath
    {
        get
        {
            var path = $"res://{MainFile.ModId}/images/potions/{GetType().Name}.png";
            return ResourceLoader.Exists(path) ? path : $"res://{MainFile.ModId}/images/potions/potion.png";
        }
    }

    private string _OutlinePath
    {
        get
        {
            var path = $"res://{MainFile.ModId}/images/potions/{GetType().Name}_big.png";
            return ResourceLoader.Exists(path) ? path : $"res://{MainFile.ModId}/images/potions/potion_outline.png";
        }
    }
}