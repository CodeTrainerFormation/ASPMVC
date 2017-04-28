# Solution Lab-10

[Voir le différentiel avec le lab précédent](https://github.com/CodeTrainerFormation/ASPMVC/commit/ecb8036678fadc088977e9df9724af1fc8a5f6ef?diff=split)

## Préparation

Ajouter les packages suivants avec Nuget Package Manager au projet `NetSchool` : 
- Microsoft.AspNet.Identity.Core
- Microsoft.AspNet.Identity.Owin
- Microsoft.AspNet.Identity.EntityFramework
- Microsoft.Owin.Host.SystemWeb
- Microsoft.Owin.Host.Security.Facebook
- Microsoft.Owin.Host.Security.Twitter
- Microsoft.Owin.Host.Security.Google
- Microsoft.Owin.Host.Security.MicrosoftAccount

Ajouter les packages suivants au projet `DomainModel` : 
- Microsoft.AspNet.Identity.Core
- Microsoft.AspNet.Identity.EntityFramework

Ajouter les packages suivants au projet `DAL` : 
- Microsoft.AspNet.Identity.Core
- Microsoft.AspNet.Identity.EntityFramework

## Configuration

### Projet DomainModel
Dans le projet `DomainModel`, créer la classe `ApplicationUser` qui hérite de `IdentityUser`
```C#
public class ApplicationUser : IdentityUser
{
    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
    {
        // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
        // Add custom user claims here
        return userIdentity;
    }
}
```

### Projet DAL
Dans le projet `DAL`, modifier la classe `SchoolDb` : 
- changer l'héritage de la classe pour `IdentityDbContext<ApplicationUser>`
```C#
public class SchoolDb : IdentityDbContext<ApplicationUser>
{
    // ...
}
```

- ajouter une fonction `Create` dans la classe
```C#
public static SchoolDb Create()
{
    return new SchoolDb();
}
```

### Projet NetSchool
Dans le dossier `App_Start`, créer une classe `IdentityConfig`. Ce fichier va contenir plusieurs classes en réalité ([Voir le fichier](https://github.com/CodeTrainerFormation/ASPMVC/blob/10-Identity/NetSchool/NetSchoolWeb/App_Start/IdentityConfig.cs)).
```C#
using DomainModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using DAL;

namespace NetSchoolWeb.App_Start
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<SchoolDb>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                //RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                //RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
```

Toujours dans le dossier `App_Start`, créer une classe partiel nommée `Startup.Auth.cs`. ([Voir le fichier](https://github.com/CodeTrainerFormation/ASPMVC/blob/10-Identity/NetSchool/NetSchoolWeb/App_Start/Startup.Auth.cs))
```C#
using DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;
using DomainModel;
using NetSchoolWeb.App_Start;

namespace NetSchoolWeb
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(SchoolDb.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
			
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));
			
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Décommenter pour utiliser l'un des provider suivants
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
    }
}
```

Ajouter une classe `Startup` à la racine du projet `NetSchool`
```C#
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NetSchoolWeb.Startup))]
namespace NetSchoolWeb
{
    public partial class Startup
    { 
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
```

## Création des contrôleurs

Dans le dossier `Controllers` du projet `NetSchool`:
- créer le contrôleur `AccountController` [Code du contrôleur](https://github.com/CodeTrainerFormation/ASPMVC/blob/10-Identity/NetSchool/NetSchoolWeb/Controllers/AccountController.cs)
- créer le contrôleur `ManageController` [Code du contrôleur](https://github.com/CodeTrainerFormation/ASPMVC/blob/10-Identity/NetSchool/NetSchoolWeb/Controllers/ManageController.cs)

## Création des 'ViewModels'

Dans le projet `NetSchool`, créer un dossier `ViewModels`.

Dans ce dossier créer les classes suivante : 
- `AccountViewModels` [Code du ViewModel](https://github.com/CodeTrainerFormation/ASPMVC/blob/10-Identity/NetSchool/NetSchoolWeb/ViewModels/AccountViewModels.cs)
- `ManageViewModels` [Code du ViewModel](https://github.com/CodeTrainerFormation/ASPMVC/blob/10-Identity/NetSchool/NetSchoolWeb/ViewModels/ManageViewModels.cs)

## Création et modification des vues

### Créer les vues du contrôleur 'AccountController'
- [_ExternalLoginListPartial.cshtml](https://github.com/CodeTrainerFormation/ASPMVC/blob/10-Identity/NetSchool/NetSchoolWeb/Views/Account/_ExternalLoginListPartial.cshtml)
- [ConfirmEmail.cshtml](https://github.com/CodeTrainerFormation/ASPMVC/blob/10-Identity/NetSchool/NetSchoolWeb/Views/Account/ConfirmEmail.cshtml)
- [ExternalLoginConfirmation.cshtml](https://github.com/CodeTrainerFormation/ASPMVC/blob/10-Identity/NetSchool/NetSchoolWeb/Views/Account/ExternalLoginConfirmation.cshtml)
- [ExternalLoginFailure.cshtml](https://github.com/CodeTrainerFormation/ASPMVC/blob/10-Identity/NetSchool/NetSchoolWeb/Views/Account/ExternalLoginFailure.cshtml)
- [ForgotPassword.cshtml](https://github.com/CodeTrainerFormation/ASPMVC/blob/10-Identity/NetSchool/NetSchoolWeb/Views/Account/ForgotPassword.cshtml)
- [ForgotPasswordConfirmation.cshtml](https://github.com/CodeTrainerFormation/ASPMVC/blob/10-Identity/NetSchool/NetSchoolWeb/Views/Account/ForgotPasswordConfirmation.cshtml)
- [Login.cshtml](https://github.com/CodeTrainerFormation/ASPMVC/blob/10-Identity/NetSchool/NetSchoolWeb/Views/Account/Login.cshtml)
- [Register.cshtml](https://github.com/CodeTrainerFormation/ASPMVC/blob/10-Identity/NetSchool/NetSchoolWeb/Views/Account/Register.cshtml)
- [ResetPassword.cshtml](https://github.com/CodeTrainerFormation/ASPMVC/blob/10-Identity/NetSchool/NetSchoolWeb/Views/Account/ResetPassword.cshtml)
- [ResetPasswordConfirmation.cshtml](https://github.com/CodeTrainerFormation/ASPMVC/blob/10-Identity/NetSchool/NetSchoolWeb/Views/Account/ResetPasswordConfirmation.cshtml)
- [SendCode.cshtml](https://github.com/CodeTrainerFormation/ASPMVC/blob/10-Identity/NetSchool/NetSchoolWeb/Views/Account/SendCode.cshtml)
- [VerifyCode.cshtml](https://github.com/CodeTrainerFormation/ASPMVC/blob/10-Identity/NetSchool/NetSchoolWeb/Views/Account/VerifyCode.cshtml)


### Créer les vues du contrôleur 'ManageController'
- [AddPhoneNumber.cshtml](https://github.com/CodeTrainerFormation/ASPMVC/blob/10-Identity/NetSchool/NetSchoolWeb/Views/Manage/AddPhoneNumber.cshtml)
- [ChangePassword.cshtml](https://github.com/CodeTrainerFormation/ASPMVC/blob/10-Identity/NetSchool/NetSchoolWeb/Views/Manage/ChangePassword.cshtml)
- [Index.cshtml](https://github.com/CodeTrainerFormation/ASPMVC/blob/10-Identity/NetSchool/NetSchoolWeb/Views/Manage/Index.cshtml)
- [ManageLogins.cshtml](https://github.com/CodeTrainerFormation/ASPMVC/blob/10-Identity/NetSchool/NetSchoolWeb/Views/Manage/ManageLogins.cshtml)
- [SetPassword.cshtml](https://github.com/CodeTrainerFormation/ASPMVC/blob/10-Identity/NetSchool/NetSchoolWeb/Views/Manage/SetPassword.cshtml)
- [VerifyPhoneNumber.cshtml](https://github.com/CodeTrainerFormation/ASPMVC/blob/10-Identity/NetSchool/NetSchoolWeb/Views/Manage/VerifyPhoneNumber.cshtml)


### Créer les vues partagées
- [_LoginPartial.cshtml](https://github.com/CodeTrainerFormation/ASPMVC/blob/10-Identity/NetSchool/NetSchoolWeb/Views/Shared/_LoginPartial.cshtml)
- [Lockout.cshtml](https://github.com/CodeTrainerFormation/ASPMVC/blob/10-Identity/NetSchool/NetSchoolWeb/Views/Shared/Lockout.cshtml)

### Modifier les layout

- Editer le fichier `Views\_ViewStart.cshtml` 
```cshtml
@{
    var controller = HttpContext.Current.Request.RequestContext.RouteData.Values["Controller"].ToString();
    string cLayout = "";
    if (controller == "Account")
    {
        cLayout = "~/Views/Shared/_Layout.cshtml";
    }
    else
    {
        cLayout = "~/Views/Shared/_AdminLayout.cshtml";
    }
    Layout = cLayout;
}
```

- Editer les liens de navigation dans le fichier `Views\Shared\_Layout.cshtml`
```cshtml
<div class="navbar-collapse collapse">
    <ul class="nav navbar-nav">
        <li>@Html.ActionLink("Se connecter", "Login", "Account")</li>
        <li>@Html.ActionLink("S'inscrire", "Register", "Account")</li>
    </ul>
</div>
```

- Editer les liens de navigation dans le fichier `Views\Shared\_Layout.cshtml`
```cshtml
<div id="navbar" class="navbar-collapse collapse">
                    
    @using (Html.BeginForm("LogOff", "Account", FormMethod.Post, new { id = "logoutForm" }))
    {
        @Html.AntiForgeryToken()
    <ul class="nav navbar-nav navbar-right">
            @Html.LiActionLink("Profile", "Index", "Manage")
                            
            <li><a href="javascript:document.getElementById('logoutForm').submit()">Log off</a></li>
                        
    </ul>
    }
</div>
```

## Modifier la route par défaut de l'application

Editer le fichier `App_Start\RouteConfig` 
```C#
public class RouteConfig
{
    public static void RegisterRoutes(RouteCollection routes)
    {
	    // ...
		routes.MapRoute(
			name: "Default",
			url: "{controller}/{action}/{id}",
			defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
		);
	}
}
```

## Utilisation

Pour soumettre un contrôleur à l'authentification, annoter celui ci avec `[Authorize]`.

Par exemple : 

```C#
[Authorize]
public class StudentController : Controller
{
    //...
}
```



