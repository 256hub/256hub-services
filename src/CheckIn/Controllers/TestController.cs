using Hub256.CheckIn.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hub256.CheckIn.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class TestController: Controller
    {
        /// <summary>
        /// Admin cancells poster order
        /// </summary>       
        [HttpGet]        
        public async Task<IActionResult> TestMethod(int id, CancellationToken cancellation = default)
        {            
            return Ok(new Person
            {
                Age=id,
                FirstName="Gago",
                LastName="Dza"
            });
        }

        [HttpGet]
        public async Task<IActionResult> TestView()
        {
            return View();
        }


    }

    [ApiController]
    [ApiVersion("1.1")]
    [Route("api/v{version:apiVersion}/Test/[action]")]
    //[Route("[controller]/[action]")]
    public class Test11Controller : Controller
    {
        /// <summary>
        /// Admin cancells poster order
        /// </summary>       
        [HttpGet]
        public async Task<IActionResult> TestMethod(int id=7, CancellationToken cancellation = default)
        {
            return Ok(new Person
            {
                Age = id,
                FirstName = "Gago 1.1",
                LastName = "Dza1.1"
            });
        }

        [HttpGet]
        public async Task<IActionResult> TestView()
        {
            return View();
        }


    }
}
