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
        return MaterialUtils.CreateRgbShaderMaterial(0.44f, 0.09f, 0.09f);
    }
}