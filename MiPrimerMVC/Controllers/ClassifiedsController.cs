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
        public ActionResult CreateClassified(ClassifiedModel model)
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

        public ActionResult AllClasifieds()
        {
            return View(new ClassiModel());
        }

        [HttpPost]
        public ActionResult AllClasifieds(ClassiModel model)
        {
            return View();
        }

     
        private static Dictionary<long, string> _categoryIndex = new Dictionary<long, string>
        {
            {1,"Automoviles"},
            {2,"Instruments"},
            {3,"VG. Consoles"},
            {4,"Technology"}
 
        };

        public ActionResult ByCategory(long? id)
        {
            var cByCategory = _readOnlyRepository.GetAll<Classifieds>().ToList();
            var cByCategoryModel = new ClassiModel
            {
                myClassifiedsList = cByCategory.FindAll(x => x.Category == _categoryIndex[id])
            };

            cByCategoryModel.myClassifiedsList.Reverse();

            return View(cByCategoryModel);
        }

        public ActionResult AdvancedSearch()
        {
            var cAdvancedSearch = new ClassiModel()
            {
                myClassifiedsList = _readOnlyRepository.GetAll<Classifieds>().ToList()
            };

            return View(cAdvancedSearch);
        }

        [HttpPost]
        public ActionResult AdvancedSearch(ClassiModel model)
        {
            var filters = _readOnlyRepository.GetAll<Classifieds>().ToList();

            //filters = filters.FindAll(x => x.Article == model.Cas.Title);

            if (model.Cas.Title != null)
            {
                filters = filters.FindAll(x => x.Article.Contains(model.Cas.Title));
            }

            if (model.Cas.Description != null)
            {
                filters = filters.FindAll(x => x.Description.Contains(model.Cas.Description));
            }

            model.myClassifiedsList = filters;

            return View(model);
        }
    


        public class ClassiModel
        {
            public List<Classifieds> myClassifiedsList { get; set; }
            public CategoryAdvancedSearch Cas { get; set; }
        }

        public class CategoryAdvancedSearch
        {
            public string Category { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
        }

    }
}