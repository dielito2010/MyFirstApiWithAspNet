using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace MyFirstApiWithAspNet.Endpoints.Security;

public class TokenPost
{
    public static string Template => "/token";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static IResult Action(
        LoginRequest loginRequest,
        IConfiguration configuration,
        UserManager<IdentityUser> userManager,
        IWebHostEnvironment environment)
    {
        var user = userManager.FindByEmailAsync(loginRequest.Email).Result;
        if (user == null)
        {
            return Results.BadRequest(new Dictionary<string, string[]>
        {
            { "Email", new[] { "Usuário não encontrado." } }
        });
        }

        if (!userManager.CheckPasswordAsync(user, loginRequest.Password).Result)
        {
            return Results.BadRequest(new Dictionary<string, string[]>
        {
            { "Password", new[] { "Senha incorreta." } }
        });
        }

        var claims = userManager.GetClaimsAsync(user).Result;
        var subject = new ClaimsIdentity(new Claim[]
        {
        new Claim(ClaimTypes.Email, loginRequest.Email),
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        });
        subject.AddClaims(claims);

        var key = Encoding.ASCII.GetBytes(configuration["JwtBearerTokenSettings:SecretKey"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = subject,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = configuration["JwtBearerTokenSettings:Audience"],
            Issuer = configuration["JwtBearerTokenSettings:Issuer"],
            Expires = environment.IsDevelopment() || environment.IsStaging() ?
            DateTime.UtcNow.AddYears(1) : DateTime.UtcNow.AddMinutes(60)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Results.Ok(new
        {
            token = tokenHandler.WriteToken(token)
        });
    }
}