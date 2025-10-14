using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDomvs.Data;
using ToDomvs.Models;
using System.Linq;
using System.Threading.Tasks;


namespace ToDomvs.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TaskApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        private TaskApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet] // api/tasks
        public async Task<IActionResult> GetAllTasks()
        {
            var task = await _context.TaskItems
                .Include(t => t.Project)
                .Include(t => t.AssignedUser)
                .ToListAsync();

            return Ok(task);
        }

        [HttpGet("{id}")] //api/tasks/id
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _context.TaskItems
                .Include(t => t.Project)
                .Include(t => t.AssignedUser)
                .FirstOrDefaultAsync(t => t.TaskID == id);

            if (task == null)
                return NotFound(new { succes = false, message = "task not found" });

            return Ok(task);
        }

        [HttpGet("project/{projectId}")] // api/tasks/project/id
        public async Task<IActionResult> GetTaskByProject(int projectId)
        {
            var tasks = await _context.TaskItems
                 .Where(t => t.ProjectID == projectId)
                .Include(t => t.Project)
                .Include(t => t.AssignedUser)
                .ToListAsync();

            return Ok(tasks);
        }
        [HttpGet("upcoming")] //api/tasks/upcoming
        public async Task<IActionResult> GetUpcomingTasks([FromQuery] int days = 3)
        {
            var now = System.DateTime.Now;
            var tasks = await _context.TaskItems
                .Where(t => t.DueDate >= now && t.DueDate <= now.AddDays(days))
                .Include(t => t.AssignedUser)
                .ToListAsync();

            return Ok(tasks);
        }

        [HttpGet("in progress")] // api/tasks/inprogress
        public async Task<IActionResult> GetInProgressTasks()
        {
            var tasks = await _context.TaskItems
                .Where(t => t.Status == Models.TaskStatus.InProgress)
                .Include(TaskStatus => TaskStatus.AssignedUser)
                .ToListAsync();

            return Ok(tasks);
        }

        [HttpPost] // api/tasks
        public async Task<IActionResult> CreateTask([FromBody] TaskItemModel task)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.TaskItems.Add(task);
            await _context.SaveChangesAsync();

            return Ok(new { succes = true, message = "task created successfully" });
        }

        [HttpPut("{id}")] //api/tasks/id 
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskItemModel updatedTask)
        {
            if (id != updatedTask.TaskID)
                return BadRequest(new { success = false, message = "task id mismatch." });

            var existingTask = await _context.TaskItems.FindAsync(id);
            if (existingTask == null)
                return NotFound(new { success = false, message = "task not found." });

            existingTask.Title = updatedTask.Title;
            existingTask.Description = updatedTask.Description;
            existingTask.Status = updatedTask.Status;
            existingTask.DueDate = updatedTask.DueDate;
            existingTask.ProjectID = updatedTask.ProjectID;
            existingTask.AssignedUserID = updatedTask.AssignedUserID;

            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "task updated successfully." });
        }

        [HttpDelete("{id}")] // api/tasks/id
        public async Task<IActionResult> DeleteTask(int id)
        {
            var existingtask = await _context.TaskItems.FindAsync(id);
            if (existingtask == null)
                return NotFound(new { success = false, message = "task id does not exist" });

            _context.TaskItems.Remove(existingtask);
            await _context.SaveChangesAsync();

            return Ok(new { succes = true, message = "task deleted succesfully" });
        }
    }
}