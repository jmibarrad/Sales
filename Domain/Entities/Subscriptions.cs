namespace Domain.Entities
{
    public class Subscriptions: IEntity
    {
        public virtual long Id { get; set; }
        public virtual bool Archived { get; protected set; }
        public virtual long Follower { get; set; }
        public virtual long Following { get; set; }

        public Subscriptions(long follower, long following)
        {
            Follower = follower;
            Following = following;
            Archived = false;
        }

        public Subscriptions()
        {
            Archived = false;
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
