using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using Domain.Entities;
using Domain.Services;
using MiPrimerMVC.Models;

namespace MiPrimerMVC.Controllers
{
    public class AccountLoginController : Controller
    {
        readonly IReadOnlyRepository _readOnlyRepository;
        readonly IWriteOnlyRepository _writeOnlyRepository;
        //readonly IMappingEngine _mappingEngine;
        public AccountLoginController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository/*, IMappingEngine mappingEngine*/)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
           // _mappingEngine = mappingEngine;
        }

        public ActionResult Login()
        {
            return View(new AccountLoginModel());
        }

        
        [HttpPost]
        public ActionResult Login(AccountLoginModel login, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = _readOnlyRepository.FirstOrDefault<AccountLogin>(x => x.Email == login.Email && x.Password==(new Hasher().Encrypt(login.Password)));
                if (user != null)
                {
                        FormsAuthentication.SetAuthCookie(login.Email, login.RememberMe);
                        //SetAuthenticationCookie(login.Email, user.Role);

                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }


                        return RedirectToAction("ToInbox");
                
                } 
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            } 

            return View(login);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "AccountLogin");
        }

        [Authorize]
        public ActionResult ToInbox()
        {
            
            var mModel = new MessagesModel();
            var x = _readOnlyRepository.FirstOrDefault<AccountLogin>(z=>z.Email==HttpContext.User.Identity.Name);
            mModel.MessagesList= x.AccountMessages.ToList();
            return View(mModel);
        }

        [HttpPost]
        public ActionResult ToInbox(MessagesModel model)
        {
            return View(model);
        }

        public void SetAuthenticationCookie(string userEmail, string roles)
        {
            var cookieSection = (HttpCookiesSection)ConfigurationManager.GetSection("system.web/httpCookies");
            var authenticationSection =
                (AuthenticationSection)ConfigurationManager.GetSection("system.web/authentication");

            var authTicket =
                new FormsAuthenticationTicket(
                    1, userEmail, DateTime.Now,
                    DateTime.Now.AddMinutes(authenticationSection.Forms.Timeout.TotalMinutes),
                    false, roles);

            String encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            if (cookieSection.RequireSSL || authenticationSection.Forms.RequireSSL)
            {
                authCookie.Secure = true;
            }

            HttpContext.Response.Cookies.Add(authCookie);

        }

       }
}