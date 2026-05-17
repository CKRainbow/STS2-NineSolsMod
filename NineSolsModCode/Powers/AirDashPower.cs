using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using NineSolsMod.NineSolsModCode.Cards;
using STS2RitsuLib.Interop.AutoRegistration;

namespace NineSolsMod.NineSolsModCode.Powers;

[RegisterPower]
public class AirDashPower : NineSolsModTemporaryPower
{
    public override AbstractModel OriginModel => ModelDb.Card<AirDash>();

    protected override bool IsPositive => true;

    public override PowerModel InternallyAppliedPower => ModelDb.Power<DexterityPower>();

}