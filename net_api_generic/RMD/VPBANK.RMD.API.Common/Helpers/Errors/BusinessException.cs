using Newtonsoft.Json;
using VPBANK.RMD.API.Settings.Sections;
using VPBANK.RMD.Utils.Common.Datas;

namespace VPBANK.RMD.API.Common.Helpers.Errors
{
    public class BusinessException : BusinessExceptionResponse
    {
        public override string ToString() => JsonConvert.SerializeObject(this, JsonSetting.ConfigFormat);
    }

    public class FieldValidate : FieldValidateResponse
    {
        public override string ToString() => JsonConvert.SerializeObject(this, JsonSetting.ConfigFormat);
    }
}