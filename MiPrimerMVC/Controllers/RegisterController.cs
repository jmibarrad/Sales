using System;
using System.Web.Mvc;
using System.Windows.Forms;
using Domain.Entities;
using Domain.Services;
using MiPrimerMVC.Models;

namespace MiPrimerMVC.Controllers
{
    public class RegisterController: Controller
    {
        readonly IReadOnlyRepository _readOnlyRepository;
        readonly IWriteOnlyRepository _writeOnlyRepository;
        public RegisterController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
        }

        public ActionResult Register()
        {
            return View(new RegisterModel());
        }
        [HttpPost]
        public ActionResult Register(RegisterModel rModel)
        {
           if (ModelState.IsValid)
            {

                        var user = _readOnlyRepository.FirstOrDefault<AccountLogin>(x => x.Email == rModel.Email);
                        if (user != null)
                        {
                            //check
                            MessageBox.Show("User Exists.");
                        }
                        else
                        {
                           var registeredAccount = new AccountLogin(rModel.Email, rModel.Name ,(new Hasher()).Encrypt(rModel.Password), "user");
                           
                            _writeOnlyRepository.Create(registeredAccount);
                            MailTo.SendSimpleMessage(rModel.Email, rModel.Name, "Gracias por Registrarse");

                        }ModelState.AddModelError("", "Something went wrong with your credentials.");
        
            }

            return View(rModel);
        }

        #region ValPassword
        public static bool ValidatePassword( string password )
        {
          const int MIN_LENGTH =  8 ;
          const int MAX_LENGTH = 20 ;

          if ( password == null ) throw new ArgumentNullException() ;

          bool meetsLengthRequirements = password.Length >= MIN_LENGTH && password.Length <= MAX_LENGTH ;
          bool hasUpperCaseLetter      = false ;
          bool hasLowerCaseLetter      = false ;
          bool hasDecimalDigit         = false ;

          if ( meetsLengthRequirements )
          {
            foreach (char c in password )
            {
              if      ( char.IsUpper(c) ) hasUpperCaseLetter = true ;
              else if ( char.IsLower(c) ) hasLowerCaseLetter = true ;
              else if ( char.IsDigit(c) ) hasDecimalDigit    = true ;
            }
          }

          bool isValid = meetsLengthRequirements
                      && hasUpperCaseLetter
                      && hasLowerCaseLetter
                      && hasDecimalDigit
                      ;
          return isValid ;

        }
        #endregion 

    }
}