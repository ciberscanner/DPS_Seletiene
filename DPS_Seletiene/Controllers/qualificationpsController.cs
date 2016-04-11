using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DPS_Seletiene.data;

namespace DPS_Seletiene.Controllers
{
    public class qualificationpsController : Controller
    {
        private seletieneEntities db = new seletieneEntities();

        // GET: qualificationps
        [Authorize(Roles = "Administrador, Callcenter")]
        public ActionResult Index(string searchString)
        {
            var qualificationps = db.qualificationps.Include(q => q.productservice);

            if (searchString != null)
            {
                qualificationps = qualificationps.Where(s => (s.value.ToString()).Contains(searchString)
                                       || s.productservice.name.Contains(searchString)
                                       || s.productservice.userapp.name1.Contains(searchString)
                                       || s.productservice.userapp.city1.department.Name.Contains(searchString)
                                       || s.productservice.userapp.city1.Name.Contains(searchString));
            }

            qualificationps = qualificationps.OrderByDescending(s => s.dateup);

            return View(qualificationps.ToList());
        }

        // GET: qualificationps/Details/5
        [Authorize(Roles = "Administrador, Callcenter")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            qualificationps qualificationps = db.qualificationps.Find(id);
            if (qualificationps == null)
            {
                return HttpNotFound();
            }
            return View(qualificationps);
        }

        // GET: qualificationps/Create
        [Authorize(Roles = "Administrador, Callcenter")]
        public ActionResult Create()
        {
            ViewBag.idproduct = new SelectList(db.productservice, "id", "name");
            return View();
        }

        // POST: qualificationps/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,idproduct,comment,value,dateup")] qualificationps qualificationps)
        {
            if (ModelState.IsValid)
            {
                db.qualificationps.Add(qualificationps);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idproduct = new SelectList(db.productservice, "id", "name", qualificationps.idproduct);
            return View(qualificationps);
        }

        // GET: qualificationps/Edit/5
        [Authorize(Roles = "Administrador, Callcenter")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            qualificationps qualificationps = db.qualificationps.Find(id);
            if (qualificationps == null)
            {
                return HttpNotFound();
            }
            ViewBag.idproduct = new SelectList(db.productservice, "id", "name", qualificationps.idproduct);
            return View(qualificationps);
        }

        // POST: qualificationps/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idproduct,comment,value,dateup")] qualificationps qualificationps)
        {
            if (ModelState.IsValid)
            {
                db.Entry(qualificationps).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idproduct = new SelectList(db.productservice, "id", "name", qualificationps.idproduct);
            return View(qualificationps);
        }

        // GET: qualificationps/Delete/5
        [Authorize(Roles = "Administrador, Callcenter")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            qualificationps qualificationps = db.qualificationps.Find(id);
            if (qualificationps == null)
            {
                return HttpNotFound();
            }
            return View(qualificationps);
        }

        // POST: qualificationps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            qualificationps qualificationps = db.qualificationps.Find(id);
            db.qualificationps.Remove(qualificationps);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
