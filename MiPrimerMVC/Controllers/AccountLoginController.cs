using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using CaptchaMvc.HtmlHelpers;
using Domain.Entities;
using Domain.Services;
using MiPrimerMVC.Models;

namespace MiPrimerMVC.Controllers
{
    public class AccountLoginController : Controller
    {
        readonly IReadOnlyRepository _readOnlyRepository;
        readonly IWriteOnlyRepository _writeOnlyRepository;
        public AccountLoginController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
        }

        public ActionResult Login()
        {
            return View(new AccountLoginModel());
        }

        
        [HttpPost]
        public ActionResult Login(AccountLoginModel login)
        {
            var user = _readOnlyRepository.FirstOrDefault<AccountLogin>(x => x.Email == login.Email);
            if (user == null)
            {
                MessageBox.Show("User doesnt Exists.");
            }
            else
            {
                if (!user.Password.Equals(new Hasher().Encrypt(login.Password)))
                {
                    MessageBox.Show("Incorrect Password.");
                }
                else
                {
                    if (Session["Accounts"] == null)
                    Session["Accounts"] = user;


                    return RedirectToAction("ToInbox");
                }
            }

            return View(login);
        }
    
        public ActionResult ToInbox()
        {
            //direct user value
            //var user = _readOnlyRepository.FirstOrDefault<AccountLogin>(x => x.Id == 1);
            //Try Sessions
            
            var user = (AccountLogin)Session["Accounts"];
            Session["Accounts"] = user;
            var mModel = new MessagesModel();
            var x = _readOnlyRepository.GetById<AccountLogin>(user.Id);
            mModel.MessagesList= x.AccountMessages.ToList();
            return View(mModel);
        }

        [HttpPost]
        public ActionResult ToInbox(MessagesModel Model)
        {
            return View(Model);
        }

   
        public ActionResult ToHome()
        {
            return View("~/Views/CalculadoraRomana/Index.cshtml");
        }

       }
}