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
                          model.Price, model.Description, user.Email, model.UrlImage, model.UrlImage1, model.UrlImage2, model.UrlImage3, model.UrlImage4, validate.Embed(model.UrlVideo)));

                        user.AccountClassifieds = classifiedsList;
                        _writeOnlyRepository.Update(user);

                var notifyList = _readOnlyRepository.GetAll<Subscriptions>().Where(x => x.Following == user.Id && !x.Archived);
                
                if(!notifyList.Any())
                foreach (var x in notifyList)
                {
                    var userToBeNotify = _readOnlyRepository.FirstOrDefault<AccountLogin>(z => z.Id == x.Follower);
                    var list = userToBeNotify.Notifications.ToList();
                    list.Add(new Notifications(user.Email,model.Article,"Classi"));
                    userToBeNotify.Notifications = list;
                    _writeOnlyRepository.Update(userToBeNotify);
                }

                //check
                MessageBox.Show("Classified added successfully");

            }        

            return View("CreateClassified",model);
        }

        [Authorize]
        public ActionResult MyClassifieds(long id=0)
        {
            var mcModel = new ClassiModel();
            if (id != 0)
            {
                mcModel.classifiedForDetail = _readOnlyRepository.FirstOrDefault<Classifieds>(z => z.Id == id);

            }
          
            var x = _readOnlyRepository.FirstOrDefault<AccountLogin>(z=>z.Email==HttpContext.User.Identity.Name && !z.Archived);
            mcModel.myClassifiedsList= x.AccountClassifieds.Where(z=>!z.AdminArchived && !z.Archived).ToList();
            mcModel.myClassifiedsListArchived = x.AccountClassifieds.Where(z => !z.AdminArchived && z.Archived).ToList();
           
            return View(mcModel);
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

        [Authorize]
        public ActionResult ManageClassifieds(long id)
        {
            var mClassified = new ManageClassifiedsModel();
            mClassified.ClassifiedToBeEdited = _readOnlyRepository.FirstOrDefault<Classifieds>(x => x.Id == id);


            return View(mClassified);
        }

        [HttpPost]
        [Authorize]
        public ActionResult ManageClassifieds(ManageClassifiedsModel model)
        {
            
            if (ModelState.IsValid)
            {
                if (model.ClassifiedToBeEdited.Article != model.Article || model.Article.Trim() == "")
                    model.ClassifiedToBeEdited.Article = model.Article;

                if (model.ClassifiedToBeEdited.ArticleModel != model.ArticleModel)
                    model.ClassifiedToBeEdited.ArticleModel = model.ArticleModel;

                if (model.ClassifiedToBeEdited.Description != model.Description || model.Description.Trim() == "")
                    model.ClassifiedToBeEdited.Description = model.Description;

                if (model.ClassifiedToBeEdited.Price != model.Price)
                    model.ClassifiedToBeEdited.Price = model.Price;

                model.ClassifiedToBeEdited.UrlImage = model.UrlImage;
                model.ClassifiedToBeEdited.UrlImage1 = model.UrlImage1;
                model.ClassifiedToBeEdited.UrlImage2 = model.UrlImage2;
                model.ClassifiedToBeEdited.UrlImage3 = model.UrlImage3;
                model.ClassifiedToBeEdited.UrlImage4 = model.UrlImage4;

                if (model.ClassifiedToBeEdited.Location != model.Location || model.Location.Trim()!="")
                    model.ClassifiedToBeEdited.Location = model.Location;

                if (model.ClassifiedToBeEdited.Category != model.Category)
                    model.ClassifiedToBeEdited.Category = model.Category;

                if (model.ClassifiedToBeEdited.UrlVideo != model.UrlVideo || model.UrlVideo.Trim() == "")
                    model.ClassifiedToBeEdited.UrlVideo = model.UrlVideo;

                _writeOnlyRepository.Update(model.ClassifiedToBeEdited);

            } ModelState.AddModelError("", "Something went wrong.");

            return RedirectToAction("MyClassifieds");
        }

        [Authorize]
        public ActionResult Archive(long id)
        {
            var classifiedToBeArchived = _readOnlyRepository.FirstOrDefault<Classifieds>(x => x.Id == id);
            classifiedToBeArchived.Archive();
            _writeOnlyRepository.Update(classifiedToBeArchived);

            return RedirectToAction("MyClassifieds");
        }

        [Authorize]
        public ActionResult Activate(long id)
        {
            var classifiedToBeActivated = _readOnlyRepository.FirstOrDefault<Classifieds>(x => x.Id == id);
            classifiedToBeActivated.Activate();
            _writeOnlyRepository.Update(classifiedToBeActivated);

            return RedirectToAction("MyClassifieds");
        }

        public ActionResult Report(long id)
        {
            var classifiedToBeReported = _readOnlyRepository.FirstOrDefault<Classifieds>(x => x.Id==id);
            var adminUser = _readOnlyRepository.FirstOrDefault<AccountLogin>(x => x.Id == 1);
            var list = adminUser.Notifications.ToList();
            list.Add(new Notifications(classifiedToBeReported.Email, classifiedToBeReported.Article, "Report"));
            adminUser.Notifications = list;
            _writeOnlyRepository.Update(adminUser); 


            return RedirectToAction("Detailed", new{id});
        }
        public ActionResult AllClassifieds(string category="All")
        {
            var allModel = new ClassiModel();
            switch (category)
            {
                case "All": allModel.myClassifiedsList = _readOnlyRepository.GetAll<Classifieds>().Where(x=>!x.Archived && !x.AdminArchived).ToList();
                    break;
                case "Auto": allModel.myClassifiedsList = _readOnlyRepository.GetAll<Classifieds>().Where(x => x.Category == "Automoviles" && !x.Archived && !x.AdminArchived).ToList();
                    break;
                case "Inst": allModel.myClassifiedsList = _readOnlyRepository.GetAll<Classifieds>().Where(x => x.Category == "Instruments" && !x.Archived && !x.AdminArchived).ToList();
                    break;
                case "VGC": allModel.myClassifiedsList = _readOnlyRepository.GetAll<Classifieds>().Where(x => x.Category == "VG. Consoles" && !x.Archived && !x.AdminArchived).ToList();
                    break;
                case "Tech": allModel.myClassifiedsList = _readOnlyRepository.GetAll<Classifieds>().Where(x => x.Category == "Technology" && !x.Archived && !x.AdminArchived).ToList();
                    break;
            }
           
            return View(allModel);
        }
        
        public ActionResult AdvancedSearch()
        {
            var cAdvancedSearch = new ClassiModel
            {
                myClassifiedsList = _readOnlyRepository.GetAll<Classifieds>().Where(x=>!x.Archived && !x.AdminArchived).ToList()
            };

            return View(cAdvancedSearch);
        }

        [HttpPost]
        public ActionResult AdvancedSearch(ClassiModel model)
        {
            var filters = new List<Classifieds>();

            if (model.Cas.Title != null && model.Cas.TitleBool)
            {
                for (var i = 0; i < filters.Count; i++)
                {
                    filters.Add(filters.FirstOrDefault(x => x.Article.ToUpper().Contains(model.Cas.Title.ToUpper()) && !x.Archived && !x.AdminArchived));
                }
            }

            if (model.Cas.Category != null && model.Cas.CategoryBool)
            {
                for (var i = 0; i < filters.Count; i++)
                {
                    filters.Add(filters.FirstOrDefault(x => x.Category.ToUpper().Contains(model.Cas.Category.ToUpper()) && !x.Archived && !x.AdminArchived));
                }
            }

            if (model.Cas.Description != null && model.Cas.DescriptionBool)
            {
                for (var i = 0; i < filters.Count; i++)
                {
                    filters.Add(filters.FirstOrDefault(x => x.Description.ToUpper().Contains(model.Cas.Description.ToUpper()) && !x.Archived && !x.AdminArchived));
                }
            }

            model.myClassifiedsList = filters;

            return View(model);
        }

        public ActionResult MostVisited()
        {
            var classifiedVisited = _readOnlyRepository.GetAll<Classifieds>().ToList();
            classifiedVisited = classifiedVisited.FindAll(x => x.Visited > 0 && !x.AdminArchived && !x.Archived);

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
            var l = _readOnlyRepository.GetAll<Classifieds>().Where(x => !x.Archived && !x.AdminArchived).ToList();
            l.Reverse();

            if (l.Count > 5)
                l.RemoveRange(5, l.Count - 5);

         

            return l;
        }

        public List<Classifieds> Featured()
        {
            var alles = _readOnlyRepository.GetAll<Classifieds>().Where(x => !x.Archived && !x.AdminArchived).ToList();
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
            public List<Classifieds> myClassifiedsListArchived { get; set; }

            public CategoryAdvancedSearch Cas { get; set; }

            //For ContactInfo
            public UserData ContactInfo { get; set; }

            //For Edit
            public string Category { get; set; }

            [Required(ErrorMessage = "Article´s Name is required")]
            [DataType(DataType.Text)]
            [DescriptionValidation(MinimumAmountOfWords = 1, MaximumAmountOfCharacters = 100,
            ErrorMessage = "The description must contains a minimum of 1 word and a maximum of 100 characters.")]
            public string Article { get; set; }
            public string ArticleModel { get; set; }

            [Range(0.01, 10000.00, ErrorMessage = "Price must be between 0.01 and 10000.00")]
            [Required(ErrorMessage = "Price is Required")]
            [DataType(DataType.Currency)]
            public float Price { get; set; }

            [Required(ErrorMessage = "Location is required")]
            [DataType(DataType.Text)]
            public string Location { get; set; }

            [Required(ErrorMessage = "Description is required")]
            [DataType(DataType.MultilineText)]
            [DescriptionValidation(MinimumAmountOfWords = 3, MaximumAmountOfCharacters = 255,
            ErrorMessage = "The description must contains a minimum of 3 words and a maximum of 255 characters.")]
            public string Description { get; set; }

            [DataType(DataType.ImageUrl, ErrorMessage = "Url for image is not valid")]
            public string UrlImage { get; set; }
            
            public string UrlVideo { get; set; }

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