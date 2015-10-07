using System;
using System.Collections.Generic;
using System.Linq;
using A2.University.Web.Models.Entities;
using Microsoft.Ajax.Utilities;

namespace A2.University.Web.Models.Business
{
    public static class GradeRules
    {

        /// <summary>
        /// Returns the grade of a mark.
        /// </summary>
        /// <param name="mark">int</param>
        /// <returns></returns>
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
        private readonly UniversityEntities _db;
        private const int Pass = 50;
        private const int MaxAttempts = 3;

        public UnitRules()
        {
            _db = new UniversityEntities();
        }

        public bool IsNotUniqueUnitId(string unitId)
        {
            var units = _db.Units.FirstOrDefault(
                u => u.unit_id == unitId);

            return units != null;
        }

        /// <summary>
        /// Checks passed unit uniqueness for Create view.
        /// </summary>
        /// <param name="unitEnrolmentId">long</param>
        /// <param name="studentId">long</param>
        /// <param name="unitId">string</param>
        /// <param name="mark">int</param>
        /// <returns>bool</returns>
        public bool IsNotUniquePassedUnit(long unitEnrolmentId, long studentId, string unitId, int mark)
        {
            var passes = _db.UnitEnrolments.FirstOrDefault(
                // ignore own id in query
                ue => ue.unit_enrolment_id != unitEnrolmentId &&
                ue.student_id == studentId && 
                ue.unit_id == unitId &&
                ue.mark >= Pass);

            return passes != null;
        }

        /// <summary>
        /// Checks unit uniqueness per semester for Create view.
        /// </summary>
        /// <param name="unitEnrolmentId">long</param>
        /// <param name="studentId">long</param>
        /// <param name="unitId">string</param>
        /// <param name="yearSem">int</param>
        /// <returns>bool</returns>
        public bool IsNotUniqueUnitInSem(long unitEnrolmentId, long studentId, string unitId, int yearSem)
        {
            var unit = _db.UnitEnrolments.FirstOrDefault(
                // ignore own id in query
                ue => ue.unit_enrolment_id != unitEnrolmentId &&
                ue.student_id == studentId && 
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
                (from ue in _db.UnitEnrolments
                    where ue.student_id == studentId &&
                    ue.unit_id == unitId
                    select ue).Count();

            return query >= MaxAttempts;
        }
    }

    public class CourseRules
    {
        private readonly UniversityEntities _db;

        public Dictionary<string, string> CourseStates;

        public CourseRules()
        {
            _db = new UniversityEntities();

            CourseStates = new Dictionary<string, string>
            {
                { "Completed", "COMPLETED" },
                { "Enrolled", "ENROLLED"},
                { "Discontinued", "DISCONTIN"},
                { "Excluded", "EXCLUDED" }
            };
        }

        /// <summary>
        /// Checks ENROLLED course uniqueness.
        /// </summary>
        /// <param name="studentId">long</param>
        /// <returns></returns>
        public bool IsNotUniqueEnrolled(long studentId)
        {
            // can't use dict in linq, substitute with string
            string state = CourseStates["Enrolled"];

            var course = _db.CourseEnrolments.FirstOrDefault(
                ce => ce.student_id == studentId &&
                      ce.course_status == state);

            return course != null;
        }

        /// <summary>
        /// Checks if student is ENROLLED in a course.
        /// Student must be ENROLLED in a course first before enrolling in any units.
        /// </summary>
        /// <param name="studentId">long</param>
        /// <returns></returns>
        public bool IsStudentCourseEnrolled(long studentId)
        {
            // can't use dict in linq, substitute with string
            string state = CourseStates["Enrolled"];

            var courseEnrolment =
                _db.CourseEnrolments.FirstOrDefault(
                    ce => ce.student_id == studentId &&
                          ce.course_status == state);

            return courseEnrolment != null;
        }

        /// <summary>
        /// Checks course enrolment uniqueness per Student.
        /// </summary>
        /// <param name="studentId">long</param>
        /// <param name="courseId">string</param>
        /// <returns></returns>
        public bool IsNotUniqueCourse(long studentId, string courseId)
        {
            var course = _db.CourseEnrolments.FirstOrDefault(
                    ce => ce.student_id == studentId &&
                    ce.course_id == courseId);

            return course != null;
        }
    }

    public class ProgressRules
    {
        private const int Pass = 50;

        private readonly List<UnitEnrolment> _unitResults;
        private readonly int _cpRequired;
        private readonly int _duration;

        public ProgressRules(long? studentId, string courseId)
        {
            UniversityEntities db = new UniversityEntities();

            // get list of unit enrolments for this course
            _unitResults = db.UnitEnrolments
                .Where(ue =>
                    ue.student_id == studentId &&
                    ue.CourseEnrolment.course_id == courseId)
                .ToList();

            // set cp required
            _cpRequired = _unitResults
                .Select(ue =>
                    ue.CourseEnrolment.Course.CourseType.credit_points)
                .Single();

            // set duration
            _duration = _unitResults
                .Select(ue =>
                    ue.CourseEnrolment.Course.CourseType.duration)
                .Single();
        }

        /// <summary>
        /// Returns course average of marks.
        /// </summary>
        /// <returns></returns>
        public double GetCourseAverage()
        {
            // get sum of results
            var sum = _unitResults
                .Sum(result => 
                    result.mark);

            double average = sum / _unitResults.Count;

            // return average
            return Math.Round(average, 2);
        }


        /// <summary>
        /// Returns CP achieved.
        /// </summary>
        /// <returns></returns>
        public int GetCpAchieved()
        {
            // return cp sum where mark is pass
            return _unitResults
                .Where(result =>
                    result.mark >= Pass)
                .Sum(result => 
                    result.Unit.credit_points);
        }

        /// <summary>
        /// Returns CP remaining.
        /// </summary>
        /// <returns></returns>
        public int GetCpRemaining()
        {
            var sum = _unitResults
                .Where(result =>
                    result.mark >= Pass)
                .Sum(result =>
                    result.Unit.credit_points);

            return _cpRequired - sum;
        }
        
        /// <summary>
        /// Returns course status.
        /// </summary>
        /// <returns></returns>
        public string GetCourseStatus()
        {
            return _unitResults
                .Select(result =>
                    result.CourseEnrolment.course_status)
                .Single();
        }

        /// <summary>
        /// Returns count of units attempted.
        /// </summary>
        /// <returns></returns>
        public int GetUnitsAttempted()
        {
            return _unitResults.Count;
        }

        /// <summary>
        /// Returns list of highest mark/s.
        /// </summary>
        /// <returns></returns>
        public List<UnitEnrolment> GetHighestMark()
        {
            // get the highest mark
            var highest = _unitResults
                .Max(results =>
                    results.mark);

            // return result/s with highest mark
            return _unitResults
                .Where(result =>
                    result.mark == highest)
                .ToList();
        }

        /// <summary>
        /// Returns list of lowest mark/s.
        /// </summary>
        /// <returns></returns>
        public List<UnitEnrolment> GetLowestMark()
        {
            // get the lowest mark
            var lowest = _unitResults
                .Min(result =>
                    result.mark);

            // return result/s with lowest mark
            return _unitResults
                .Where(result =>
                    result.mark == lowest)
                .ToList();
        } 
    }
}