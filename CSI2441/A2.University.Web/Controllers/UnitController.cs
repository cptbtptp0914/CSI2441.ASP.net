using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using A2.University.Web.Models.Entities;

namespace A2.University.Web.Controllers
{
    public class UnitController : Controller
    {
        private UniversityEntities db = new UniversityEntities();

        // GET: Unit
        public ActionResult Index()
        {
            var units = db.Units.Include(u => u.Staff).Include(u => u.UnitType);
            return View(units.ToList());
        }

        // GET: Unit/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unit unit = db.Units.Find(id);
            if (unit == null)
            {
                return HttpNotFound();
            }
            return View(unit);
        }

        // GET: Unit/Create
        public ActionResult Create()
        {
            ViewBag.coodinator_id = new SelectList(db.Staffs, "staff_id", "firstname");
            ViewBag.unit_type_id = new SelectList(db.UnitTypes, "unit_type_id", "title");
            return View();
        }

        // POST: Unit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "unit_id,title,coodinator_id,credit_points,unit_type_id")] Unit unit)
        {
            if (ModelState.IsValid)
            {
                db.Units.Add(unit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.coodinator_id = new SelectList(db.Staffs, "staff_id", "firstname", unit.coodinator_id);
            ViewBag.unit_type_id = new SelectList(db.UnitTypes, "unit_type_id", "title", unit.unit_type_id);
            return View(unit);
        }

        // GET: Unit/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unit unit = db.Units.Find(id);
            if (unit == null)
            {
                return HttpNotFound();
            }
            ViewBag.coodinator_id = new SelectList(db.Staffs, "staff_id", "firstname", unit.coodinator_id);
            ViewBag.unit_type_id = new SelectList(db.UnitTypes, "unit_type_id", "title", unit.unit_type_id);
            return View(unit);
        }

        // POST: Unit/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "unit_id,title,coodinator_id,credit_points,unit_type_id")] Unit unit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.coodinator_id = new SelectList(db.Staffs, "staff_id", "firstname", unit.coodinator_id);
            ViewBag.unit_type_id = new SelectList(db.UnitTypes, "unit_type_id", "title", unit.unit_type_id);
            return View(unit);
        }

        // GET: Unit/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unit unit = db.Units.Find(id);
            if (unit == null)
            {
                return HttpNotFound();
            }
            return View(unit);
        }

        // POST: Unit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Unit unit = db.Units.Find(id);
            db.Units.Remove(unit);
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
