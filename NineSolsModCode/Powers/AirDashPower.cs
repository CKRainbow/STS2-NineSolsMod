using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using NineSolsMod.NineSolsModCode.Cards;

namespace NineSolsMod.NineSolsModCode.Powers;

public class AirDashPower : CustomTemporaryDexterityPower
{
    public override AbstractModel OriginModel => ModelDb.Card<AirDash>();

    public override LocString Description => new LocString("powers", IsPositive ? "TEMPORARY_DEXTERITY_POWER.description" : "TEMPORARY_DEXTERITY_DOWN.description");

    protected override string SmartDescriptionLocKey
    {
        get
        {
            if (!IsPositive)
            {
                return "TEMPORARY_DEXTERITY_DOWN.smartDescription";
            }

            return "TEMPORARY_DEXTERITY_POWER.smartDescription";
        }
    }
}