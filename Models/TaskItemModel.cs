using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using ToDomvs.Models;

namespace ToDomvs.Models
{
    public class TaskItemModel
    {
        public int TaskID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public DateTime DueDate { get; set; }
        public int ProjectID { get; set; }
        public ProjectModel Project { get; set; }
        public int? AssignedUserID { get; set; }
        public UserModel AssignedUser { get; set; }
    }
}
