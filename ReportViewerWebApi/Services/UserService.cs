using AutoMapper;
using ReportViewerWebApi.ViewModels;

namespace ReportViewerWebApi.Services;

public class UserService
{
    private readonly DAL.Services.UserService userService;
    private readonly IMapper mapper;

    public UserService(DAL.Services.UserService userService, IMapper mapper)
    {
        this.userService = userService;
        this.mapper = mapper;
    }

    public async Task RegisterUser(UserViewModel user)
    {
        DAL.Model.AppUser appUser = mapper.Map<DAL.Model.AppUser>(user);
        await userService.AddAppUser(appUser);
    }
}
