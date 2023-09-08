//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

 

/*namespace WebRestApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineStockController : ControllerBase
    {
    }
}*/


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebRestApp.Models;
using WebRestApp.Models.Db;
using static WebRestApp.Models.SecurityService;
//using WebRestApp.Models.Db;

namespace WebRestApp.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class MedicineStockController : ControllerBase
    {
        public MedicineStockService service;
        public MedicineStockController(MedicineStockService svc)
        {
            service = svc;
        }

       // [AllowAnonymous]
        [HttpGet]
        [Route("MedicineInventories")]

        public IActionResult GetAllMeds()
        {
            var list = service.GetAllMeds();
            return Ok(list);
        }
        [Authorize]
        [HttpGet]
        [Route("MedicineInventory/{id}")]
        public IActionResult GetMed(int id)
        {
            MedicineInventory md = service.GetMed(id);
            if (md == null)
                return NotFound();
            else
                return Ok(md);
        }
        [HttpPost]
        [Route("add")]
        public IActionResult PostMed(MedicineInventory medicine)
        {
            int result = service.AddMed(medicine);
            if (result == 1) return Ok();
            else
                return new StatusCodeResult(501); //HttpStatusCode
        }
        //----------use route to pass data-------------
        [HttpPost]
        [Route("addnew/{MedicineName}/{CategoryId}/{Stocklevel}/{Price}")]
        public IActionResult PostMed2([FromRoute] MedicineInventory medicine)
        {
            int result = service.AddMed(medicine);
            if (result == 1) return Ok();
            else
                return new StatusCodeResult(501); //HttpStatusCode
        }
        //--------------use query string---------
        [HttpPost]
        [Route("addquery")]
        public IActionResult PostMed3([FromQuery] MedicineInventory medicine)
        {
            int result = service.AddMed(medicine);
            if (result == 1) return Ok();
            else
                return new StatusCodeResult(501); //HttpStatusCode
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeleteMed(int id)
        {
            bool result = service.DeleteMed(id);
            if (result == true)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        [Route("update")]

        public IActionResult UpdateMed(MedicineInventory md)
        {
            bool result = service.UpdateMed(md);
            if (result == true)
            {
                return Ok();
            }
            else
                return NotFound();
        }



        //[Authorize]
        [HttpGet]
        [Route("MedicinesExpiringNextMonth")]
        public ObjectResult GetMedicinesExpiringNextMonth()
        {
            var expiringMeds = service.GetMedicinesExpiringNextMonth();
            if (expiringMeds == null)
            {
                return NotFound("No medicines are expiring next month.");
            }
            return Ok(expiringMeds);
        }
        /*public ObjectResult GetAllMeds()
        {
            var list = service.GetAllMeds();
            return Ok(list);
        }*/

        //[Authorize]
        [HttpGet]
        [Route("MedicinesReachingCriticalStock")]
        public IActionResult GetMedicinesReachingCriticalStock()
        {
            /* Console.Write("Enter the critical stock level: ");
             if (!int.TryParse(Console.ReadLine(), out int criticalStockLevel))
             {
                 Console.WriteLine("Invalid input for critical stock level");
                 return;
             }*/
            int criticalStockLevel = 10; // Define your critical stock level here
            var criticalMeds = service.GetMedicinesReachingCriticalStock(criticalStockLevel);
            if (criticalMeds == null || criticalMeds.Count == 0)
            {
                return NotFound("No medicines are reaching critical stock levels.");
            }
            return Ok(criticalMeds);
        }
        [HttpGet]
        [Route("MedicineInventories/ByCategory/{categoryId}")]
        public IActionResult GetMedicinesByCategoryId(int categoryId)
        {
            var medicines = service.GetMedicinesByCategoryId(categoryId);
            if (medicines.Count == 0)
            {
                return NotFound();
            }
            return Ok(medicines);
        }
        [HttpGet]
        [Route("search")]
        public IActionResult SearchMedicines([FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("Search term is required.");
            }

            List<MedicineInventory> searchResults = service.SearchMeds(searchTerm);

            if (searchResults.Count == 0)
            {
                return NotFound("No medicines found matching the search term.");
            }

            return Ok(searchResults);
        }
    }
}