using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DPS_Seletiene.data;

namespace DPS_Seletiene.Controllers.api
{
    public class citiesController : ApiController
    {
        private seletieneEntities db = new seletieneEntities();

        // GET: api/cities
        public IQueryable<city> Getcity()
        {
            return db.city;
        }

        // GET: api/cities/5
        [ResponseType(typeof(city))]
        public IHttpActionResult Getcity(int id)
        {
            city city = db.city.Find(id);
            if (city == null)
            {
                return NotFound();
            }

            return Ok(city);
        }

        // PUT: api/cities/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putcity(int id, city city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != city.ID)
            {
                return BadRequest();
            }

            db.Entry(city).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!cityExists(id))
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

        // POST: api/cities
        [ResponseType(typeof(city))]
        public IHttpActionResult Postcity(city city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.city.Add(city);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (cityExists(city.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = city.ID }, city);
        }

        // DELETE: api/cities/5
        [ResponseType(typeof(city))]
        public IHttpActionResult Deletecity(int id)
        {
            city city = db.city.Find(id);
            if (city == null)
            {
                return NotFound();
            }

            db.city.Remove(city);
            db.SaveChanges();

            return Ok(city);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool cityExists(int id)
        {
            return db.city.Count(e => e.ID == id) > 0;
        }
    }
}