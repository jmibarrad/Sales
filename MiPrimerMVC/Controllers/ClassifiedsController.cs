using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Windows.Forms;
using Domain.Entities;
using Domain.Services;
using MiPrimerMVC.Models;
using MiPrimerMVC.ValidationAttributes;

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

        [Authorize]
        public ActionResult CreateClassified()
        {
            return View(new ClassifiedModel());
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateClassified(ClassifiedModel model)
        {
            if (ModelState.IsValid)
            {
                        var validate = new IValidate();
                        var user = _readOnlyRepository.FirstOrDefault<AccountLogin>(x => x.Email == HttpContext.User.Identity.Name);
                        var classifiedsList = user.AccountClassifieds.ToList();

                        classifiedsList.Add(new Classifieds(model.Category, model.Article, model.ArticleModel, model.Location,
                          model.Price, model.Description, user.Email, model.UrlImage, validate.Embed(model.UrlVideo)));

                        user.AccountClassifieds = classifiedsList;
                        _writeOnlyRepository.Update(user);
                        
                //check
                MessageBox.Show("Classified added successfully");

            }        

            return View("CreateClassified",model);
        }

        [Authorize]
        public ActionResult MyClassifieds()
        {
            var mcModel = new ClassiModel();
            
            var x = _readOnlyRepository.FirstOrDefault<AccountLogin>(z=>z.Email==HttpContext.User.Identity.Name);
            mcModel.myClassifiedsList= x.AccountClassifieds.ToList();
           
            return View(mcModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult MyClassifieds(ClassiModel model)
        {
            return View(model);
        }

        public static string EmailReceiver="";
        public ActionResult Detailed(long id)
        {
            var classified = _readOnlyRepository.GetById<Classifieds>(id);
            var classiModel = new ClassiModel();
            classiModel.classifiedForDetail = classified;
            EmailReceiver = classiModel.classifiedForDetail.Email;
            var user = _readOnlyRepository.FirstOrDefault<AccountLogin>(x => x.Email == EmailReceiver);
            classiModel.ContactInfo = user.UserInfo;
            classiModel.classifiedForDetail.Visited++;
            _writeOnlyRepository.Update(classiModel.classifiedForDetail);

            return View(classiModel);
        }

        [HttpPost]
        public ActionResult Detailed(ClassiModel model)
        {
                    
                    var user = _readOnlyRepository.FirstOrDefault<AccountLogin>(x => x.Email == EmailReceiver);
                    var messageList = user.AccountMessages.ToList();
                    messageList.Add(new Messages(model.sendEmail.Email, model.sendEmail.Name, model.sendEmail.Message, "Classified|Info"));
                    user.AccountMessages = messageList;
                    _writeOnlyRepository.Update(user);
                    MailTo.SendSimpleMessage(EmailReceiver, model.sendEmail.Name, model.sendEmail.Message);
                  
            //check
            MessageBox.Show("Email sent successfully");
            
            return View(model);
        }

        public ActionResult AllClassifieds(string category="All")
        {
            var allModel = new ClassiModel();
            switch (category)
            {
                case "All": allModel.myClassifiedsList = _readOnlyRepository.GetAll<Classifieds>().ToList();
                    break;
                case "Auto": allModel.myClassifiedsList = _readOnlyRepository.GetAll<Classifieds>().Where(x=>x.Category == "Automoviles").ToList();
                    break;
                case "Inst": allModel.myClassifiedsList = _readOnlyRepository.GetAll<Classifieds>().Where(x => x.Category == "Instruments").ToList();
                    break;
                case "VGC": allModel.myClassifiedsList = _readOnlyRepository.GetAll<Classifieds>().Where(x => x.Category == "VG. Consoles").ToList();
                    break;
                case "Tech": allModel.myClassifiedsList = _readOnlyRepository.GetAll<Classifieds>().Where(x => x.Category == "Technology").ToList();
                    break;
            }
           
            return View(allModel);
        }
        
        public ActionResult AdvancedSearch()
        {
            var cAdvancedSearch = new ClassiModel
            {
                myClassifiedsList = _readOnlyRepository.GetAll<Classifieds>().ToList()
            };

            return View(cAdvancedSearch);
        }

        [HttpPost]
        public ActionResult AdvancedSearch(ClassiModel model)
        {
            var filters = _readOnlyRepository.GetAll<Classifieds>().ToList();

            if (model.Cas.Title != null && model.Cas.TitleBool)
            {
                filters = filters.FindAll(x => x.Article.ToUpper().Contains(model.Cas.Title.ToUpper()));
            }

            if (model.Cas.Category != null && model.Cas.CategoryBool)
            {
                filters = filters.FindAll(x => x.Category.Contains(model.Cas.Category));
            }

            if (model.Cas.Description != null && model.Cas.DescriptionBool)
            {
                filters = filters.FindAll(x => x.Description.ToUpper().Contains(model.Cas.Description.ToUpper()));
            }

            model.myClassifiedsList = filters;

            return View(model);
        }

       [Authorize]
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

            var model = new ThreeListClassifiedsModel
            {
                MostVisitedList = classifiedVisited,
                RecentList = MostRecent(),
                FeaturedList = Featured()
            };

            return View(model);
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

            //For ContactInfo
            public UserData ContactInfo { get; set; }
        }

        public class CategoryAdvancedSearch
        {

            public string Category { get; set; }

            public bool CategoryBool { get; set; }

            [DataType(DataType.Text)]
            public string Title { get; set; }

            public bool TitleBool { get; set; }

            [DataType(DataType.Text)]
            public string Description { get; set; }

            public bool DescriptionBool { get; set; }

        }

        public class SendEmail
        {
            [Required(ErrorMessage = "Name is required")]
            [DataType(DataType.Text)]
            [StringLength(50, ErrorMessage = "First name should be between 3 and 50 characters.", MinimumLength = 3)]
            public string Name { get; set; }

            [Required(ErrorMessage = "Email is required")]
            [DataType(DataType.EmailAddress)]
            [StringLength(100, ErrorMessage = "Email should be between 5 and 100 characters.", MinimumLength = 5)]
            public string Email { get; set; }

            [Required(ErrorMessage = "Description is required")]
            [DataType(DataType.MultilineText)]
            [DescriptionValidation(MinimumAmountOfWords = 3, MaximumAmountOfCharacters = 250,
            ErrorMessage = "The description must contains a minimum of 3 words and a maximum of 250 characters.")]
            public string Message { get; set; }

            [Required(ErrorMessage = "Email is required")]
            [DataType(DataType.EmailAddress)]
            [StringLength(100, ErrorMessage = "Email should be between 5 and 100 characters.", MinimumLength = 5)]
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