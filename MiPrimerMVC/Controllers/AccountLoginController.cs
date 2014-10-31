using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using Domain.Entities;
using Domain.Services;
using MiPrimerMVC.Models;

namespace MiPrimerMVC.Controllers
{
    public class AccountLoginController : Controller
    {
        readonly IReadOnlyRepository _readOnlyRepository;
        readonly IWriteOnlyRepository _writeOnlyRepository;
        public AccountLoginController(ICalculadora calc, IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository)
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
                    return RedirectToAction("ToHome");
                }
            }

            return View(login);
        }

        public ActionResult ToHome()
        {
            return View("~/Views/CalculadoraRomana/Index.cshtml");
        }
    }
}