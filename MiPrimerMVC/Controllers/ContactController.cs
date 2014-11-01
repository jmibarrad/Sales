using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using Domain.Entities;
using Domain.Services;
using MiPrimerMVC.Models;

namespace MiPrimerMVC.Controllers
{
    public class ContactController : Controller
    {
        readonly IReadOnlyRepository _readOnlyRepository;
        readonly IWriteOnlyRepository _writeOnlyRepository;
        public ContactController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
        }

        public ActionResult Contact()
        {
            return View(new ContactModel());
        }

        [HttpPost]
        [AcceptVerbs("POST","HEAD")]
        public ActionResult Contact(ContactModel cModel)
        {
            if (!ValidateText(cModel.Message))
            {
                MessageBox.Show("Muy corto");
            }
            else
            {
                if (!ValidateLength(cModel.Message))
                {
                    MessageBox.Show("No mas de 250 caracteres");
                }
                else
                {
                    var user = _readOnlyRepository.FirstOrDefault<AccountLogin>(x => x.Id == 1);
                   
                    var messageList = user.AccountMessages.ToList();
                    messageList.Add(new Messages(cModel.Email,cModel.Name,cModel.Message));
                    user.AccountMessages = messageList;
                    _writeOnlyRepository.Update(user);

                }
            }

            return View(cModel);
        }



        public static Boolean ValidateText(string message)
        {

            var wordCounter = message.Split(' ');
            
            if (wordCounter.Count() < 3&&!ValidateOnlyLetters(message))
            {
                return false;
            }
            return true;
        }

        public static Boolean ValidateOnlyLetters(string message)
        {
            Boolean correct = true;
            foreach (char c in message)
            {
                if (char.IsDigit(c)) correct = false;
                else if (char.IsSymbol(c)) correct = false;
            }
            return correct;
        }

        public static Boolean ValidateLength(string message)
        {
            if (message.Length > 250)
            {
                return false;
            }
            return true;
        }
    }
}