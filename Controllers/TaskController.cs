using Microsoft.AspNetCore.Mvc;

namespace ToDomvs.Controllers
{
    public class TaskController : Controller
    {
        public IActionResult Main()
        {
            return View();
        }
    }
}
