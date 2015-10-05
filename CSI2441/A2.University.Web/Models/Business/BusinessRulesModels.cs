using System.Collections.Generic;
using System.Linq;
using A2.University.Web.Models.Entities;

namespace A2.University.Web.Models.Business
{
    public static class GradeRules
    {
        public static string GetGrade(int mark)
        {
            if (mark >= 80)
            {
                return "HD";
            }
            else if (mark >= 70)
            {
                return "D";
            }
            else if (mark >= 60)
            {
                return "CR";
            }
            else if (mark >= 50)
            {
                return "C";
            }
            else
            {
                return "N";
            }
        }
    }

    public class UnitRules
    {
        private readonly UniversityEntities db;
        private const int Pass = 50;
        private const int MaxAttempts = 3;

        public UnitRules()
        {
            db = new UniversityEntities();
        }

        /// <summary>
        /// Checks passed unit uniqueness.
        /// </summary>
        /// <param name="studentId">long</param>
        /// <param name="unitId">string</param>
        /// <param name="mark">int</param>
        /// <returns>bool</returns>
        public bool IsUniquePass(long studentId, string unitId, int mark)
        {
            var passes = db.UnitEnrolments.FirstOrDefault(
                ue => ue.student_id == studentId && 
                ue.unit_id == unitId 
                && ue.mark >= Pass);

            return passes != null;
        }

        /// <summary>
        /// Checks unit uniqueness per semester.
        /// </summary>
        /// <param name="studentId">long</param>
        /// <param name="unitId">string</param>
        /// <param name="yearSem">int</param>
        /// <returns>bool</returns>
        public bool IsUniqueInSem(long studentId, string unitId, int yearSem)
        {
            var unit = db.UnitEnrolments.FirstOrDefault(
                    ue => ue.student_id == studentId && 
                    ue.unit_id == unitId && 
                    ue.year_sem == yearSem);

            return unit != null;
        }
        
        /// <summary>
        /// Checks max unit attempts.
        /// </summary>
        /// <param name="studentId">long</param>
        /// <param name="unitId">string</param>
        /// <returns>bool</returns>
        public bool IsMaxAttempts(long studentId, string unitId)
        {
            var query =
                (from ue in db.UnitEnrolments
                    where ue.student_id == studentId &&
                    ue.unit_id == unitId
                    select ue).Count();

            return query >= MaxAttempts;
        }
    }

    public class CourseRules
    {
        private readonly UniversityEntities db;

        public Dictionary<string, string> CourseStates;

        public CourseRules()
        {
            db = new UniversityEntities();

            CourseStates = new Dictionary<string, string>
            {
                {"Enrolled", "ENROLLED"},
                { "Discontinued", "DISCONTIN"}
            };
        }

        /// <summary>
        /// Checks ENROLLED course uniqueness.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public bool IsNotUniqueEnrolled(long studentId)
        {
            // can't use dict in linq, substitute with string
            string state = CourseStates["Enrolled"];

            var course = db.CourseEnrolments.FirstOrDefault(
                ce => ce.student_id == studentId &&
                      ce.course_status == state);

            return course != null;
        }

        /// <summary>
        /// Checks if student is enrolled in a course.
        /// Student must be enrolled in course first before enrolling in any units.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public bool IsStudentCourseEnrolled(long studentId)
        {
            // can't use dict in linq, substitute with string
            string state = CourseStates["Enrolled"];

            var courseEnrolment =
                db.CourseEnrolments.FirstOrDefault(
                    ce => ce.student_id == studentId &&
                          ce.course_status == state);

            return courseEnrolment != null;
        }
    }
}