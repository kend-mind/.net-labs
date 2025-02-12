using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace VPBANK.RMD.Utils.Security
{
    public static class JwtSecurityKey
    {
        public static SymmetricSecurityKey Create(string secret)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(string.IsNullOrEmpty(secret) ? "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH" : secret));
        }
    }
}
