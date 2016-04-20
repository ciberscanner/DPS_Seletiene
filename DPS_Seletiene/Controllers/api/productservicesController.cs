using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity.Validation;
using DPS_Seletiene.data;

namespace DPS_Seletiene.Controllers.api
{
    public class productservicesController : ApiController
    {
        private seletieneEntities db = new seletieneEntities();

        // GET: api/productservices
        public IQueryable<productservice> Getproductservice()

        {
            db.Configuration.LazyLoadingEnabled = false;
            return db.productservice.Where(r => r.userapp.status == 2).Where(r=>r.status==2);
        }
          
        [System.Web.Http.HttpGet]
     
        public IQueryable<productservice> productbyuser(int iduser)
        {
            db.Configuration.LazyLoadingEnabled = false;
            return db.productservice.Where(r => r.idowner == iduser);
        }

        [System.Web.Http.HttpGet]

        [ResponseType(typeof(qualificationps))]
        public async Task<IHttpActionResult> rateProduct(int idpproducto, int value, string comment)
        {

            qualificationps rate = new qualificationps();
            rate.idproduct = idpproducto;
            rate.value = value;
            rate.comment=comment;
            rate.dateup = DateTime.Now;
            db.qualificationps.Add(rate);
            db.SaveChanges();
            return Ok(rate);
        }

          [System.Web.Http.HttpGet]

        [ResponseType(typeof(productservice))]
        public async Task<IHttpActionResult> stateproducto(int idproducto, int state)
        {
            productservice productservice = await db.productservice.FindAsync(idproducto);
            productservice.status = state;
            db.Entry(productservice).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(productservice);
        }
         
        // PUT: api/productservices/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putproductservice(int id, productservice productservice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != productservice.id)
            {
                return BadRequest();
            }

            db.Entry(productservice).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!productserviceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/productservices
        public IHttpActionResult Postproductservice(productservice productservice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            productservice.status = 1;
            try
            {
                db.productservice.Add(productservice);
                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            return CreatedAtRoute("DefaultApi", new { id = productservice.id }, productservice);
        }

        // DELETE: api/productos/5
        [ResponseType(typeof(productservice))]
        public IHttpActionResult Deleteproductservice(int id)
        {
            productservice productservice = db.productservice.Find(id);
            if (productservice == null)
            {
                return NotFound();
            }

            db.productservice.Remove(productservice);
            db.SaveChanges();

            return Ok(productservice);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool productserviceExists(int id)
        {
            return db.productservice.Count(e => e.id == id) > 0;
        }
    }
}