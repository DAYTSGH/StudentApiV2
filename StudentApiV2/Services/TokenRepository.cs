using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StudentApiV2.Entities;
using StudentApiV2.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StudentApiV2.Services
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ILoginRepository _loginRepository;
        private readonly Token _tokenOption;

        public TokenRepository(ILoginRepository loginRepository,IOptions<Token> tokenOption)
        {
            _loginRepository = loginRepository ?? throw new ArgumentNullException(nameof(loginRepository));
            _tokenOption = tokenOption.Value ?? throw new ArgumentNullException(nameof(tokenOption));
        }
        //身份valid和验证码pass才给予认证
        public bool IsAuthenticated(LoginRequestDto request,out string token)
        {
            token = string.Empty;
            if (!_loginRepository.IsValid(request))
                return false;
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,request.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOption.Secret));
            var credentails = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(_tokenOption.Issuer, _tokenOption.Audience, expires: DateTime.Now.AddMinutes(_tokenOption.AccessExpiration), signingCredentials: credentails);

            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return _loginRepository.IsValid(request);
        }

        
    }
}
