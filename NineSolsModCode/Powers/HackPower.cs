using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using NineSolsMod.NineSolsModCode.Cards;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
public class HackPower : CustomTemporaryStrenthPower
{
    public override AbstractModel OriginModel => ModelDb.Card<Hack>();

    public override LocString Description => new LocString("powers", IsPositive ? "TEMPORARY_STRENGTH_POWER.description" : "TEMPORARY_STRENGTH_DOWN.description");

    protected override string SmartDescriptionLocKey
    {
        get
        {
            if (!IsPositive)
            {
                return "TEMPORARY_STRENGTH_DOWN.smartDescription";
            }

            return "TEMPORARY_STRENGTH_POWER.smartDescription";
        }
    }
}