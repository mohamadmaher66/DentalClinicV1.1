using Microsoft.AspNetCore.Mvc;

namespace DentalClinicAPI.Controllers
{
    [Route("[controller]")]
    public class TestController : Controller
    {
        [HttpGet]
        public string Index()
        {
            return "All working";
        }
    }
}
