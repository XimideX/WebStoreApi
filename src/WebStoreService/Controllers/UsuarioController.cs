using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStoreDAO.CoreDAO;

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

        [HttpGet("[action]")]
        public async Task<IdentityUser> Login(string user, string password)
        {
            var userDatabase = await userManager.FindByIdAsync(user);
            if (user != null)
            {
                var userSignIn = await signInManager.PasswordSignInAsync(user, password, false, false);
                if (userSignIn.Succeeded)
                {
                    return userDatabase;
                } else  
                {
                    // TODO error when trying to signin user
                }
            }
            return userDatabase;
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
                _logger.LogInformation("Message sent");;
                return "Success";
            }
            catch (Exception ex)
            {
                _logger.LogInformation("error: ", ex);;
                return "Fail " + ex.Message;
            }
        }

        [HttpGet("[action]")]
        public async Task<IdentityUser> Register(string newUsername, string newPassword)
        {
            var user = new IdentityUser();
            user.UserName = newUsername;

            var result = userManager.CreateAsync(user, newPassword);
            if (result.IsCompletedSuccessfully)
            {
                await SendEmail(newPassword);
                var userSignIn = await signInManager.PasswordSignInAsync(user, newPassword, false, false);
                // userSignIn 
                if (userSignIn.Succeeded)
                {
                    return user;
                } else 
                {
                    // TODO error when trying to signin user
                }
                // TODO send e-mail to confirm and only then, access the website
            }


            return user;

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
            await signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
        }
    }
}