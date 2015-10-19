using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using A2.University.Web.Models;
using A2.University.Web.Models.Business;
using A2.University.Web.Models.Entities;
using A2.University.Web.Models.StaffPortal;

namespace A2.University.Web.Controllers.StaffPortal
{

    /// <summary>
    /// Controller for Results
    /// </summary>
    public class ResultsController : Controller
    {
        private readonly UniversityEntities _db = new UniversityEntities();

        /// <summary>
        /// GET: Results
        /// Displays grid of CourseEnrolments with link to view Progress
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ResultsIndexViewModel resultsIndexViewModel = new ResultsIndexViewModel();
            var courseEnrolmentsEntity = _db.CourseEnrolments
                .OrderByDescending(ce => ce.course_enrolment_id)
                .ThenByDescending(ce => ce.student_id)
                .Include(ce => ce.Course)
                .Include(ce => ce.Student)
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

        /// <summary>
        /// GET: Results/Details/5
        /// Displays a Student's progress for a particular Course.
        /// Shows transcript of unit results for the course as partial view.
        /// If Student is not enrolled in any units, error message is shown.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public ActionResult Progress(long? studentId, string courseId)
        {
            if (studentId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entitymodel, match id
            var unitEnrolmentsEntity = _db.UnitEnrolments
                // order by student id descending
                .OrderByDescending(ue => ue.student_id)
                // then by year/sem descending
                .ThenByDescending(ue => ue.year_sem)
                // then by unit title
                .ThenBy(ue => ue.Unit.title)
                .Where(ue =>
                    ue.student_id == studentId &&
                    ue.CourseEnrolment.course_id == courseId)
                .Include(ue => ue.Student)
                .Include(ue => ue.CourseEnrolment);

            // create progress summary if student has any unit enrolments
            if (unitEnrolmentsEntity.Any())
            {
                ProgressRules progressRules = new ProgressRules(studentId, courseId);

                // create viewmodels
                ProgressViewModel progressViewModel = new ProgressViewModel
                {
                    // populate summary
                    StudentId = (long)studentId,
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
                    CourseAverageGrade = GradeRules.GetGrade((int)progressRules.GetCourseAverage()),
                    CpAchieved = progressRules.GetCpAchieved(),
                    CpRemaining = progressRules.GetCpRemaining(),
                    CourseStatus = progressRules.GetCourseStatus(),
                    UnitsAttempted = progressRules.GetUnitsAttempted(),
                    HighestMark = progressRules.GetHighestMark(),
                    LowestMark = progressRules.GetLowestMark()
                };

                // create/populate transcript
                progressViewModel.TranscriptView = new TranscriptViewModel();
                foreach (UnitEnrolment result in unitEnrolmentsEntity)
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

            // else student is not enrolled a unit, show user error message instead
            TempData["notice"] = "Student is not enrolled in any units";
            return View();
        }
    }
}
