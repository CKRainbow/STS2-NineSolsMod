using Godot;
using STS2RitsuLib.Scaffolding.Content;

namespace NineSolsMod.NineSolsModCode.Character;

public class YiPotionPool : TypeListPotionPoolModel
{
    public override Color LabOutlineColor => Yi.Color;
    public override string EnergyColorName => "Jade";


    public override string BigEnergyIconPath => $"res://{MainFile.ModId}/images/charui/big_energy_yi.png";
    public override string TextEnergyIconPath => $"res://{MainFile.ModId}/images/charui/text_energy_yi.png";

}