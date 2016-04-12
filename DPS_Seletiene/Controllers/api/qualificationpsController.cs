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
    public class qualificationpsController : ApiController
    {
        private seletieneEntities db = new seletieneEntities();

        // GET: api/qualificationps
        public IQueryable<qualificationps> Getqualificationps()
        {
            return db.qualificationps;
        }

           
        [System.Web.Http.HttpGet]
        public IQueryable<qualificationps> qualificationpsbyproduct(int idproducts)
        {
            return db.qualificationps.Where(r => r.idproduct == idproducts);
        }

        // GET: api/qualificationps/5
        [ResponseType(typeof(qualificationps))]
        public IHttpActionResult Getqualificationps(int id)
        {
            qualificationps qualificationps = db.qualificationps.Find(id);
            if (qualificationps == null)
            {
                return NotFound();
            }

            return Ok(qualificationps);
        }

        // PUT: api/qualificationps/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putqualificationps(int id, qualificationps qualificationps)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != qualificationps.id)
            {
                return BadRequest();
            }

            db.Entry(qualificationps).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!qualificationpsExists(id))
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

        // POST: api/qualificationps
        [ResponseType(typeof(qualificationps))]
        public IHttpActionResult Postqualificationps(qualificationps qualificationps)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.qualificationps.Add(qualificationps);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = qualificationps.id }, qualificationps);
        }

        // DELETE: api/qualificationps/5
        [ResponseType(typeof(qualificationps))]
        public IHttpActionResult Deletequalificationps(int id)
        {
            qualificationps qualificationps = db.qualificationps.Find(id);
            if (qualificationps == null)
            {
                return NotFound();
            }

            db.qualificationps.Remove(qualificationps);
            db.SaveChanges();

            return Ok(qualificationps);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool qualificationpsExists(int id)
        {
            return db.qualificationps.Count(e => e.id == id) > 0;
        }
    }
}