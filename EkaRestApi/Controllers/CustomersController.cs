using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiCore.Models;
using System;
using System.Linq;

namespace RestApiCore.Controllers
{
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        // private readonly NorthwindOriginalContext db = new NorthwindOriginalContext();

        // Dependency Injection tyyli!
        private readonly NorthwindOriginalContext db;

        public CustomersController(NorthwindOriginalContext dbparam)
        {
            db = dbparam;
        }

        // Hakee kaikki
        [HttpGet]
        public ActionResult GetAll()
        {
            var customers = db.Customers;
            return Ok(customers);
        }
        // ID-Haku
        [HttpGet]
        [Route("{id}")]
        public ActionResult GetOneCustomer(string id)
        {
            try 
            {
                var asiakas = db.Customers.Find(id);

                if (asiakas == null) 
                {
                    return NotFound("Asiakasta id:lla" + " " + id + " " + "ei löytynyt.");
                }
                else
                {
                    return Ok(asiakas);
                }
                
            }
            catch (Exception poikkeus)
            {
                return BadRequest(poikkeus.Message);
            }
        }
        // Poisto
        [HttpDelete]
        [Route("{id}")]
        public ActionResult Poista(string id) // ID poikkeuksellisesti string, tietokannasta johtuen!!!
        {
            var asiakkaat = db.Customers.Find(id);
            if (asiakkaat == null)
            {
                return NotFound("Asiakasta id:lla" + id + "ei löytynyt");
            }
            else
            {
                try
                {
                    db.Customers.Remove(asiakkaat);
                    db.SaveChanges();

                    return Ok("Poistettiin asiakas " + asiakkaat.CompanyName);
                }
                catch (Exception ex)
                {
                    return BadRequest("Tapahtui virhe. Tässä tietoa: " + ex);
                }
            }
        }

        [HttpPost]
        // Lisäys
        public ActionResult PostCreateNew([FromBody] Customer asiakas)
        {
            try
            {
                db.Customers.Add(asiakas);
                db.SaveChanges();
                return Ok("Lisättiin asiakas " + asiakas.CompanyName);
            }
            catch (Exception e)
            {
                return BadRequest("Asiakkaan lisääminen ei onnistunut! Tässä lisätietoja: " + e);
            }
        }

        //Muokkaus
        [HttpPut]
        [Route("{id}")]
        public ActionResult PutEdit(string id, [FromBody] Customer asiakas)
        {

            try
            {
                Customer customer = db.Customers.Find(id);
                if (customer != null)
                {
                    customer.CompanyName = asiakas.CompanyName;
                    customer.ContactName = asiakas.ContactName;
                    customer.ContactTitle = asiakas.ContactTitle;
                    customer.Country = asiakas.Country;
                    customer.Address = asiakas.Address;
                    customer.City = asiakas.City;
                    customer.PostalCode = asiakas.PostalCode;
                    customer.Phone = asiakas.Phone;
                    customer.Fax = asiakas.Fax;

                    db.SaveChanges();
                    return Ok(customer.CustomerId);
                }
                else
                {
                    return NotFound("Päivitettävää asiakasta ei löytynyt!");
                }
            }
            catch (Exception e)
            {
                return BadRequest("Jokin meni pieleen asiakasta päivitettäessä. Alla lisätietoa" + e);
            }

        }

        // Get Customers by country parameter localhost:xxxxxx/api/customers/country/finland
        [HttpGet]
        [Route("country/{maa}")]
        public ActionResult GetSomeCustomers(string maa)
        {
            /*var cust = (from c in db.Customers
                                where c.Country == maa
                                select c).ToList();
            */

            // Sama kuin yllä, mutta lambda tyylillä:
            //var cust = db.Customers.Where(c => c.Country.ToLower() == maa.ToLower());

            // Tässä riittää että tiedetään maan nimen osa
            var cust = db.Customers.Where(c => c.Country.ToLower().Contains(maa.ToLower()));


            return Ok(cust);
        }
    }
}
