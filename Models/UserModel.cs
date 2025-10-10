using ToDomvs.Models;

namespace ToDomvs.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<ProjectModel> Projects { get; set; } = new List<ProjectModel>();
        public ICollection<TaskItemModel> AssignedTasks { get; set; } = new List<TaskItemModel>();
    }
}
