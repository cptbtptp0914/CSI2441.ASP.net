using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using A2.University.Web.Models;
using A2.University.Web.Models.Business;
using A2.University.Web.Models.Entities;

namespace A2.University.Web.Controllers
{
    public class UnitEnrolmentController : Controller
    {
        private UniversityEntities db = new UniversityEntities();

        // GET: UnitEnrolments
        public ActionResult Index()
        {
            UnitEnrolmentIndexViewModel unitEnrolmentViewModel = new UnitEnrolmentIndexViewModel();
            var unitEnrolmentsEntity = db.UnitEnrolments.Include(u => u.Student).Include(u => u.Unit).Include(u => u.CourseEnrolment).ToList();

            // transfer entity list to viewmodel list
            foreach (UnitEnrolment unitEnrolment in unitEnrolmentsEntity)
            {
                unitEnrolmentViewModel.UnitEnrolments.Add(new UnitEnrolmentIndexViewModel
                {
                    UnitEnrolmentId = unitEnrolment.unit_enrolment_id,
                    StudentId = unitEnrolment.student_id,
                    StudentFirstName = unitEnrolment.Student.firstname,
                    StudentLastName = unitEnrolment.Student.lastname,
                    UnitId = unitEnrolment.unit_id,
                    Title = unitEnrolment.Unit.title,
                    YearSem = unitEnrolment.year_sem.ToString(),
                    Mark = unitEnrolment.mark.ToString(),
                    Grade = GradeRules.GetGrade(unitEnrolment.mark),
                    CourseId = unitEnrolment.CourseEnrolment.course_id
                });
            }

            // render view using viewmodel list
            return View(unitEnrolmentViewModel.UnitEnrolments);
        }

        // GET: UnitEnrolments/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entitymodel, match id
            UnitEnrolment unitEnrolmentEntityModel = db.UnitEnrolments.Find(id);
            // create viewmodel, pass values from entitymodel
            UnitEnrolmentDetailsViewModel unitEnrolmentViewModel = new UnitEnrolmentDetailsViewModel
            {
                // derived field
                CourseIdTitle = 
                    $"{unitEnrolmentEntityModel.CourseEnrolment.course_id} " +
                    $"{unitEnrolmentEntityModel.CourseEnrolment.Course.title}"

            };

            PopulateViewModel(unitEnrolmentViewModel, unitEnrolmentEntityModel);

            if (unitEnrolmentEntityModel == null)
            {
                return HttpNotFound();
            }
            return View(unitEnrolmentViewModel);
        }

        // GET: UnitEnrolments/Create
        public ActionResult Create()
        {
            // create viewmodel
            UnitEnrolmentCreateViewModel unitEnrolmentViewModel = new UnitEnrolmentCreateViewModel();
            // populate dropdownlists
            PopulateDropDownLists(unitEnrolmentViewModel);

            return View(unitEnrolmentViewModel);
        }

        // POST: UnitEnrolments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UnitEnrolmentId,StudentId,UnitId,YearSem,Mark")] UnitEnrolmentCreateViewModel unitEnrolmentViewModel)
        {
            // if input passes validation
            if (ModelState.IsValid)
            {
                // create entitymodel, pass values from viewmodel
                UnitEnrolment unitEnrolmentEntityModel = new UnitEnrolment();
                PopulateEntityModel(unitEnrolmentViewModel, unitEnrolmentEntityModel);

                // update db using entitymodel
                db.UnitEnrolments.Add(unitEnrolmentEntityModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // populate dropdownlists
            PopulateDropDownLists(unitEnrolmentViewModel);

            return View(unitEnrolmentViewModel);
        }

        // GET: UnitEnrolments/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entitymodel, match id
            UnitEnrolment unitEnrolmentEntityModel = db.UnitEnrolments.Find(id);
            // create viewmodel, pass values from entitymodel
            UnitEnrolmentEditViewModel unitEnrolmentViewModel = new UnitEnrolmentEditViewModel
            {
                // readonly field in view
                StudentIdFullName = 
                    $"{unitEnrolmentEntityModel.Student.student_id} " +
                    $"{unitEnrolmentEntityModel.Student.firstname} " +
                    $"{unitEnrolmentEntityModel.Student.lastname}"
            };

            PopulateViewModel(unitEnrolmentViewModel, unitEnrolmentEntityModel);

            // populate dropdownlists
            PopulateDropDownLists(unitEnrolmentViewModel);

            if (unitEnrolmentEntityModel == null)
            {
                return HttpNotFound();
            }

            // render view using viewmodel
            return View(unitEnrolmentViewModel);
        }

        // POST: UnitEnrolments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UnitEnrolmentId,StudentId,UnitId,YearSem,Mark")] UnitEnrolment unitEnrolmentEntityModel, UnitEnrolmentEditViewModel unitEnrolmentViewModel)
        {
            if (ModelState.IsValid)
            {
                // populate entitymodel
                PopulateEntityModel(unitEnrolmentViewModel, unitEnrolmentEntityModel);

                // update db using entitymodel
                db.Entry(unitEnrolmentEntityModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // populate dropdownlists
            PopulateDropDownLists(unitEnrolmentViewModel);

            return View(unitEnrolmentViewModel);
        }

        // GET: UnitEnrolments/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entitymodel, match id
            UnitEnrolment unitEnrolmentEntityModel = db.UnitEnrolments.Find(id);
            // create viewmodel, pass values from entitymodel
            UnitEnrolmentDeleteViewModel unitEnrolmentViewModel = new UnitEnrolmentDeleteViewModel();
            PopulateViewModel(unitEnrolmentViewModel, unitEnrolmentEntityModel);

            if (unitEnrolmentEntityModel == null)
            {
                return HttpNotFound();
            }
            return View(unitEnrolmentViewModel);
        }

        // POST: UnitEnrolments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            UnitEnrolment unitEnrolment = db.UnitEnrolments.Find(id);
            db.UnitEnrolments.Remove(unitEnrolment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Populates dropdownlists for unit enrolment view.
        /// </summary>
        /// <param name="viewModel">UnitEnrolmentDropDownListViewModel</param>
        private void PopulateDropDownLists(UnitEnrolmentDropDownListViewModel viewModel)
        {
            string state = new CourseRules().CourseStates["Enrolled"];

            // get list of students ENROLLED in a course
            var courseEnrolmentsEntity = (
                from courseEnrolment in db.CourseEnrolments
                where courseEnrolment.course_status == state
                select courseEnrolment)
                .Include(s => s.Student) // join Student table
                .ToList();

            // get list of units
            var unitsEntity = (from units in db.Units
                select units).ToList();

            // transfer relevant elements to viewmodel list
            foreach (CourseEnrolment enrolment in courseEnrolmentsEntity)
            {
                viewModel.Students.Add(new UnitEnrolmentDropDownListViewModel
                {
                    StudentId = enrolment.student_id,
                    StudentIdFullName = 
                        $"{enrolment.student_id} " +
                        $"{enrolment.Student.firstname} " +
                        $"{enrolment.Student.lastname}"
                });
            }

            foreach (Unit unit in unitsEntity)
            {
                viewModel.Units.Add(new UnitEnrolmentDropDownListViewModel
                {
                    UnitId = unit.unit_id,
                    UnitIdTitle = $"{unit.unit_id} {unit.title}"
                });
            }

            // populate dropdownlist from viewmodel list
            viewModel.StudentDropDownList = new SelectList(viewModel.Students.OrderBy(s => s.StudentId), "StudentId", "StudentIdFullName");
            viewModel.UnitDropDownList = new SelectList(viewModel.Units.OrderBy(u => u.UnitId), "UnitId", "UnitIdTitle");
        }

        /// <summary>
        /// Passes data from the view model to the entity model.
        /// </summary>
        /// <param name="viewModel">UnitBaseViewModel</param>
        /// <param name="entityModel">Unit</param>
        private void PopulateEntityModel(UnitEnrolmentBaseViewModel viewModel, UnitEnrolment entityModel)
        {
            entityModel.unit_enrolment_id = viewModel.UnitEnrolmentId;
            entityModel.student_id = viewModel.StudentId;
            entityModel.unit_id = viewModel.UnitId;
            entityModel.year_sem = int.Parse(viewModel.YearSem);
            entityModel.mark = int.Parse(viewModel.Mark);

            // can't use dict in linq, substitute with string
            string state = new CourseRules().CourseStates["Enrolled"];

            // select course_enrolment_id where StudentId is match, and is ENROLLED
            var query = 
                (from ce in db.CourseEnrolments
                where ce.student_id == viewModel.StudentId &&
                      ce.course_status == state
                select new { ce.course_enrolment_id }).Single();

            // pass value to viewModel, might need it
            viewModel.CourseEnrolmentId = query.course_enrolment_id;

            // pass value to entitymodel
            entityModel.course_enrolment_id = viewModel.CourseEnrolmentId;
        }

        /// <summary>
        /// Passes data from the entity model to the view model.
        /// </summary>
        /// <param name="viewModel">UnitBaseViewModel</param>
        /// <param name="entityModel">Unit</param>
        private void PopulateViewModel(UnitEnrolmentBaseViewModel viewModel, UnitEnrolment entityModel)
        {
            viewModel.UnitEnrolmentId = entityModel.unit_enrolment_id;

            viewModel.StudentId = entityModel.student_id;
            viewModel.UnitId = entityModel.unit_id;
            viewModel.YearSem = entityModel.year_sem.ToString();
            viewModel.Mark = entityModel.mark.ToString();

            viewModel.StudentFullName =
                $"{entityModel.Student.firstname} " +
                $"{ entityModel.Student.lastname}";

            viewModel.Title = entityModel.Unit.title;
            viewModel.Grade = GradeRules.GetGrade(entityModel.mark);
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
