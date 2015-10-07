using System;
using System.Collections.Generic;
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
    public class ResultsController : Controller
    {
        private readonly UniversityEntities _db = new UniversityEntities();

        // GET: Results
        public ActionResult Index()
        {
            ResultsIndexViewModel resultsIndexViewModel = new ResultsIndexViewModel();
            var courseEnrolmentsEntity = _db.CourseEnrolments
                .Include(ce => 
                    ce.Course)
                .Include(ce => 
                    ce.Student)
                .ToList();

            // transfer entity list to viewmodel list
            foreach (CourseEnrolment courseEnrolment in courseEnrolmentsEntity)
            {
                resultsIndexViewModel.ResultsByCourse.Add(new ResultsIndexViewModel
                {
                    CourseEnrolmentId = courseEnrolment.course_enrolment_id,
                    StudentId = courseEnrolment.student_id,
                    StudentFirstName = courseEnrolment.Student.firstname,
                    StudentLastName = courseEnrolment.Student.lastname,
                    CourseId = courseEnrolment.course_id,
                    Title = courseEnrolment.Course.title,
                    CourseStatus = courseEnrolment.course_status
                });
            }

            return View(resultsIndexViewModel.ResultsByCourse);
        }

        // GET: Results/Details/5
        public ActionResult Progress(long? studentId, string courseId)
        {
            if (studentId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProgressRules progressRules = new ProgressRules(studentId, courseId);

            // create entitymodel, match id
            var unitEnrolmentsEntity = _db.UnitEnrolments
                .Where(ue =>
                    ue.student_id == studentId &&
                    ue.CourseEnrolment.course_id == courseId)
                .Include(ue => 
                    ue.Student)
                .Include(ue => 
                    ue.CourseEnrolment)
                .ToList();

            // create viewmodel
            ProgressViewModel progressViewModel = new ProgressViewModel
            {
                // populate summary
                StudentId = (long) studentId,
                StudentFullName =
                    $"{unitEnrolmentsEntity.Select(ue => ue.Student.firstname).FirstOrDefault()} " +
                    $"{unitEnrolmentsEntity.Select(ue => ue.Student.lastname).FirstOrDefault()}",

                CourseId = unitEnrolmentsEntity
                    .Select(ue =>
                        ue.unit_id)
                    .FirstOrDefault(),

                CourseTitle = unitEnrolmentsEntity
                    .Select(ue =>
                        ue.CourseEnrolment.Course.title)
                    .FirstOrDefault(),

                CourseAverage = progressRules.GetCourseAverage(),
                CpAchieved = progressRules.GetCpAchieved(),
                CpRemaining = progressRules.GetCpRemaining(),
                CourseStatus = progressRules.GetCourseStatus(),
                UnitsAttempted = progressRules.GetUnitsAttempted(),

                HighestMark = progressRules.GetHighestMark(),
                LowestMark = progressRules.GetLowestMark()
            };

            // populate list of results for student
            foreach (UnitEnrolment unitEnrolment in unitEnrolmentsEntity)
            {
                // each row of results table
                progressViewModel.UnitResults.Add(new ProgressViewModel
                {
                    UnitEnrolmentId = unitEnrolment.unit_enrolment_id,
                    CourseEnrolmentId = unitEnrolment.course_enrolment_id,
                    UnitId = unitEnrolment.unit_id,
                    UnitTitle = unitEnrolment.Unit.title,
                    Mark = unitEnrolment.mark,
                    Grade = GradeRules.GetGrade(unitEnrolment.mark)
                });
            }

            return View(progressViewModel);
        }

        // GET: Results/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Results/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Results/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Results/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Results/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Results/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
