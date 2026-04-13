using BaseLib.Abstracts;
using NineSolsMod.NineSolsModCode.Extensions;
using Godot;

namespace NineSolsMod.NineSolsModCode.Character;

public class YiCardPool : CustomCardPoolModel
{
    public override string Title => Yi.CharacterId; //This is not a display name.

    public override string BigEnergyIconPath => "big_energy_yi.png".CharacterUiPath();
    public override string TextEnergyIconPath => "text_energy_yi.png".CharacterUiPath();


    /* These HSV values will determine the color of your card back.
    They are applied as a shader onto an already colored image,
    so it may take some experimentation to find a color you like.
    Generally they should be values between 0 and 1. */
    public override Color EnergyOutlineColor => Yi.Color;
    public override Color ShaderColor => Yi.Color;
    // public override float H => 0.47f; //Hue; changes the color.
    // public override float S => 0.52f; //Saturation
    // public override float V => 0.60f; //Brightness

    //Alternatively, leave these values at 1 and provide a custom frame image.
    /*public override Texture2D CustomFrame(CustomCardModel card)
    {
        //This will attempt to load NineSolsMod/images/cards/frame.png
        return PreloadManager.Cache.GetTexture2D("cards/frame.png".ImagePath());
    }*/

    //Color of small card icons
    public override Color DeckEntryCardColor => new("4a9b8c");

    public override bool IsColorless => false;
}