﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using A2.University.Web.Models;
using A2.University.Web.Models.Entities;

namespace A2.University.Web.Controllers
{
    public class CourseController : Controller
    {
        private UniversityEntities db = new UniversityEntities();

        // GET: Course
        public ActionResult Index()
        {
            CourseIndexViewModel courseViewModel = new CourseIndexViewModel();
            courseViewModel.Courses = db.Courses.Include(c => c.CourseType).Include(c => c.Staff).ToList();

            return View(courseViewModel.Courses);
        }

        // GET: Course/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entitymodel, match id
            Course courseEntityModel = db.Courses.Find(id);
            // create viewmodel, pass values from entitymodel
            CourseDetailsViewModel courseViewModel = new CourseDetailsViewModel();
            SetCourseViewModel(courseViewModel, courseEntityModel);

            if (courseEntityModel == null)
            {
                return HttpNotFound();
            }

            // render view using viewmodel
            return View(courseViewModel);
        }

        // GET: Course/Create
        public ActionResult Create()
        {
            // crate viewmodel
            CourseCreateViewModel courseViewModel = new CourseCreateViewModel();
            // populate dropdownlists
            courseViewModel.CoordinatorDropDownList = new SelectList(db.Staff.OrderBy(s => s.firstname), "staff_id", "fullname");
            courseViewModel.CourseTypeTitleDropDownList = new SelectList(db.CourseTypes, "course_type_id", "title");

            return View(courseViewModel);
        }

        // POST: Course/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "course_id,title,coordinator_id,course_type_id")] CourseCreateViewModel courseViewModel)
        {
            // if input passes validation
            if (ModelState.IsValid)
            {
                // create entity model, pass values from viewmodel
                Course courseEntityModel = new Course();
                SetCourseEntityModel(courseViewModel, courseEntityModel);

                // update db using entitymodel
                db.Courses.Add(courseEntityModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // populate dropdownlists
            courseViewModel.CoordinatorDropDownList = new SelectList(db.Staff.OrderBy(s => s.firstname), "staff_id", "fullname");
            courseViewModel.CourseTypeTitleDropDownList = new SelectList(db.CourseTypes, "course_type_id", "title");

            // render view using viewmodel
            return View(courseViewModel);
        }

        // GET: Course/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entitymodel, match id
            Course courseEntityModel = db.Courses.Find(id);
            // create viewmodel, pass values from entitymodel
            CourseEditViewModel courseViewModel =  new CourseEditViewModel();
            SetCourseViewModel(courseViewModel, courseEntityModel);

            // populate dropdownlists
            courseViewModel.CoordinatorDropDownList = new SelectList(db.Staff.OrderBy(s => s.firstname), "staff_id", "fullname");
            courseViewModel.CourseTypeTitleDropDownList = new SelectList(db.CourseTypes, "course_type_id", "title");

            if (courseEntityModel == null)
            {
                return HttpNotFound();
            }

            // render view using viewmodel
            return View(courseViewModel);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "course_id,title,coordinator_id,course_type_id")] Course courseEntityModel, CourseEditViewModel courseViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(courseEntityModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // populate dropdownlists
            courseViewModel.CoordinatorDropDownList = new SelectList(db.Staff.OrderBy(s => s.firstname), "staff_id", "fullname");
            courseViewModel.CourseTypeTitleDropDownList = new SelectList(db.CourseTypes, "course_type_id", "title");

            // render view using viewmodel
            return View(courseViewModel);
        }

        // GET: Course/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entitymodel, match id
            Course courseEntityModel = db.Courses.Find(id);
            // create viewmodel, pass values from entitymodel
            CourseDeleteViewModel courseViewModel = new CourseDeleteViewModel();
            SetCourseViewModel(courseViewModel, courseEntityModel);

            if (courseEntityModel == null)
            {
                return HttpNotFound();
            }
            return View(courseViewModel);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Passes data from the view model to the entity model.
        /// </summary>
        /// <param name="viewModel">UnitBaseViewModel</param>
        /// <param name="entityModel">Unit</param>
        private void SetCourseEntityModel(CourseBaseViewModel viewModel, Course entityModel)
        {
            entityModel.course_id = viewModel.course_id;
            entityModel.title = viewModel.title;
            entityModel.coordinator_id = viewModel.coordinator_id;
            entityModel.course_type_id = viewModel.course_type_id;
        }

        /// <summary>
        /// Passes data from the entity model to the view model.
        /// </summary>
        /// <param name="viewModel">UnitBaseViewModel</param>
        /// <param name="entityModel">Unit</param>
        private void SetCourseViewModel(CourseBaseViewModel viewModel, Course entityModel)
        {
            viewModel.course_id = entityModel.course_id;
            viewModel.title = entityModel.title;
            viewModel.coordinator_id = entityModel.coordinator_id;
            viewModel.course_type_id = entityModel.course_type_id;

            viewModel.coordinator_name = entityModel.Staff.firstname + " " + entityModel.Staff.surname;
            viewModel.course_type_title = entityModel.CourseType.title;
            viewModel.credit_points = entityModel.CourseType.credit_points;
            viewModel.duration = entityModel.CourseType.duration;
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
