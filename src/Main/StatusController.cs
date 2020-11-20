namespace Main
{
    using System;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class StatusController : ControllerBase
    {
        public StatusController(IHostingEnvironment environment)
        {
            Console.WriteLine(environment.ContentRootPath);
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "Hello!";
        }
    }
}