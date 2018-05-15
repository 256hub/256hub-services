using Hub256.CheckIn.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hub256.CheckIn.Controllers
{
    [Route("[controller]/[action]")]
    public class TestController: Controller
    {

        /// <summary>
        /// Admin cancells poster order
        /// </summary>       
        [HttpGet]        
        public async Task<IActionResult> TestMethod(int id, CancellationToken cancellation = default(CancellationToken))
        {            
            return Ok(new Person
            {
                Age=id,
                FirstName="Gago",
                LastName="Dza"
            });
        }

        public async Task<IActionResult> TestView()
        {
            return View();
        }


    }
}
