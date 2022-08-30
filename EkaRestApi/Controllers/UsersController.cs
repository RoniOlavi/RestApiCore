using Microsoft.AspNetCore.Mvc;
using RestApiCore.Models;
using System;

namespace RestApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // private readonly NorthwindOriginalContext db = new NorthwindOriginalContext();

        // Dependency Injection tyyli!
        private readonly NorthwindOriginalContext db;

        public UsersController(NorthwindOriginalContext dbparam)
        {
            db = dbparam;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var kayttajat = db.Users;
            foreach (var user in kayttajat) 
            {
                user.Password = null; // Salasanan näyttäminen "Nullataan" palautusta varten! Tässä EI db.savechanges, jotta salasana ei nollaannu tietokantaan asti!
            }
            return Ok(kayttajat);
        }

        // Uuden lisäys
        [HttpPost]
        public ActionResult PostCreateNew([FromBody] User kayt)
        {
            try
            {
                db.Users.Add(kayt);
                db.SaveChanges();
                return Ok("Lisättiin käyttäjä: " + kayt.UserName);
            }
            catch (Exception e)
            {
                return BadRequest("Lisääminen ei onnistunut! Lisätietoja: " + e);
            }
        }

        // Poisto
        [HttpDelete]
        [Route("{id}")]
        public ActionResult Poista(int id) // ID poikkeuksellisesti string, tietokannasta johtuen!!!
        {
            var asiakkaat = db.Users.Find(id);
            if (asiakkaat == null)
            {
                return NotFound("Käyttäjää id:lla" + id + "ei löytynyt");
            }
            else
            {
                try
                {
                    db.Users.Remove(asiakkaat);
                    db.SaveChanges();

                    return Ok("Poistettiin käyttäjä " + asiakkaat.LastName);
                }
                catch (Exception ex)
                {
                    return BadRequest("Tapahtui virhe. Tässä tietoa: " + ex);
                }
            }
        }
        // Muokkaus
        [HttpPut]
        [Route("{id}")]
        public ActionResult EditUser(int id, [FromBody] User kayttaja)
        {
            try
            {
                User kt = db.Users.Find(id);
                if (kt != null)
                {
                    kt.FirstName = kayttaja.FirstName;
                    kt.LastName = kayttaja.LastName;
                    kt.Email = kayttaja.Email;
                    kt.AccesslevelId = kayttaja.AccesslevelId;
                    kt.UserName = kayttaja.UserName;
                    kt.Password = kayttaja.Password;

                    db.SaveChanges();
                    return Ok("Id " + kt.UserId + " Päivitetty!");
                }
                else
                {
                    return NotFound("Päivitettävää käyttäjää ei löytynyt!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
