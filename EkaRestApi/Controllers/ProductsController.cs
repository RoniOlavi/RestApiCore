using Microsoft.AspNetCore.Mvc;
using RestApiCore.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace RestApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // private readonly NorthwindOriginalContext db = new NorthwindOriginalContext();

        // Dependency Injection tyyli!
        private readonly NorthwindOriginalContext db;

        public ProductsController(NorthwindOriginalContext dbparam)
        {
            db = dbparam;
        }

        [HttpGet]
        public ActionResult GetProducts()
        {
            var tuotteet = db.Products;
            try
            {
                if (tuotteet != null)
                {
                    return Ok(tuotteet);
                }
                else
                {
                    return NotFound("Tuotteita ei löytynyt! ");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        [Route("{key}")]
        public ActionResult GetProduct(int key)
        {
            var tuote = db.Products.Find(key);
            try
            {
                if (tuote != null)
                {
                    return Ok(tuote);
                }
                else
                {
                    return NotFound("Tuotetta ei löytynyt annetulla id:lla! ");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("name/{name}")]
        public ActionResult GetProductByName(string name)
        {
            List<Product> tuote = (from c in db.Products
                                       where c.ProductName == name
                                       select c).ToList();
            string message = "Tuotetta ei löytynyt! ";
            try
            {
                if (tuote.Count > 0)
                {
                    return Ok(tuote);
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
        public ActionResult AddProduct([FromBody] Product tuote)
        {
            try
            {
                db.Products.Add(tuote);
                db.SaveChanges();
                return Ok("Lisättiin uusi tuote " + tuote.ProductId + " tietokantaan.");
            }
            catch (Exception error)
            {
                return BadRequest("Lisääminen ei onnistunut! " + error);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult EditProduct(int id, [FromBody] Product tuote)
        {
            try
            {
                var product = db.Products.Find(id);
                if (product != null)
                {
                    product.ProductName = tuote.ProductName;
                    product.QuantityPerUnit = tuote.QuantityPerUnit;
                    product.UnitPrice = tuote.UnitPrice;
                    product.UnitsInStock = tuote.UnitsInStock;
                    product.UnitsOnOrder = tuote.UnitsOnOrder;
                    product.SupplierId = tuote.SupplierId;
                    product.CategoryId = tuote.CategoryId;
                    product.ReorderLevel = tuote.ReorderLevel;
                    product.Discontinued = tuote.Discontinued;

                    db.SaveChanges();
                    return Ok("Id " + product.ProductId + " päivitetty!");
                }
                else
                {
                    return NotFound("Päivitettävää tuotetta ei löytynyt!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteProduct(int id)
        {
            try
            {
                Product tuote = db.Products.Find(id);
                if (tuote != null)
                {
                    db.Products.Remove(tuote);
                    db.SaveChanges();
                    return Ok("Id " + tuote.ProductId + " poistettu!");
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
