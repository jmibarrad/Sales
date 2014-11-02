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
    public class ClassifiedsController: Controller
    {
         readonly IReadOnlyRepository _readOnlyRepository;
        readonly IWriteOnlyRepository _writeOnlyRepository;
        public ClassifiedsController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
        }

        public ActionResult CreateClassified()
        {
            if (Session["Accounts"] == null)
                return View("~/Views/AccountLogin/Login.cshtml");

            return View(new ClassifiedModel());
        }

        
        [HttpPost]
        public ActionResult Pene(ClassifiedModel model)
        {
            var validate = new IValidate();

            if (!(validate.ValidateTextWords(model.Article, 1) && validate.ValidateTextLength(model.Article, 1, 100)))
            {
                MessageBox.Show("1. At Least a Word\n2. 100 characters Maximum");
            }
            else
            {
                if (!(validate.ValidateTextWords(model.Description, 3) && validate.ValidateTextLength(model.Description, 1, 250)))
                {
                    MessageBox.Show("1. At Least 3 Words\n2. 250 characters Maximum");
                }
                else
                {
                    if (!(validate.ValidateUrl(model.UrlImage) && validate.ValidateUrl(model.UrlVideo)))
                    {
                        MessageBox.Show("URLs Not valid.");
                    }
                    else
                    {

                        var user2 = (AccountLogin)Session["Accounts"];
                        Session["Accounts"] = user2;
                        var user = _readOnlyRepository.FirstOrDefault<AccountLogin>(x => x.Id == user2.Id);
                        var classifiedsList = user.AccountClassifieds.ToList();

                        classifiedsList.Add(new Classifieds(model.Category, model.Article, model.ArticleModel, model.Location,
                          model.Price, model.Description, user.Email, model.UrlImage, model.UrlVideo));

                        user.AccountClassifieds = classifiedsList;
                        _writeOnlyRepository.Update(user);
                    }
                }
            }

            return View("CreateClassified",model);
        }


        public ActionResult MyClassifieds()
        {
            var mcModel = new ClassiModel();
            if (Session["Accounts"] != null) { 
            var user = (AccountLogin)Session["Accounts"];
            Session["Accounts"] = user;
            var x = _readOnlyRepository.GetById<AccountLogin>(user.Id);
            mcModel.myClassifiedsList= x.AccountClassifieds.ToList();
            }
            else
            {
                return View("~/Views/AccountLogin/Login.cshtml");
            }

            return View(mcModel);
        }

        [HttpPost]
        public ActionResult MyClassifieds(ClassiModel model)
        {
            return View(model);
        }

        public class ClassiModel
        {
            public List<Classifieds> myClassifiedsList { get; set; } 
        }
    }
}