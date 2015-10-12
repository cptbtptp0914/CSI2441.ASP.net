﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using A2.University.Web.Models;
using A2.University.Web.Models.Business;
using A2.University.Web.Models.Entities;
using A2.University.Web.Models.StudentPortal;

namespace A2.University.Web.Controllers
{
    [UniAuthorize(Roles = "STUDENT")]
    public class StudentPortalController : Controller
    {
        private readonly UniversityEntities _db = new UniversityEntities();

        // GET: StudentPortal
        public ActionResult Index(string email)
        {
            // get current student by matching login viewmodel's email with student's email in database
            var currentStudent = _db.Students
                .FirstOrDefault(s => s.email == email);

            if (currentStudent != null)
            {
                // populate info for student
                StudentCoursesViewModel studentPortalViewModel = new StudentCoursesViewModel
                {
                    StudentId = currentStudent.student_id,
                    StudentFirstName = currentStudent.firstname,
                    StudentLastName = currentStudent.lastname,
                    StudentFullName =
                        $"{currentStudent.firstname} " +
                        $"{currentStudent.lastname}",

                    Dob = currentStudent.dob.ToString("dd/MM/yyyy"),
                    Gender = currentStudent.gender,
                    Email = currentStudent.email,
                    Landline = currentStudent.ph_landline,
                    Mobile = currentStudent.ph_mobile,
                    Address = currentStudent.adrs,
                    City = currentStudent.adrs_city,
                    State = currentStudent.adrs_state,
                    Postcode = currentStudent.adrs_postcode
                };

                // transfer list to viewmodel list
                studentPortalViewModel.CoursesList = new StudentCourseEnrolmentListViewModel();
                foreach (CourseEnrolment courseEnrolment in currentStudent.CourseEnrolments.OrderByDescending(c => c.course_enrolment_id))
                {
                    studentPortalViewModel.CoursesList.StudentCourseEnrolments.Add(new StudentCourseEnrolmentListViewModel
                    {
                        StudentId = currentStudent.student_id,
                        StudentFirstName = currentStudent.firstname,
                        StudentLastName = currentStudent.lastname,
                        StudentFullName =
                            $"{currentStudent.firstname} " +
                            $"{currentStudent.lastname}",

                        CourseEnrolmentId = courseEnrolment.course_enrolment_id,
                        CourseId = courseEnrolment.course_id,
                        CourseTitle = courseEnrolment.Course.title,
                        CourseStatus = courseEnrolment.course_status
                    });
                }

                return View(studentPortalViewModel);
            }

            // if we get here, student email was not found in db
            return View();
        }

        public ActionResult Progress(long? studentId, string courseId)
        {
            if (studentId == null || courseId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProgressRules progressRules = new ProgressRules(studentId, courseId);

            // create entity model, match id
            var unitResultEntity = _db.UnitEnrolments
                .Where(ue =>
                    ue.student_id == studentId &&
                    ue.CourseEnrolment.course_id == courseId)
                .Include(ue =>
                    ue.Student)
                .Include(ue =>
                    ue.CourseEnrolment)
                .ToList();

            // create viewmodels

            // populate summary
            StudentProgressViewModel progressViewModel = new StudentProgressViewModel
            {
                StudentId = (long) studentId,
                StudentFullName =
                    $"{unitResultEntity.Select(ue => ue.Student.firstname).FirstOrDefault()} " +
                    $"{unitResultEntity.Select(ue => ue.Student.lastname).FirstOrDefault()}",

                CourseId = unitResultEntity
                    .Select(ue =>
                        ue.CourseEnrolment.course_id)
                    .FirstOrDefault(),

                CourseTitle = unitResultEntity
                    .Select(ue =>
                        ue.CourseEnrolment.Course.title)
                    .FirstOrDefault(),

                CourseAverageMark = progressRules.GetCourseAverage(),
                CourseAverageGrade = GradeRules.GetGrade((int) progressRules.GetCourseAverage()),
                CpAchieved = progressRules.GetCpAchieved(),
                CpRemaining = progressRules.GetCpRemaining(),
                CourseStatus = progressRules.GetCourseStatus(),
                UnitsAttempted = progressRules.GetUnitsAttempted(),
                HighestMark = progressRules.GetHighestMark(),
                LowestMark = progressRules.GetLowestMark(),

                Email = unitResultEntity.Select(ur => ur.Student.email).FirstOrDefault()
            };

            // populate transcript
            progressViewModel.TranscriptView = new StudentTranscriptViewModel();
            foreach (UnitEnrolment result in unitResultEntity)
            {
                progressViewModel.TranscriptView.Transcript.Add(new StudentProgressViewModel
                {
                    UnitEnrolmentId = result.unit_enrolment_id,
                    CourseEnrolmentId = result.course_enrolment_id,
                    UnitId = result.unit_id,
                    UnitTitle = result.Unit.title,
                    YearSem = result.year_sem,
                    Mark = result.mark,
                    Grade = GradeRules.GetGrade(result.mark)
                });
            }

            return View(progressViewModel);
        }
    }
}