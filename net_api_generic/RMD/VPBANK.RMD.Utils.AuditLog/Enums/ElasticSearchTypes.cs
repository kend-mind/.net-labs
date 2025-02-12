using System.ComponentModel;

namespace VPBANK.RMD.Utils.AuditLog.Enums
{
    public enum ElasticSearchTypes
    {
        None,
        [Description("USER-SERVICE")]
        UserService,
        [Description("SCHEDULE-SERVICE")]
        ScheduleService,
        [Description("PHOENIX-SERVICE")]
        PhoenixService,
        [Description("IPHOENIX-SERVICE")]
        IPhoenixService
    }
}
