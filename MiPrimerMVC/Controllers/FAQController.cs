using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using Domain.Entities;
using Domain.Services;
using MiPrimerMVC.ValidationAttributes;

namespace MiPrimerMVC.Controllers
{
    public class FAQController : Controller
    {
        readonly IReadOnlyRepository _readOnlyRepository;
        readonly IWriteOnlyRepository _writeOnlyRepository;
        public FAQController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
        }

        [HttpGet]
        public ActionResult FAQ()
        {
            var questionsList = _readOnlyRepository.GetAll<Questions>().Where(x=>!x.Archived).ToList();
            var questionModel = new QuestionModel();
            questionsList.Reverse();
            questionModel.QuestionList = questionsList;

            return View(questionModel);
        }

        [HttpPost]
        public ActionResult FAQ(QuestionModel model)
        {
          
            var questionToBePushed = new Questions(model.QuestionText,model.Email, model.Name);

             _writeOnlyRepository.Create(questionToBePushed);
        //check
                    MessageBox.Show("Question Added Succesfully");
            
            var questionsList = _readOnlyRepository.GetAll<Questions>().Where(x=>!x.Archived).ToList();
            questionsList.Reverse();
            model.QuestionList = questionsList;

            return View(model);
        }

        [Authorize(Roles = "ADMIN")]
        public ActionResult ManageFaq()
        {
            var questionsList = _readOnlyRepository.GetAll<Questions>().Where(x=>!x.Archived).ToList();
            var questionModel = new QuestionModel();
            questionsList.Reverse();
            questionModel.QuestionList = questionsList;
            return View(questionModel);
        }

        [Authorize(Roles = "ADMIN")]
        public ActionResult AnswerQuestions(long id)
        {
            var questionToBeAnswered = _readOnlyRepository.FirstOrDefault<Questions>(x => x.Id == id && !x.Archived);
            var answermodel = new AnswerModel {FaqQuestion = questionToBeAnswered};

            return View(answermodel);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ActionResult AnswerQuestions(AnswerModel model)
        {
            var questionToBeSaved = _readOnlyRepository.FirstOrDefault<Questions>(x=>x.Id==model.FaqQuestion.Id);
            var list = questionToBeSaved.QuestionAnswers.ToList();
            list.Add(new Answers(model.AnswerText));
            questionToBeSaved.QuestionAnswers = list;
            _writeOnlyRepository.Update(questionToBeSaved);

            return View(model);
        }

        [Authorize(Roles = "ADMIN")]
        public ActionResult ArchiveQuestion(long id)
        {
            var questionToBeArchived = _readOnlyRepository.FirstOrDefault<Questions>(x => x.Id == id);
            questionToBeArchived.Archive();
            _writeOnlyRepository.Update(questionToBeArchived);

            return RedirectToAction("FAQ");
        }

    }


    public class QuestionModel
    {
        public List<Questions> QuestionList { get; set; }

        [Required(ErrorMessage = "Question is required")]
        [DataType(DataType.MultilineText)]
        [DescriptionValidation(MinimumAmountOfWords = 3, MaximumAmountOfCharacters = 255,
            ErrorMessage = "Question must contain a minimum of 3 words and a maximum of 255 characters.")]
        public string QuestionText { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "First name should be between 3 and 50 characters.", MinimumLength = 3)]
        public string Name { get; set; }
    }

    public class AnswerModel
    {
        [Required(ErrorMessage = "Name is required")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "First name should be between 1 and 50 characters.", MinimumLength = 1)]
        public string AnswerText { get; set; }
        public Questions FaqQuestion { get; set; }
    }
}