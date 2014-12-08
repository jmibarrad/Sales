using System;

namespace Domain.Entities
{
    public class Answers :IEntity 
    {
        public virtual long Id { get; set; }
        public virtual bool Archived { get; set; }
        public virtual string AnswerText { get; set; }
        public virtual DateTime PostedAnswerDate { get; protected set; }

        public Answers(string answertext)
        {
            AnswerText = answertext;
            PostedAnswerDate = DateTime.Today;
        }

        public Answers()
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
    }
}
