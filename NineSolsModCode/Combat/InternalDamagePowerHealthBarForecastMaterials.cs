using Godot;
using STS2RitsuLib.Utils;

namespace NineSolsMod.NineSolsModCode.Combat;

internal static class InternalDamagePowerHealthBarForecastMaterials
{
    private static ShaderMaterial? _forecastMaterial;

    public static Material ForecastMaterial =>
        _forecastMaterial ??= Create();

    private static ShaderMaterial Create()
    {
        return MaterialUtils.CreateHsvShaderMaterial(0.47f, 0.52f, 0.60f);
    }
}