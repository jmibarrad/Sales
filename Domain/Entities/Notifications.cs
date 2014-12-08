namespace Domain.Entities
{
    public class Notifications:IEntity
    {
        public virtual long Id { get; set; }
        public virtual bool Archived { get; protected set; }
        public virtual string Email { get; set; }
        public virtual string Name { get; set; }
        public virtual string Type { get; set; }
        public virtual bool Seen { get; set; }

        public Notifications(string email, string name, string type)
        {
            Seen = false;
            Archived = false;
            Email = email;
            Name = name;
            Type = type;
        }

        public Notifications()
        {
            Archived = false;
        }
        public virtual void Archive()
        {
            Archived = true;
        }

        public virtual void Watch()
        {
            Seen = true;
        }
        public virtual void Activate()
        {
            Archived = false;
        }
    }
}