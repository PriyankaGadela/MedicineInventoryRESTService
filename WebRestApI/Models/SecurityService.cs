/*using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

 

namespace WebRestApp.Models
{
    public class SecurityService
    {
    }
}*/
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace WebRestApp.Models
{
    public class SecurityService
    {
        IConfiguration config;
        DataService service;
        public SecurityService(IConfiguration _config, DataService _service)
        {
            this.config = _config;
            this.service = _service;
        }
        private string GenerateJWT(string role)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Aud,config["Jwt:audience"]!),
                new Claim(JwtRegisteredClaimNames.Iss,config["Jwt:issuer"]!),
                new Claim(ClaimTypes.Role, role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:key"]!));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var securityToken = new JwtSecurityToken(config["Jwt:issuer"]!, config["Jwt:audience"]!, claims, expires: DateTime.Now.AddMinutes(30), signingCredentials: cred);
            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }
        public TokenAndRole? AuthenticateUserAndGetToken(OperatorLoginModel model)
        {
            string? token = null;
            string? role = service.FindUser(model);
            if (role != null)
            {
                token = GenerateJWT(role!);
                return new TokenAndRole() { Token = token, Role = role };
            }
            else
            {
                return null;  //means   "INVALID_UID_PWD";
            }

        }
        public class TokenAndRole
        {
            public string? Token { get; set; }
            public string? Role { get; set; }

        }
        public class Policies
        {
            /*public const string Manager = "Manager";
            public const string Executive = "Executive";*/
            public const string Admin = "Admin";
            /*public static AuthorizationPolicy ManagerPolicy()
            {
                return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Manager).Build();
            }
            public static AuthorizationPolicy ExecutivePolicy()
            {
                return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Executive).Build();
            }*/

            public static AuthorizationPolicy AdminPolicy()
            {
                return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Admin).Build();
            }
        }
        public class OperatorLoginModel
        {
            public string? Username { get; set; }
            public string? Password { get; set; }
        }
    }
}