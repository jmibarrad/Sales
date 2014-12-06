using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class Questions :IEntity
    {
        public virtual long Id { get; set; }
        public virtual bool Archived { get; set; }
        public virtual string QuestionText { get; set; }
        public virtual IEnumerable<Answers> QuestionAnswers { get; set;}
        public virtual DateTime PostedQuestionDate { get; protected set; }
        public virtual string Email { get; set; }
        public virtual string Name { get; set; }

        public Questions(string questionText, string email, string name)
        {
            QuestionText = questionText;
            Email=email;
            Name = name;
            PostedQuestionDate = DateTime.Today;
        }

        public Questions()
        {
            
        }

        public virtual void Archive()
        {
            Archived = true;
        }

        public virtual void Activate()
        {
            Archived = false;
        }

        public virtual void AddClassified(Answers newAnswer)
        {
            if (QuestionAnswers.All(x => x.Id == newAnswer.Id))
            {
                ((IList<Answers>)QuestionAnswers).Add(newAnswer);
            }
        }

        public virtual void ArchiveClassified(long answerId)
        {
            var answerToBeArchived = QuestionAnswers.FirstOrDefault(pred => pred.Id == answerId);
            if (answerToBeArchived != null)
            {
                answerToBeArchived.Archive();
            }
        }

    }
}
