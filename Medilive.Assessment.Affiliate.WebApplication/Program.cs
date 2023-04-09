using MappingProviderCore.Abstract;
using MappingProviderCore.Impl;
using Medilive.Assessment.Affiliate.Business;
using Medilive.Assessment.Affiliate.Business.ReferenceDataManagement;
using Medilive.Assessment.Affiliate.Data;
using Medilive.Assessment.Affiliate.Data.Model.UserManagement;
using Medilive.Assessment.Affiliate.Mapping.Configuration;
using Medilive.Assessment.Affiliate.Mapping.Configuration.UserManagement;
using Medilive.Assessment.Core.Abstract.Data;
using Medilive.Assessment.Core.Abstract.Jwt;
using Medilive.Assessment.Core.Abstract.RuleEngine;
using Medilive.Assessment.Core.Extensions;
using Medilive.Assessment.Core.Tools.Jwt;
using Medilive.Assessment.Core.Tools.ReCaptcha;
using Medilive.Assessment.Core.Tools.RuleEngine;
using Medilive.Assessment.Rules.Configuration.AuthenticationManagement.Login;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region Service Registrations
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var connectionStr = config["Application:ConnectionStrings:AffiliateDb"];

builder.Services.AddScoped<DbContext, MediliveAffiliateDatabaseContext>(serviceProvider => {
    //TODO:implement a logic to get the connection string from the configuration file
    var optionsBuilder = new DbContextOptionsBuilder<MediliveAffiliateDatabaseContext>();

    optionsBuilder.UseSqlServer(connectionStr);
    optionsBuilder.EnableSensitiveDataLogging();

    return new MediliveAffiliateDatabaseContext(optionsBuilder.Options);
});

builder.Services.AddScoped<GoogleReCaptchaService>(serviceProvider => {
    var recaptchaUrl = config["Application:ReCaptcha:VerificationUrl"];
    var recaptchaPrivateKey = config["Application:ReCaptcha:PrivateKey"];
    return new GoogleReCaptchaService(recaptchaUrl, recaptchaPrivateKey);
});

builder.Services.AddScoped<IDataRepository, DataRepositoryDefaultImpl>();
builder.Services.AddScoped<IJwtHelper, JwtHelper>();

#region Register Mapping Configurations
builder.Services.AddSingleton<IMappingServiceProvider, MappingService>(servicebuilder => {
    //TODO:implement a logic to pass configuration class types into the mapper constructor 
    var mappingService = new MappingService(new IMappingConfiguration[] { 
    new GenderToReferenceDataDtoMappingConfiguration()
    ,new AffiliateUserToTokenClaimsMappingConfiguration() 
    ,new RegisterAffiliateUserDtoToAffiliateUserMappingConfiguration()
    ,new ErrorMessagesToResponseMessageMappingConfiguration()
    ,new AffiliateUserToAffiliateUserDtoMappingConfiguration()
    });
    mappingService.SetDependencyResolver(requestedType => servicebuilder.GetService(requestedType));
    return mappingService;
});
#endregion End Of Register Mapping Configurations

#region Rule Configurations
builder.Services.AddSingleton<IRuleServiceProvider>(serviceProvider =>
{
    var service = new RuleServiceProviderDefaultImpl();
    service.AddRule<LoginUsernamePasswordCheckRule>("Login");
    service.AddRule<RegisterFormDataCheckRule>("Register");
    service.SetDependencyResolver(requestedType => serviceProvider.CreateScope().ServiceProvider.GetService(requestedType));

    return service;
});
#endregion End Of Rule Configurations

builder.Services.AddScoped<ReferenceDataManager>();
builder.Services.AddScoped<AffiliateUserManager>();
builder.Services.AddScoped<AuthenticationManager>();

#endregion End Of Service Registrations

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
var tokenHashKey = config["Application:Keys:AuthTokenValidationKey"];
app.UseJwt(tokenHashKey);

#region Route Mappings

#region General Route Mappings
app.MapControllerRoute(
    name: "default",
    pattern: "", defaults: new { controller = "Home", action = "Index" });

app.MapControllerRoute(
    name: "privacy",
    pattern: "privacy", defaults: new { controller = "Home", action = "Privacy" });

app.MapControllerRoute(
    name: "redirect",
    pattern: "redirect", defaults: new { controller = "Home", action = "Redirect" });
#endregion End Of General Route Mappings

#region Reference Data
app.MapControllerRoute(name: "gender-list", pattern: "gender-list", defaults: new { controller = "ReferenceData", action = "GetGenderList" });
#endregion End Of Reference Data

#region User
app.MapControllerRoute(name: "user-registration", pattern: "new-user-registration", defaults: new { controller = "User", action = "Register" });
app.MapControllerRoute(name: "user-home", pattern: "user-home", defaults: new { controller = "User", action = "UserHome" });
app.MapControllerRoute(name: "get-user-info", pattern: "get-user-info", defaults: new { controller = "User", action = "GetUserInfo" });
app.MapControllerRoute(name: "get-user-summary", pattern: "get-user-summary", defaults: new { controller = "User", action = "GetUserSummary" });
#endregion End Of User

#region Authentication
app.MapControllerRoute(name: "user-login", pattern: "user-login", defaults: new { controller = "Authentication", action = "Login" });
app.MapControllerRoute(name: "user-logout", pattern: "user-logout", defaults: new { controller = "Authentication", action = "Logout" });
#endregion End Of Authentication

#endregion End Of Route Mappings

//Add following only after adding app.UseJwt
app.Use(async (req, next) =>
{
    var dataRepository = req.RequestServices.GetService<IDataRepository>();
    var route = dataRepository.Get<Medilive.Assessment.Affiliate.Data.Model.AuthenticationManagement.Route>()
    .FirstOrDefault(route => route.RouteTemplate == req.Request.Path.Value);

    if (route != null)
    {
        if (!req.User.Identity.IsAuthenticated
            && route.Access == Medilive.Assessment.Affiliate.Data.Model.AuthenticationManagement.RouteAccess.Values.AUTHENTICATED_USER)
        {
            req.Response.Redirect("/user-login");
            return;
        }
        if (req.User.Identity.IsAuthenticated
            && route.Access == Medilive.Assessment.Affiliate.Data.Model.AuthenticationManagement.RouteAccess.Values.UNAUTHENTICATED_USER)
        {
            req.Response.Redirect("/user-home");
            return;
        }
    }
    await next();
});

//Set indentification cookie
//Add following only after adding app.UseJwt
app.Use(async (req, next) =>
{
    if (!req.Request.Cookies.ContainsKey("IdentificationCookie"))
    {
        req.Response.Cookies.Append("IdentificationCookie", Guid.NewGuid().ToString());
    }

    var ipNumber = req.Connection.RemoteIpAddress.MapToIPv4().Address;
    var identificationCookie = req.Request.Cookies["IdentificationCookie"];

    var dataRepository = req.RequestServices.GetService<IDataRepository>();
    var route = dataRepository.Get<Medilive.Assessment.Affiliate.Data.Model.AuthenticationManagement.Route>()
    .FirstOrDefault(route => route.RouteTemplate == req.Request.Path.Value);

    var unixNow = DateTime.Now.ToUnixTimeLong();

    // Check if the reference code trial limit has been exceeded more than 2 times within the last 24 hours.
    var isUserBlocked = dataRepository.Get<ClientBlock>().Any(clientBlock =>
        (clientBlock.IdentificationCookie == identificationCookie
        || clientBlock.IpNumber == ipNumber)
        && clientBlock.BlockedUntil > unixNow);

    if (isUserBlocked)
    {
        req.Response.Clear();
        req.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
        return;

    }
    await next();
});

app.Run();
