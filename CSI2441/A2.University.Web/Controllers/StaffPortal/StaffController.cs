using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using A2.University.Web.Models;
using A2.University.Web.Models.Business.Services;
using A2.University.Web.Models.Entities;

namespace A2.University.Web.Controllers.StaffPortal
{
    public class StaffController : Controller
    {
        private readonly UniversityEntities _db = new UniversityEntities();
        private int _emailMatchTally;
        private string _email;
        private string _tempEmail;

        // GET: Staff
        public ActionResult Index()
        {
            StaffIndexViewModel staffViewModel = new StaffIndexViewModel();
            var staffEntity = _db.Staff.ToList();

            // transfer entity list to viewmodel list
            foreach (Staff staff in staffEntity)
            {
                staffViewModel.Staff.Add(new StaffIndexViewModel
                {
                    StaffId = staff.staff_id,
                    FirstName = staff.firstname,
                    LastName = staff.surname,
                    Email = staff.email,
                    FullName = staff.firstname + " " + staff.surname
                });
            }

            return View(staffViewModel.Staff);
        }

        // GET: Staff/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entitymodel, match id
            Staff staffEntityModel = _db.Staff.Find(id);
            // create viewmodel, pass values from entitymodel
            StaffDetailsViewModel staffViewModel = new StaffDetailsViewModel();
            PopulateViewModel(staffViewModel, staffEntityModel);

            if (staffEntityModel == null)
            {
                return HttpNotFound();
            }
            
            // render view using viewmodel
            return View(staffViewModel);
        }

        // GET: Staff/Create
        public ActionResult Create()
        {
            // render view using viewmodel
            return View(new StaffCreateViewModel());
        }

        // POST: Staff/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StaffId,FirstName,LastName,Email")] StaffCreateViewModel staffViewModel)
        {
            // if input passes validation
            if (ModelState.IsValid)
            {
                // create entitymodel, pass values from viewmodel
                Staff staffEntityModel = new Staff();
                PopulateEntityModel(staffViewModel, staffEntityModel);

                // update db using entitymodel
                _db.Staff.Add(staffEntityModel);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(staffViewModel);
        }

        // GET: Staff/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entitymodel, match id
            Staff staffEntityModel = _db.Staff.Find(id);
            // create viewmodel, pass values from entity model
            StaffEditViewModel staffViewModel = new StaffEditViewModel();
            PopulateViewModel(staffViewModel, staffEntityModel);

            if (staffEntityModel == null)
            {
                return HttpNotFound();
            }

            // render view using viewmodel
            return View(staffViewModel);
        }

        // POST: Staff/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StaffId,FirstName,LastName,Email")] Staff staffEntityModel, StaffEditViewModel staffViewModel)
        {
            if (ModelState.IsValid)
            {
                // populate the entitymodel
                PopulateEntityModel(staffViewModel, staffEntityModel);

                // update db using entitymodel
                _db.Entry(staffEntityModel).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(staffViewModel);
        }

        // GET: Staff/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entity model, match id
            Staff staffEntityModel = _db.Staff.Find(id);
            // create viewmodel, pass values from entitymodel
            StaffDeleteViewModel staffViewModel = new StaffDeleteViewModel();
            PopulateViewModel(staffViewModel, staffEntityModel);

            if (staffEntityModel == null)
            {
                return HttpNotFound();
            }

            // render view using viewmodel
            return View(staffViewModel);
        }

        // POST: Staff/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Staff staff = _db.Staff.Find(id);
            _db.Staff.Remove(staff);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Passes data from the view model to the entity model.
        /// </summary>
        /// <param name="viewModel">StaffBaseViewModel</param>
        /// <param name="entityModel">Staff</param>
        private void PopulateEntityModel(StaffBaseViewModel viewModel, Staff entityModel)
        {
            entityModel.staff_id = viewModel.StaffId;
            entityModel.firstname = viewModel.FirstName;
            entityModel.surname = viewModel.LastName;
            
            // generate email
            StartEmailRecursiveSearch(viewModel);
            // could pass directly to entity, but may need in view
            viewModel.Email = _email;
            entityModel.email = viewModel.Email;
        }


        /// <summary>
        /// Passes data from the entity model to the view model.
        /// </summary>
        /// <param name="viewModel">StaffBaseViewModel</param>
        /// <param name="entityModel">Staff</param>
        private void PopulateViewModel(StaffBaseViewModel viewModel, Staff entityModel)
        {
            viewModel.StaffId = entityModel.staff_id;
            viewModel.FirstName = entityModel.firstname;
            viewModel.LastName = entityModel.surname;
            viewModel.Email = entityModel.email;
        }

        /// <summary>
        /// Function initiates recursive search of generated emails to ensure no duplicates exist.
        /// Begins by reseting tallies and current Email fields, generates first version of Email, then passes to EmailRecursiveSearch.
        /// TODO: Try to implement this in GenerateEmail class, code duplication between Student/UnitControllers
        /// </summary>
        /// <param name="staff">Staff</param>
        private void StartEmailRecursiveSearch([Bind(Include = "StaffId,FirstName,LastName,Email")] StaffBaseViewModel staff)
        {
            // reset fields for each new student instance
            _emailMatchTally = 0;
            _email = "";

            // generate initial standard Email
            string target = staff.FirstName[0] + "." + staff.LastName + EmailGenerator.StaffEmailSuffix;
            // use email as target for search
            EmailRecursiveSearch(staff, target.ToLower());
        }
        
        /// <summary>
        /// Recursive function to search each version of generated Email against existing emails to ensure no duplicates exist.
        /// </summary>
        /// <param name="staff">Staff</param>
        /// <param name="target">string</param>
        private void EmailRecursiveSearch([Bind(Include = "StaffId,FirstName,LastName,Email")] StaffBaseViewModel staff,
            string target)
        {
            if (SearchEmail(target))
            {
                // increment tally
                _emailMatchTally++;
                // make new temp Email based on tally
                _tempEmail = EmailGenerator.GenerateEmail(
                    "staff",
                    _emailMatchTally,
                    staff.FirstName.ToLower(),
                    staff.LastName.ToLower());

                // recursive call to search new version of generated Email
                EmailRecursiveSearch(staff, _tempEmail);
            }
            else
            {
                _email = target;
            }
        }

        /// <summary>
        /// Function searches generated Email against existing emails in database.
        /// </summary>
        /// <param name="target">string</param>
        /// <returns>bool</returns>
        private bool SearchEmail(string target)
        {
            return _db.Staff.Any(e => e.email == target);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
