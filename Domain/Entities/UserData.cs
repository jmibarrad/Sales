namespace Domain.Entities
{
    public class UserData : IEntity
    {
        public virtual long Id { get; set; }
        public virtual bool Archived { get; set; }
        public virtual string Gender { get; set; }
        public virtual string Country { get; set; }
        public virtual string Blog { get; set; }
        public virtual int AmountOfClassifieds { get; set; }
        public virtual string Cellphone { get; set; }
        public virtual string OfficePhone { get; set; } 
        public virtual string ProfileImg { get; set; }
        public virtual string Description { get; set; }
        public virtual int TotalSold { get; set; }

        public UserData(string gender, string country, string cellphone, string officephone, string profileimg, string description, string blog)
        {
            Archived = false;
            AmountOfClassifieds = 0;
            TotalSold = 0;
            Gender = gender;
            Country = (country ?? "");
            Cellphone = (cellphone ?? "(000) 000-0000");
            OfficePhone = (officephone ?? "(000) 000-0000");
            ProfileImg = (profileimg ?? "http://t0.gstatic.com/images?q=tbn:ANd9GcQCBtudYm6AzY3K3wcsZkpQ-izIyIBx5LvtRqKNxJNgtvEdPeq ");
            Description = (description ?? "No description provided.");
            Blog = (blog ?? "@no-link-available"); ;
        }

        public UserData()
        {
            AmountOfClassifieds = 0;
            TotalSold = 0;
            Country = "";
            Cellphone = "(000) 000-0000";
            OfficePhone = "(000) 000-0000";
            ProfileImg = "http://t0.gstatic.com/images?q=tbn:ANd9GcQCBtudYm6AzY3K3wcsZkpQ-izIyIBx5LvtRqKNxJNgtvEdPeq ";
            Description = "No description provided.";
            Blog = "@no-link-available"; ;
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
