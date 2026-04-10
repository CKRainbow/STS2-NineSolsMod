using BaseLib.Abstracts;
using BaseLib.Extensions;
using NineSolsMod.NineSolsModCode.Extensions;
using Godot;

namespace NineSolsMod.NineSolsModCode.Powers;

public abstract class NineSolsModPower : CustomPowerModel
{
    //Loads from NineSolsMod/images/powers/your_power.png
    public override string CustomPackedIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
            return ResourceLoader.Exists(path) ? path : "power.png".PowerImagePath();
        }
    }

    public override string CustomBigIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
            return ResourceLoader.Exists(path) ? path : "power.png".BigPowerImagePath();
        }
    }
}