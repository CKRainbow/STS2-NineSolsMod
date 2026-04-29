using Godot;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Utils;

namespace NineSolsMod.NineSolsModCode.Character;

public class YiCardPool : TypeListCardPoolModel
{
    public override string Title => Yi.CharacterId; //This is not a display name.
    public override string EnergyColorName => "Jade";

    // 74 x 74
    public override string BigEnergyIconPath => $"res://{MainFile.ModId}/images/charui/big_energy_yi.png";
    // 24 x 24
    public override string TextEnergyIconPath => $"res://{MainFile.ModId}/images/charui/text_energy_yi.png";

    public override Color EnergyOutlineColor => Yi.Color;
    public override Color DeckEntryCardColor => new("4a9b8c");

    public override Material? PoolFrameMaterial => MaterialUtils.CreateHsvShaderMaterial(0.47f, 0.52f, 0.60f);

    public override bool IsColorless => false;


}