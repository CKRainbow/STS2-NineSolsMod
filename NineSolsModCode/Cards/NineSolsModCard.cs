using MegaCrit.Sts2.Core.Entities.Cards;
using Godot;
using STS2RitsuLib.Scaffolding.Content;

namespace NineSolsMod.NineSolsModCode.Cards;

public abstract class NineSolsModCard(int cost, CardType type, CardRarity rarity, TargetType target, bool showInLibrary = true) :
    ModCardTemplate(cost, type, rarity, target, showInLibrary)
{
    public override CardAssetProfile AssetProfile => new(
        PortraitPath: PortraitPath,
        BetaPortraitPath: BetaPortraitPath
    );

    //Smaller variants of card images for efficiency:
    //Smaller variant of fullart: 250x350
    //Smaller variant of normalart: 250x190

    //Uses card_portraits/card_name.png as image path. These should be smaller images.
    public override string PortraitPath
    {
        get
        {
            var path = $"res://{MainFile.ModId}/images/cards/{GetType().Name}.png";
            return ResourceLoader.Exists(path) ? path : $"res://{MainFile.ModId}/images/cards/card.png";
        }
    }
    public override string BetaPortraitPath
    {
        get
        {
            var path = $"res://{MainFile.ModId}/images/cards/beta/{GetType().Name}.png";
            return ResourceLoader.Exists(path) ? path : $"res://{MainFile.ModId}/images/cards/beta/card.png";
        }
    }
}