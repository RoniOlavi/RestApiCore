using Microsoft.AspNetCore.Mvc;
using RestApiCore.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace RestApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        // private readonly NorthwindOriginalContext db = new NorthwindOriginalContext();

        // Dependency Injection tyyli!
        private readonly NorthwindOriginalContext db;

        public EmployeesController(NorthwindOriginalContext dbparam)
        {
            db = dbparam;
        }

        [HttpGet]
        public ActionResult GetEmployees()
        {
            var employees = db.Employees;
            try
            {
                if (employees != null)
                {
                    return Ok(employees);
                }
                else
                {
                    return NotFound("Dataa ei löytynyt! ");
                }
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        [Route("{key}")]
        public ActionResult GetEmployee(int key)
        {
            var tekija = db.Employees.Find(key);
            try
            {
                if (tekija != null)
                {
                    return Ok(tekija);
                }
                else
                {
                    return NotFound("Dataa ei löytynyt annetulla id:lla! ");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("city/{city}")]
        public ActionResult GetCity(string city)
        {
            List<Employee> kaupunki = (from c in db.Employees
                                       where c.City == city
                                       select c).ToList();
            string message = "Kaupunkia ei löytynyt! ";
            try
            {
                if (kaupunki.Count > 0)
                {
                    return Ok(kaupunki);
                }
                else
                {
                    return BadRequest(message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("")]
        public ActionResult AddEmployee([FromBody] Employee employee)
        {
            try
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return Ok("Lisättiin uusi työntekijä " + employee.LastName + " tietokantaan.");
            }
            catch (Exception)
            {
                return BadRequest("Lisääminen ei onnistunut! ");
            }
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult EditEmployee(int id, [FromBody] Employee tekija)
        {
            try
            {
                Employee emp = db.Employees.Find(id);
                if (emp != null)
                {
                    emp.FirstName = tekija.FirstName;
                    emp.LastName = tekija.LastName;
                    emp.Address = tekija.Address;
                    emp.BirthDate = tekija.BirthDate;
                    emp.HireDate = tekija.HireDate;
                    emp.HomePhone = tekija.HomePhone;
                    emp.PostalCode = tekija.PostalCode;
                    emp.City = tekija.City;
                    emp.Country = tekija.Country;
                    emp.Title = tekija.Title;
                    emp.Region = tekija.Region;
                    emp.TitleOfCourtesy = tekija.TitleOfCourtesy;
                    emp.Extension = tekija.Extension;
                    emp.Notes = tekija.Notes;

                    db.SaveChanges();
                    return Ok("Id " + emp.EmployeeId + " Päivitetty!");
                }
                else
                {
                    return NotFound("Päivitettävää henkiöä ei löytynyt!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteEmployee(int id)
        {
            try
            {
                Employee emp = db.Employees.Find(id);
                if (emp != null)
                {
                    db.Employees.Remove(emp);
                    db.SaveChanges();
                    return Ok("Id " + emp.EmployeeId + " Poistettu!");
                }
                else
                {
                    return NotFound("Id ei löytynyt!");
                }
            }
            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen!");
            }
        }
    }
}
