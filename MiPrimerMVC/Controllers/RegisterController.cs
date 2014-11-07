using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
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
            if (!(rModel.Name.Length>3&&rModel.Name.Length<=50))
            {
                MessageBox.Show("Name more than 3 letters and less than 50.");

            }else{ 

            if (!ValidatePassword(rModel.Password))
            {
                MessageBox.Show("Passwords requirement are: \n1. Length between {8,15}.\n2.At least an Uppercase.\n3.At least a number.\n4.No special characters.");


            }else{    
                    if (!((new Hasher()).Encrypt(rModel.Password).Equals((new Hasher()).Encrypt(rModel.ConfirmPassword))))
                    {
                        MessageBox.Show("Passwords don´t match.");
                    }
                    else { 
                
                        var user = _readOnlyRepository.FirstOrDefault<AccountLogin>(x => x.Email == rModel.Email);
                        if (user != null)
                        {
                            MessageBox.Show("User Exists.");
                        }
                        else
                        {
                           var RegisteredAccount = new AccountLogin(rModel.Email, rModel.Name ,(new Hasher()).Encrypt(rModel.Password), "user");
               
                            var createdOperation = _writeOnlyRepository.Create(RegisteredAccount);
                            MailTo.SendSimpleMessage(rModel.Email, rModel.Name, "Gracias por Registrarse");

                        }           
                    }
                }
            }
            return View(rModel);
        }

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
     

    }
}