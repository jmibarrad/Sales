using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
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
                          model.Price, model.Description, user.Email, model.UrlImage, validate.Embed(model.UrlVideo)));

                        user.AccountClassifieds = classifiedsList;
                        _writeOnlyRepository.Update(user);
                        MessageBox.Show("Classified added successfully");

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

        public static string EmailReceiver="";
        public ActionResult Detailed(long id)
        {
            var classified = _readOnlyRepository.GetById<Classifieds>(id);
            var classiModel = new ClassiModel();
            classiModel.classifiedForDetail = classified;
            EmailReceiver = classiModel.classifiedForDetail.Email;

            classiModel.classifiedForDetail.Visited++;
            _writeOnlyRepository.Update(classiModel.classifiedForDetail);

            return View(classiModel);
        }

        [HttpPost]
        public ActionResult Detailed(ClassiModel model)
        {
            var validate = new IValidate();
            if (!(validate.ValidateTextLength(model.sendEmail.Name,3,50)&&validate.ValidateOnlyLetters(model.sendEmail.Name)))
            {
                MessageBox.Show("Requirements Name: \n1. Between 3 to 50 characters\n2. Only Letters");

            }
            else
            {
                if (!(validate.ValidateTextWords(model.sendEmail.Message, 3) && validate.ValidateTextLength(model.sendEmail.Message, 3, 250)))
                {
                    MessageBox.Show("Requirements Question: \n1. No more than 250 characters\n2. At least three words");

                }
                else
                {

                    var user = _readOnlyRepository.FirstOrDefault<AccountLogin>(x => x.Email == EmailReceiver);
                    var messageList = user.AccountMessages.ToList();
                    messageList.Add(new Messages(model.sendEmail.Email, model.sendEmail.Name, model.sendEmail.Message, "Classified|Info"));
                    user.AccountMessages = messageList;
                    _writeOnlyRepository.Update(user);
                    MailTo.SendSimpleMessage(EmailReceiver, model.sendEmail.Name, model.sendEmail.Message);
                    MessageBox.Show("Email sent successfully");
                }
            }

            return View(model);
        }

        public ActionResult ByCategory()
        {
            var cSimpleSearch = new ClassiModel()
            {
                myClassifiedsList = _readOnlyRepository.GetAll<Classifieds>().ToList()
            };
            cSimpleSearch.myClassifiedsList.Reverse();
            return View(cSimpleSearch);
        }

        [HttpPost]
        public ActionResult ByCategory(ClassiModel model)
        {
            var filters = _readOnlyRepository.GetAll<Classifieds>().ToList();

            if (model.Cas.Category != null)
            {
                filters = filters.FindAll(x => x.Category.ToUpper().Contains(model.Cas.Category.ToUpper()));
            }

            filters.Reverse();
            model.myClassifiedsList = filters;

            return View(model);
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

        public ActionResult SimpleSearch()
        {
            var cSimpleSearch = new ClassiModel()
            {
                myClassifiedsList = _readOnlyRepository.GetAll<Classifieds>().ToList()
            };

            return View(cSimpleSearch);
        }

        [HttpPost]
        public ActionResult SimpleSearch(ClassiModel model)
        {
            var filters = _readOnlyRepository.GetAll<Classifieds>().ToList();
            
            if (model.Cas.Title != null)
            {
                filters = filters.FindAll(x => x.Article.ToUpper().Contains(model.Cas.Title.ToUpper()));
            }

            model.myClassifiedsList = filters;

            return View(model);
        }

        public ActionResult MostVisited()
        {
            var classifiedVisited = _readOnlyRepository.GetAll<Classifieds>().ToList();
            classifiedVisited = classifiedVisited.FindAll(x => x.Visited > 0);

            var top = new List<Classifieds>();

            if (classifiedVisited.Count > 5)
            {
                while (top.Count < 5)
                {
                    var biggest = classifiedVisited[0];
                    for (int i = 1; i < classifiedVisited.Count; i++)
                    {
                        if (biggest.Visited < classifiedVisited[i].Visited)
                        {
                            biggest = classifiedVisited[i];
                        }
                    }
                    top.Add(biggest);
                    classifiedVisited.Remove(biggest);
                }

                classifiedVisited = top;
            }

            var Model = new ThreeListClassifiedsModel
            {
                MostVisitedList = classifiedVisited,
                RecentList = MostRecent(),
                FeaturedList = Featured()
            };

            return View(Model);
        }

        public List<Classifieds> MostRecent()
        {
            var l = _readOnlyRepository.GetAll<Classifieds>().ToList();
            l.Reverse();

            if (l.Count > 5)
                l.RemoveRange(5, l.Count - 5);

         

            return l;
        }

        public List<Classifieds> Featured()
        {
            var alles = _readOnlyRepository.GetAll<Classifieds>().ToList();
            var toModel = new List<Classifieds>();

            if (alles.Count > 10)
            {
                var list = new List<Classifieds>();

                var music = alles.FindAll(x => x.Category == "Automoviles");

                var hh = alles.FindAll(x => x.Category == "Instruments");

                var ser = alles.FindAll(x => x.Category == "VG. Consoles");

                var vehicles = alles.FindAll(x => x.Category == "Techonology");

                music.Reverse();
                hh.Reverse();
                ser.Reverse();
                vehicles.Reverse();

                list.Add(music.First());
                list.Add(hh.First());
                list.Add(ser.First());
                list.Add(vehicles.First());
                
                alles = toModel;
            }
           
            return alles;
        }

        public class ClassiModel
        {
            //For Details
            public Classifieds classifiedForDetail { get; set; }
            public SendEmail sendEmail { get; set; }
            
            //For Filters
            public List<Classifieds> myClassifiedsList { get; set; }
            public CategoryAdvancedSearch Cas { get; set; }
        }

        public class CategoryAdvancedSearch
        {
            public string Category { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
        }

        public class SendEmail
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Message { get; set; }
            public string Email2 { get; set; }
        }

        public class ThreeListClassifiedsModel
        {
            public List<Classifieds> MostVisitedList { get; set; }
            public List<Classifieds> RecentList { get; set; }
            public List<Classifieds> FeaturedList { get; set; }
        }
    }
}