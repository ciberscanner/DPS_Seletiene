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
    public class departmentsController : ApiController
    {
        private seletieneEntities db = new seletieneEntities();

        // GET: api/ 
        public IQueryable<department> Getdepartment()
        {
            return db.department;
        }

        // GET: api/departments/5
        [ResponseType(typeof(department))]
        public IHttpActionResult Getdepartment(int id)
        {
            department department = db.department.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            return Ok(department);
        }

        // PUT: api/departments/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putdepartment(int id, department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != department.ID)
            {
                return BadRequest();
            }

            db.Entry(department).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!departmentExists(id))
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

        // POST: api/departments
        [ResponseType(typeof(department))]
        public IHttpActionResult Postdepartment(department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.department.Add(department);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (departmentExists(department.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = department.ID }, department);
        }

        // DELETE: api/departments/5
        [ResponseType(typeof(department))]
        public IHttpActionResult Deletedepartment(int id)
        {
            department department = db.department.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            db.department.Remove(department);
            db.SaveChanges();

            return Ok(department);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool departmentExists(int id)
        {
            return db.department.Count(e => e.ID == id) > 0;
        }
    }
}