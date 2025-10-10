using ToDomvs.Models;
using System.Collections.Generic; 
using System.ComponentModel.DataAnnotations;

namespace ToDomvs.Models
{
    public class ProjectModel
    {
        [Key] 
        public int ProjectID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int OwnerID { get; set; }
        public UserModel Owner { get; set; }
        public ICollection<TaskItemModel> Tasks { get; set; } = new List<TaskItemModel>();
    }
}
