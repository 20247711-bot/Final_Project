using Microsoft.AspNetCore.Authorization;
namespace Final_Project.Security;

public sealed class AuthorizeAdminAttribute : AuthorizeAttribute { public AuthorizeAdminAttribute() { Roles = "Administrator"; } }
