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
                var user = _readOnlyRepository.FirstOrDefault<AccountLogin>(x => x.Email == login.Email && x.Password==(new Hasher().Encrypt(login.Password)) && !x.Archived);
                if (user != null)
                {
                        FormsAuthentication.SetAuthCookie(login.Email, login.RememberMe);
                        SetAuthenticationCookie(login.Email, user.Role);

                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }


                        return RedirectToAction("ToInbox");
                
                } 
                ModelState.AddModelError("", "The user name or password provided is incorrect or not active.");
            } 

            return View(login);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "AccountLogin");
        }

        [Authorize]
        public  ActionResult Profile()
        {
            var manageProfile = new ManageProfileModel();
            manageProfile.CurrentUser = _readOnlyRepository.FirstOrDefault<AccountLogin>(z => z.Email == HttpContext.User.Identity.Name);

            return View(manageProfile);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Profile(ManageProfileModel model)
        {
            model.CurrentUser = _readOnlyRepository.FirstOrDefault<AccountLogin>(z => z.Email == HttpContext.User.Identity.Name);
            if (ModelState.IsValid)
            {
                if(model.CurrentUser.Name != model.Name || model.Name.Trim()=="")
                model.CurrentUser.Name = model.Name;

                if (model.CurrentUser.UserInfo.Blog!=model.Blog || model.Blog.Trim()=="")
                model.CurrentUser.UserInfo.Blog = model.Blog;

                if(model.CurrentUser.UserInfo.Cellphone!=model.Cellphone || model.Cellphone.Trim()=="")
                model.CurrentUser.UserInfo.Cellphone = model.Cellphone;

                if (model.CurrentUser.UserInfo.OfficePhone != model.OfficePhone || model.OfficePhone.Trim() == "")
                    model.CurrentUser.UserInfo.OfficePhone = model.OfficePhone;

                if (model.CurrentUser.UserInfo.Cellphone != model.Description || model.Description.Trim() == "")
                    model.CurrentUser.UserInfo.Description = model.Description;

                    model.CurrentUser.UserInfo.Country = model.Country;

                if (model.CurrentUser.UserInfo.Gender != model.Gender)
                    model.CurrentUser.UserInfo.Gender = model.Gender;

                if (model.CurrentUser.UserInfo.ProfileImg != model.ProfileImg || model.ProfileImg.Trim() == "")
                    model.CurrentUser.UserInfo.ProfileImg = model.ProfileImg;

                _writeOnlyRepository.Update(model.CurrentUser);

            } ModelState.AddModelError("", "Something went wrong.");


            return View(model);
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

        [Authorize(Roles = "ADMIN")]
        public ActionResult ManageClassifieds()
        {
            var allUser = _readOnlyRepository.GetAll<AccountLogin>().ToList(); 
            var model = new ManageUModel
            {
                UserList = allUser, 
            };


            return View(model);
        }

        long _tempId;
        [Authorize(Roles = "ADMIN")]
        public ActionResult ArchiveClassified(long id)
        {
            _tempId = id;
            var explanationMessage = new MessagesModel();
            explanationMessage.ClassifiedsShown = _readOnlyRepository.FirstOrDefault<Classifieds>(x => x.Id == _tempId);


            return View(explanationMessage);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ActionResult ArchivedClassified(MessagesModel model)
        {
            var classifiedToBeArchived = _readOnlyRepository.FirstOrDefault<Classifieds>(x => x.Id == _tempId);
            classifiedToBeArchived.AdminArchive();
            _writeOnlyRepository.Update(classifiedToBeArchived);

            var user = _readOnlyRepository.FirstOrDefault<AccountLogin>(z => z.Email == classifiedToBeArchived.Email);
            model.SendMessage.Name = HttpContext.User.Identity.Name;
            model.SendMessage.Message += "The classified: " + classifiedToBeArchived.Article + " has been removed for: " + model.SendMessage.Message;
            user.AddMessage(model.SendMessage);
            _writeOnlyRepository.Update(user);

            return RedirectToAction("ManageClassifieds");
        }
        
        [Authorize(Roles = "ADMIN")]
        public ActionResult ActivateClassified(long id)
        {
            _tempId = id;
            
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ActionResult ActivateClassified(MessagesModel model)
        {
            var classifiedToBeArtivated = _readOnlyRepository.FirstOrDefault<Classifieds>(x => x.Id == _tempId);
            classifiedToBeArtivated.AdminActivate();
            _writeOnlyRepository.Update(classifiedToBeArtivated);

            var user = _readOnlyRepository.FirstOrDefault<AccountLogin>(z => z.Email == classifiedToBeArtivated.Email);
            model.SendMessage.Name = HttpContext.User.Identity.Name;
            model.SendMessage.Message += "The classified: " + classifiedToBeArtivated.Article + " has been restored because: " + model.SendMessage.Message;
            user.AddMessage(model.SendMessage);
            _writeOnlyRepository.Update(user);

            return RedirectToAction("ManageClassifieds");
        }

        public ActionResult PublicProfile(long id)
        {
            
            var publicUser = new ProfileModel();
            publicUser.PublicUser = _readOnlyRepository.FirstOrDefault<AccountLogin>(x => x.Id == id);

            if (HttpContext.User.Identity.Name != string.Empty) { 
                var activeUser = _readOnlyRepository.FirstOrDefault<AccountLogin>(x => x.Email == HttpContext.User.Identity.Name);
                
                if (activeUser.Id == id)
                {
                    return RedirectToAction("Profile");
                }

                if (!_readOnlyRepository.GetAll<Subscriptions>().ToList().FindAll(x => x.Follower == activeUser.Id && x.Following == publicUser.PublicUser.Id && !x.Archived).Any())
                {
                    publicUser.IsFollowing = false;
                }
                else
                {
                    publicUser.IsFollowing = true;
                }
            }

            return View(publicUser);
        }

        [Authorize]
        public ActionResult Follow(long id)
        {
            var activeUser = _readOnlyRepository.FirstOrDefault<AccountLogin>(x => x.Email == HttpContext.User.Identity.Name);
            var userprofile = _readOnlyRepository.FirstOrDefault<AccountLogin>(x => x.Id == id);
            var subscriptionToBeActivated=_readOnlyRepository.FirstOrDefault<Subscriptions>(x => x.Follower == activeUser.Id && x.Following == userprofile.Id);

            if (subscriptionToBeActivated!=null)
            {
                subscriptionToBeActivated.Activate();
                _writeOnlyRepository.Update(subscriptionToBeActivated);
            }
            else
            {
               _writeOnlyRepository.Create(new Subscriptions(activeUser.Id, userprofile.Id));
            }
            var notify = new Notifications(activeUser.Email,activeUser.Name, "Subs");
            userprofile.Notifications.ToList().Add(notify);
            _writeOnlyRepository.Update(userprofile);

            return RedirectToAction("PublicProfile", new{id});
        }

        [Authorize]
        public ActionResult UnFollow(long id)
        {
            var activeUser = _readOnlyRepository.FirstOrDefault<AccountLogin>(x => x.Email == HttpContext.User.Identity.Name);
            var userprofile = _readOnlyRepository.FirstOrDefault<AccountLogin>(x => x.Id == id);
            var subscriptionToBeArchived = _readOnlyRepository.FirstOrDefault<Subscriptions>(x => x.Follower == activeUser.Id && x.Following == userprofile.Id);
            subscriptionToBeArchived.Archive();
            _writeOnlyRepository.Update(subscriptionToBeArchived);

            return RedirectToAction("PublicProfile", new{id});
        }

       }
}