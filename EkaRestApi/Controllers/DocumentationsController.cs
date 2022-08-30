using Microsoft.AspNetCore.Mvc;
using RestApiCore.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace RestApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentationsController : ControllerBase
    {
        // Dependency Injection tyyli!
        private readonly NorthwindOriginalContext db;

        public DocumentationsController(NorthwindOriginalContext dbparam)
        {
            db = dbparam;
        }
        [HttpGet]
        [Route("{key}")]
        
        public ActionResult GetDocument(string key)
        {
             // NorthwindOriginalContext db = new NorthwindOriginalContext();
                List<Documentation> documents = (from dokumentit in db.Documentations
                                                 where dokumentit.Keycode == key
                                                 select dokumentit).ToList();

                if (documents.Count > 0)
                {
                    return Ok(documents);
                }
                else
                {
                    return BadRequest("Dataa ei löytynyt annetulle id:lle! " + DateTime.Now.ToString());
                }
        }
    }
}
