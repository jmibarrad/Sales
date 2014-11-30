namespace Domain.Entities
{
    public class UserData : IEntity
    {
        public virtual long Id { get; set; }
        public virtual bool Archived { get; set; }
        public virtual string Gender { get; set; }
        public virtual string Country { get; set; }
        public virtual int AmountOfClassifieds { get; set; }
        public virtual string Cellphone { get; set; }
        public virtual string OfficePhone { get; set; } 
        public virtual string ProfileImg { get; set; }
        public virtual string Description { get; set; }
        public virtual int TotalSold { get; set; }

        public UserData(string gender, string country, string cellphone, string officephone, string profileimg, string description)
        {
            Archived = false;
            AmountOfClassifieds = 0;
            TotalSold = 0;
            Gender = gender;
            Country = country;
            Cellphone = cellphone;
            OfficePhone = officephone;
            ProfileImg = profileimg;
            Description = description;
        }

        public UserData()
        {
            Archived = false;
        }

        //method to change total sold && amount of classifieds
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
