using Domain.Entities;

namespace MiPrimerMVC.Models
{
    public class ProfileModel
    {
        public AccountLogin PublicUser { get; set; }
        public bool IsFollowing { get; set; }
    }
}