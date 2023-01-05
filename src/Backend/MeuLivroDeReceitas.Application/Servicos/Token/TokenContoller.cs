using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MeuLivroDeReceitas.Application.Servicos.Token;

public class TokenContoller
{

    private const string EmailAlias = "eml";
    private readonly double _tempoDeVidaTokenEmMinutos;
    private readonly string _chaveDeSeguranca;

    public TokenContoller(double tempoDeVidaTokenEmMinutos, string chaveDeSeguranca)
    {
        _tempoDeVidaTokenEmMinutos = tempoDeVidaTokenEmMinutos;
        _chaveDeSeguranca= chaveDeSeguranca;
    }

    public string GerarToken(string email)
    {
        var claims = new List<Claim>
        {
            new Claim(EmailAlias, email)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires= DateTime.UtcNow.AddMinutes(_tempoDeVidaTokenEmMinutos),
            SigningCredentials = new SigningCredentials(SimetricKey(), SecurityAlgorithms.HmacSha256Signature)
        };

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }

    public ClaimsPrincipal ValidarToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var parametrosValidacao = new TokenValidationParameters
        {
            RequireExpirationTime= true,
            IssuerSigningKey = SimetricKey(),
            ClockSkew = new TimeSpan(0),
            ValidateIssuer = false,
            ValidateAudience = false,
        };

        var claims = tokenHandler.ValidateToken(token, parametrosValidacao, out _);
        return claims;
    }

    public string RecuperarEmail(string token)
    {
        var claims = ValidarToken(token);
        var email = claims.FindFirst(EmailAlias).Value;
        return email;
    }

    private SymmetricSecurityKey SimetricKey()
    {
        var symmetricKey = Convert.FromBase64String( _chaveDeSeguranca );
        return new SymmetricSecurityKey( symmetricKey );
    }
}
