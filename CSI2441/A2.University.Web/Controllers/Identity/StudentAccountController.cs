using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using A2.University.Web.Models;
using A2.University.Web.Models.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace A2.University.Web.Controllers.Identity
{
    [Authorize]
    public class StudentAccountController : Controller
    {

        /// <summary>
        /// GET: /StudentAccount/Login
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// POST: /StudentAccount/Login
        /// User validation occurs in ValidatorModels.StudentLoginViewModelValidator class
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(StudentLoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Email = model.Email;
                return RedirectToAction("Index", "StudentPortal", new { email = model.Email });
            }

            return View(model);
        }

        /// <summary>
        /// GET: /StudentAccount/Register
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /StudentAccount/Register
        /// <summary>
        /// Overriding AccountController's Register POST method.
        /// Adds user and role to UserRole table, STUDENT in this case.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(StudentRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                UniversityEntities db = new UniversityEntities();

                // get student details
                var student = db.Students.FirstOrDefault(s => s.email == model.Email);

                if (student != null)
                {
                    // create entity model, pass values from viewmodel
                    StudentUser studentUserEntityModel = new StudentUser
                    {
                        student_id = student.student_id,
                        email = model.Email,
                        password = model.Password,
                        role = model.Role

                    };

                    // update db using entitymodel
                    db.StudentUsers.Add(studentUserEntityModel);
                    db.SaveChanges();

                    // provide feedback to user
                    TempData["notice"] = $"User {studentUserEntityModel.email} successfully created an account";

                    // successful registration, send user to portal
                    return RedirectToAction("Index", "StudentPortal", new {email = model.Email});
                }
            }

            // else there are validation errors
            return View(model);
        }
    }
}