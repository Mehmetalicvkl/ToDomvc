using Microsoft.AspNetCore.Mvc;

namespace ToDoMvc.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View(); // bu, Views/User/Index.cshtml dosyasını arar
        }
    }
}
