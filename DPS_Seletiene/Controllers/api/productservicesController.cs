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
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace DPS_Seletiene.Controllers.api
{
    public class productservicesController : ApiController
    {
        private seletieneEntities db = new seletieneEntities();

        // GET: api/productservices
        public IQueryable<ViewProductServices> Getproductservice()

        {
            db.Configuration.LazyLoadingEnabled = false;
           
            return db.ViewProductServices.Where(r => r.statususer == 2).Where(r=>r.status==2);
        }
          
        [System.Web.Http.HttpGet]
     
        public IQueryable<productservice> productbyuser(int iduser)
        {
            db.Configuration.LazyLoadingEnabled = false;
            return db.productservice.Where(r => r.idowner == iduser);
        }


        [System.Web.Http.HttpGet]

        public IQueryable<productservice> productsbycategory(int idcategory)
        {
            db.Configuration.LazyLoadingEnabled = false;
            return db.productservice.Where(r => r.idcategory == idcategory);
        }

        [System.Web.Http.HttpGet]

        public double productqualitity(int idproduct)
        {
            db.Configuration.LazyLoadingEnabled = false;
            System.Nullable<Double> averageFreight =
    (from ord in db.qualificationps.Where(r=>r.idproduct==idproduct)
     select ord.value )
    .Average();
    
            return (double)averageFreight;
            

     
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
            db.Configuration.LazyLoadingEnabled = false;
          
            productservice product = db.productservice.Find(idpproducto);
            product.calification = productqualitity(idpproducto);
            db.Entry(product).State = EntityState.Modified;
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
             [HttpPut]

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
                var myString = productservice.image.Split(new char[] { ',' });
                byte[] bytes = Convert.FromBase64String(myString[1]); using (MemoryStream ms = new MemoryStream(bytes))
                {
                    try{
                  //      Image image = Image.FromStream(ms);
                        string path1 = string.Format("{0}/{1}{2}", System.Web.HttpContext.Current.Server.MapPath("~/Images/"), "image_", productservice.name+".jpg");

//                        image.Save("./Image.jpg", ImageFormat.Jpeg);
                        File.WriteAllBytes(path1, bytes);
                    }
                    catch(Exception ce)
                    {
                        Console.WriteLine("");
                    }
                }
                
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