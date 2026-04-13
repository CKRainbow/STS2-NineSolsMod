using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using NineSolsMod.NineSolsModCode.Character;
using NineSolsMod.NineSolsModCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models.Cards;

namespace NineSolsMod.NineSolsModCode.Cards;

[Pool(typeof(YiCardPool))]
public abstract class NineSolsModCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    CustomCardModel(cost, type, rarity, target)
{
    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    public override string CustomPortraitPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();
            if (!Godot.FileAccess.FileExists(path))
            {
                return "card.png".BigCardImagePath();
            }
            return path;
        }
    }

    //Smaller variants of card images for efficiency:
    //Smaller variant of fullart: 250x350
    //Smaller variant of normalart: 250x190

    //Uses card_portraits/card_name.png as image path. These should be smaller images.
    public override string PortraitPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
            if (!Godot.FileAccess.FileExists(path))
            {
                return "card.png".CardImagePath();
            }
            return path;
        }
    }
    public override string BetaPortraitPath
    {
        get
        {
            var path = $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
            if (!Godot.FileAccess.FileExists(path))
            {
                return "card.png".CardImagePath();
            }
            return path;
        }
    }
}