using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using A2.University.Web.Models;
using A2.University.Web.Models.Business;
using A2.University.Web.Models.Entities;

namespace A2.University.Web.Controllers.StaffPortal
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

            // create viewmodels
            ProgressViewModel progressViewModel = new ProgressViewModel
            {
                // populate summary
                StudentId = (long) studentId,
                StudentFullName =
                    $"{unitEnrolmentsEntity.Select(ue => ue.Student.firstname).FirstOrDefault()} " +
                    $"{unitEnrolmentsEntity.Select(ue => ue.Student.lastname).FirstOrDefault()}",

                CourseId = unitEnrolmentsEntity
                    .Select(ue =>
                        ue.CourseEnrolment.course_id)
                    .FirstOrDefault(),

                CourseTitle = unitEnrolmentsEntity
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
                LowestMark = progressRules.GetLowestMark()
            };

            progressViewModel.TranscriptView = new TranscriptViewModel();
            foreach (UnitEnrolment result in unitEnrolmentsEntity.OrderBy(ue => ue.year_sem))
            {
                progressViewModel.TranscriptView.Transcript.Add(new ProgressViewModel
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
