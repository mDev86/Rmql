using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DemoApp.Workers;
using Rmql;

namespace DemoApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly RmqlClient _rmqlClient;
        
        public HomeController(RmqlClient rmqlClient)
        {
            _rmqlClient = rmqlClient;
        }

        public async Task<IActionResult> Index()
        {
            var model = new MqMessage
            {
                Id = 1,
                Message = "Message text"
            };
            await _rmqlClient.PushAsync<TestWorker, MqMessage>(model);
            
            return View();
        }
    }
}