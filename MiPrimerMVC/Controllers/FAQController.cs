using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using Domain.Entities;
using Domain.Services;

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
            List<Questions> questionsList = _readOnlyRepository.GetAll<Questions>().ToList();
            var questionModel = new QuestionModel();
            questionsList.Reverse();
            questionModel.QuestionList = questionsList;

            return View(questionModel);
        }

        [HttpPost]
        public ActionResult FAQ(QuestionModel model)
        {
            var validate = new IValidate();
            if (!(validate.ValidateTextLength(model.Name,3,50)&&validate.ValidateOnlyLetters(model.Name)))
            {
                MessageBox.Show("Requirements Name: \n1. Between 3 to 50 characters\n2. Only Letters");
            }
            else
            {
                if (!(validate.ValidateTextWords(model.QuestionText,3)&&validate.ValidateTextLength(model.QuestionText,3,250)))
                {
                    MessageBox.Show("Requirements Question: \n1. No more than 250 characters\n2. At least three words");

                }
                else
                {
                    var questionToBePushed = new Questions(model.QuestionText,model.Email, model.Name);

                    var createdOperation = _writeOnlyRepository.Create(questionToBePushed);
                    MessageBox.Show("Question Added Succesfully");
                }
            }
            List<Questions> questionsList = _readOnlyRepository.GetAll<Questions>().ToList();
            questionsList.Reverse();
            model.QuestionList = questionsList;

            return View(model);
        }

    }

    public class QuestionModel
    {
        public List<Questions> QuestionList { get; set; }
        public string QuestionText { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }

    public class AnswerModel
    {
        public string AnswerText { get; set; }
    }
}