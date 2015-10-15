using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using A2.University.Web.Models;
using A2.University.Web.Models.Entities;
using A2.University.Web.Models.StaffPortal;

namespace A2.University.Web.Controllers.StaffPortal
{

    /// <summary>
    /// Controller for Unit
    /// </summary>
    public class UnitController : Controller
    {
        private readonly UniversityEntities _db = new UniversityEntities();

        /// <summary>
        /// GET: Unit
        /// Displays Unit/Index CRUD grid of all Units in database.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {          
            UnitIndexViewModel unitViewModel = new UnitIndexViewModel();

            // get list of units from db
            var unitsEntity = _db.Units
                // order by unit id
                .OrderBy(u => u.unit_id)
                // joins
                .Include(u => u.Staff)
                .Include(u => u.UnitType)
                .ToList();

            // transfer entity list to viewmodel list
            foreach (Unit unit in unitsEntity)
            {
                unitViewModel.Units.Add(new UnitIndexViewModel
                {
                    // core fields
                    UnitId = unit.unit_id,
                    Title = unit.title,
                    CoordinatorId = unit.coordinator_id,
                    CreditPoints = unit.credit_points,
                    UnitTypeId = unit.unit_type_id,
                    // derived fields
                    StaffFullName = 
                        $"{unit.Staff.firstname} " + 
                        $"{unit.Staff.surname}",

                    UnitTypeTitle = unit.UnitType.title
                });
            }

            // render view using viewmodel list
            return View(unitViewModel.Units);
        }

        /// <summary>
        /// GET: Unit/Details/5
        /// Shows details of Unit when "View" link clicked.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entitymodel, match id
            Unit unitEntityModel = _db.Units.Find(id);
            // create viewmodel, pass values from entitymodel
            UnitDetailsViewModel unitViewModel = new UnitDetailsViewModel();
            PopulateViewModel(unitViewModel, unitEntityModel);

            if (unitEntityModel == null)
            {
                return HttpNotFound();
            }

            // render view using viewmodel
            return View(unitViewModel);
        }

        /// <summary>
        /// GET: Unit/Create
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {   
            // create viewmodel
            UnitCreateViewModel unitViewModel = new UnitCreateViewModel();
            PopulateDropDownLists(unitViewModel);
           
            // render view using viewmodel
            return View(unitViewModel);
        }

        /// <summary>
        /// POST: Unit/Create
        /// Stores new Unit in database if passes validation, defined by UnitBaseViewModelValidator.
        /// Shows feedback to user when successfully creates new unit.
        /// </summary>
        /// <param name="unitViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UnitId,Title,CoordinatorId,CreditPoints,UnitTypeId")] UnitCreateViewModel unitViewModel)
        {

            // if input passes validation
            if (ModelState.IsValid)
            {
                // create entity model, pass values from viewmodel
                Unit unitEntityModel = new Unit();
                PopulateEntityModel(unitViewModel, unitEntityModel);

                // update db using entitymodel
                _db.Units.Add(unitEntityModel);
                _db.SaveChanges();

                // provide feedback to user
                TempData["notice"] = $"Unit {unitEntityModel.unit_id} {unitEntityModel.title} was successfully created";

                return RedirectToAction("Index");
            }

            // populate dropdownlists
            PopulateDropDownLists(unitViewModel);

            // render view using viewmodel
            return View(unitViewModel);
        }

        /// <summary>
        /// GET: Unit/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entitymodel, match id
            Unit unitEntityModel = _db.Units.Find(id);
            // create viewmodel, pass values from entitymodel
            UnitEditViewModel unitViewModel = new UnitEditViewModel();
            PopulateViewModel(unitViewModel, unitEntityModel);

            // populate dropdownlists
            PopulateDropDownLists(unitViewModel);

            if (unitEntityModel == null)
            {
                return HttpNotFound();
            }

            // render view using viewmodel
            return View(unitViewModel);
        }

        /// <summary>
        /// POST: Unit/Edit/5
        /// Stores edited data if viewmodel passes validation.
        /// Shows feedback to user when successfully edits data.
        /// </summary>
        /// <param name="unitEntityModel"></param>
        /// <param name="unitViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UnitId,Title,CoordinatorId,CreditPoints,UnitTypeId")] Unit unitEntityModel, UnitEditViewModel unitViewModel)
        {
            if (ModelState.IsValid)
            {
                // populate entitymodel
                PopulateEntityModel(unitViewModel, unitEntityModel);

                // update db using entitymodel
                _db.Entry(unitEntityModel).State = EntityState.Modified;
                _db.SaveChanges();

                // provide feedback to user
                TempData["notice"] = $"Unit {unitEntityModel.unit_id} {unitEntityModel.title} was successfully edited";

                return RedirectToAction("Index");
            }

            // populate dropdownlists
            PopulateDropDownLists(unitViewModel);

            return View(unitViewModel);
        }

        /// <summary>
        /// GET: Unit/Delete/5
        /// Displays "Are you sure you want to delete" view.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // create entitymodel, match id
            Unit unitEntityModel = _db.Units.Find(id);
            // create viewmodel, pass values from entitymodel
            UnitDeleteViewModel unitViewModel = new UnitDeleteViewModel();
            PopulateViewModel(unitViewModel, unitEntityModel);

            // get number of affected rows
            int rows = GetNumberOfAffectedRows(id);
            if (rows > 0)
            {
                // tell user how many rows this deletion will affect
                TempData["delete-notice"] = $"WARNING: Deleting this record will also delete {rows} other record/s in the database!";
            }

            if (unitEntityModel == null)
            {
                return HttpNotFound();
            }
            return View(unitViewModel);
        }

        /// <summary>
        /// POST: Unit/Delete/5
        /// Deletes row from database.
        /// Shows feedback to user when successfully deletes.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Unit unit = _db.Units.Find(id);

            // do own cascade on delete
            CascadeOnDelete(id);

            _db.Units.Remove(unit);
            _db.SaveChanges();

            // provide feedback to user
            TempData["notice"] = $"Unit {unit.unit_id} {unit.title} was successfully deleted";

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Populates dropdownlists for unit view.
        /// </summary>
        /// <param name="viewModel">UnitDropDownListViewModel</param>
        private void PopulateDropDownLists(UnitDropDownListViewModel viewModel)
        {
            // get list of students/units from db
            var staffEntity = _db.Staff.ToList();
            var unitTypesEntity = _db.UnitTypes.ToList();

            // transfer relevant elements to viewmodel list
            foreach (Staff staff in staffEntity)
            {
                viewModel.Coordinators.Add(new UnitDropDownListViewModel
                {
                    CoordinatorId = staff.staff_id,
                    StaffIdFullName = 
                        $"{staff.staff_id} " +
                        $"{staff.firstname} " +
                        $"{staff.surname}"
                });
            }

            foreach (UnitType type in unitTypesEntity)
            {
                viewModel.UnitTypes.Add(new UnitDropDownListViewModel
                {
                    UnitTypeId = type.unit_type_id,
                    UnitTypeTitle = type.title
                });
            }

            // populate dropdownlist from viewmodel list
            viewModel.CoordinatorDropDownList = new SelectList(viewModel.Coordinators.OrderBy(s => s.CoordinatorId), "CoordinatorId", "StaffIdFullName");
            viewModel.UnitTypeTitleDropDownList = new SelectList(viewModel.UnitTypes.OrderBy(u => u.UnitTypeId), "UnitTypeId", "UnitTypeTitle");
        }


        /// <summary>
        /// Passes data from the view model to the entity model.
        /// </summary>
        /// <param name="viewModel">UnitBaseViewModel</param>
        /// <param name="entityModel">Unit</param>
        private void PopulateEntityModel(UnitBaseViewModel viewModel, Unit entityModel)
        {
            entityModel.unit_id = viewModel.UnitId;
            entityModel.title = viewModel.Title;
            entityModel.coordinator_id = viewModel.CoordinatorId;
            entityModel.credit_points = viewModel.CreditPoints;
            entityModel.unit_type_id = viewModel.UnitTypeId;
        }

        /// <summary>
        /// Passes data from the entity model to the view model.
        /// </summary>
        /// <param name="viewModel">UnitBaseViewModel</param>
        /// <param name="entityModel">Unit</param>
        private void PopulateViewModel(UnitBaseViewModel viewModel, Unit entityModel)
        {
            viewModel.UnitId = entityModel.unit_id;
            viewModel.Title = entityModel.title;
            viewModel.CoordinatorId = entityModel.coordinator_id;
            viewModel.CreditPoints = entityModel.credit_points;
            viewModel.UnitTypeId = entityModel.unit_type_id;

            viewModel.StaffFullName =
                $"{entityModel.Staff.firstname} " +
                $"{entityModel.Staff.surname}";

            viewModel.UnitTypeTitle = entityModel.UnitType.title;
        }

        /// <summary>
        /// Returns number of affected rows.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private int GetNumberOfAffectedRows(string id)
        {
            return _db.UnitEnrolments.Count(ue => ue.unit_id == id);
        }

        /// <summary>
        /// Implemented own cascade on delete,
        /// database not performing it on its own.
        /// </summary>
        /// <param name="id"></param>
        private void CascadeOnDelete(string id)
        {
            var unitEnrolments = _db.UnitEnrolments
                .Where(ue => ue.unit_id == id);

            foreach (UnitEnrolment x in unitEnrolments)
            {
                _db.UnitEnrolments.Remove(x);
            }
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
