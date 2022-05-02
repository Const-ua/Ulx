using Microsoft.AspNetCore.Identity;

namespace Ulx.Models
{
    public class AssignRolesVM
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public List<IdentityRole> AllRoles { get; set; }
        public IList<string> UserRoles { get; set; }

        public AssignRolesVM()
        {
            AllRoles = new List<IdentityRole>();
            UserRoles = new List<string>();
        }
    }
}
