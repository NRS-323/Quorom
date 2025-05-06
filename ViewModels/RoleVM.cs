using Quorom.DbTables;

namespace Quorom.ViewModels
{
    public class RoleVM
    {
        public RoleVM()
        {
            RoleList = [];
        }
        public QuoromUser User { get; set; }
        public List<RoleSelection> RoleList { get; set; }
    }

    public class RoleSelection
    {
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }

}
