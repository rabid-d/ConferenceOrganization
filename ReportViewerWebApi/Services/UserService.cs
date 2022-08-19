using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using ReportViewerWebApi.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ReportViewerWebApi.Services;

public class UserService
{
    private readonly DAL.Services.UserService userService;
    private readonly IMapper mapper;
    private readonly ConfigurationManager configuration;

    public UserService(DAL.Services.UserService userService, IMapper mapper, ConfigurationManager configuration)
    {
        this.userService = userService;
        this.mapper = mapper;
        this.configuration = configuration;
    }

    public async Task<bool> RegisterUser(UserViewModel user)
    {
        if (user == null || await IsUsernameTaken(user))
        {
            return false;
        }

        DAL.Model.AppUser appUser = mapper.Map<DAL.Model.AppUser>(user);
        bool success = await userService.AddAppUser(appUser);
        return success;
    }

    public async Task<string?> Login(UserViewModel user)
    {
        if (await IsLoginCredentialsValid(user))
        {
            return CreateToken(user);
        }
        return null;
    }

    public async Task<Guid?> GetUserGuid(string email)
    {
        return await userService.GetAppUserGuid(email);
    }

    private string CreateToken(UserViewModel user)
    {
        byte[] key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("JWT:Key"));
        JwtSecurityToken jwtToken = new(
            issuer: configuration.GetValue<string>("JWT:Issuer"),
            audience: configuration.GetValue<string>("JWT:Audience"),
            claims: new List<Claim>() { new Claim(ClaimTypes.Email, user.Email) },
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        ); ;

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }

    private async Task<bool> IsLoginCredentialsValid(UserViewModel user)
    {
        DAL.Model.AppUser appUser = mapper.Map<DAL.Model.AppUser>(user);
        return await userService.IsLoginCredentialsValid(appUser);
    }

    private async Task<bool> IsUsernameTaken(UserViewModel user)
    {
        return userService.IsUsernameTaken(user.Email);
    }
}
