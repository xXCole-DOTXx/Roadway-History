using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Roadway_History.Models;
using PagedList;
using System.Security.Principal;
using System.Security;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Dynamic;

namespace Roadway_History.Controllers
{
    public class StatewidesController : Controller
    {
        //public static bool IsInGroup(string groupName)
        //{
        //    var myIdentity = GetUserIDWithDomain();
        //}
        public static WindowsIdentity GetUserIdWithDomain()
        {
            var myIdentity = WindowsIdentity.GetCurrent();
            return myIdentity;
        }

        public static string GetUserId()
        {
            var id = GetUserIdWithDomain().Name.Split('\\');
            return id[1];
        }


        private RoadWay_HistoryEntities db = new RoadWay_HistoryEntities();

        // GET: Statewides
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
     
            ViewBag.CurrentSort = sortOrder;
            ViewBag.IDSortParm = String.IsNullOrEmpty(sortOrder) ? "ID" : "";
            ViewBag.countySortParm = sortOrder == "county" ? "county_desc" : "county";
            ViewBag.routeSortParm = sortOrder == "route" ? "route_desc" : "route";
            ViewBag.SignSortParm = sortOrder == "sign" ? "sign_desc" : "sign";
            ViewBag.SuppSortParm = sortOrder == "supp" ? "supp_desc" : "supp";
            ViewBag.SuppSortParm = sortOrder == "supp" ? "supp_desc" : "supp";
            ViewBag.LocalNameSortParm = sortOrder == "local" ? "local_desc" : "local";
            ViewBag.StatusSortParm = sortOrder == "status" ? "status_desc" : "status";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var statewides = from s in db.Statewides
                            select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                statewides = statewides.Where(s => s.COUNTY.Contains(searchString)
                                             || s.RouteNo.ToString().Contains(searchString)
                                             || s.LocalName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "ID":
                    statewides = statewides.OrderBy(s => s.ID);
                    break;
                case "county":
                    statewides = statewides.OrderBy(s => s.COUNTY);
                    break;
                case "county_desc":
                    statewides = statewides.OrderByDescending(s => s.COUNTY);
                    break;
                case "route":
                    statewides = statewides.OrderBy(s => s.RouteNo);
                    break;
                case "route_desc":
                    statewides = statewides.OrderByDescending(s => s.RouteNo);
                    break;
                case "sign":
                    statewides = statewides.OrderBy(s => s.SignSys);
                    break;
                case "sign_desc":
                    statewides = statewides.OrderByDescending(s => s.SignSys);
                    break;
                case "supp":
                    statewides = statewides.OrderBy(s => s.SuppDes);
                    break;
                case "supp_desc":
                    statewides = statewides.OrderByDescending(s => s.SuppDes);
                    break;
                case "local":
                    statewides = statewides.OrderBy(s => s.LocalName);
                    break;
                case "local_desc":
                    statewides = statewides.OrderByDescending(s => s.LocalName);
                    break;
                default:
                    statewides = statewides.OrderByDescending(s => s.ID);
                    break;
            }

            int pageSize = 100;
            int pageNumber = (page ?? 1);
            return View(statewides.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ReservedRoutes(string sortOrder, string currentFilter, string searchString, int? page, string countyFilter, string countySearch)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.IDSortParm = String.IsNullOrEmpty(sortOrder) ? "ID" : "";
            ViewBag.countySortParm = sortOrder == "county" ? "county_desc" : "county";
            ViewBag.routeSortParm = sortOrder == "route" ? "route_desc" : "route";
            ViewBag.SignSortParm = sortOrder == "sign" ? "sign_desc" : "sign";
            ViewBag.SuppSortParm = sortOrder == "supp" ? "supp_desc" : "supp";
            ViewBag.LocalNameSortParm = sortOrder == "local" ? "local_desc" : "local";
            ViewBag.StatusSortParm = sortOrder == "status" ? "status_desc" : "status";

            //Search string
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            //County Search string
            if (countySearch != null)
            {
                page = 1;
            }
            else
            {
                countySearch = countyFilter;
            }

            ViewBag.CurrentFilter = searchString;
            ViewBag.CountyFilter = countySearch;

            var statewides = from s in db.Statewides
                             select s;

            statewides = statewides.Where(s => s.ReservedRoute == true);

            if (!String.IsNullOrEmpty(searchString))
            {
                statewides = statewides.Where(s => s.District.ToString().Equals(searchString));

                if (!String.IsNullOrEmpty(countySearch))
                {
                    statewides = statewides.Where(s => s.COUNTY.Contains(countySearch));
                }

            }

            switch (sortOrder)
            {
                case "ID":
                    statewides = statewides.OrderBy(s => s.ID);
                    statewides = statewides.OrderBy(s => s.ID);
                    break;
                case "county":
                    statewides = statewides.OrderBy(s => s.COUNTY);
                    break;
                case "county_desc":
                    statewides = statewides.OrderByDescending(s => s.COUNTY);
                    break;
                case "route":
                    statewides = statewides.OrderBy(s => s.RouteNo);
                    break;
                case "route_desc":
                    statewides = statewides.OrderByDescending(s => s.RouteNo);
                    break;
                case "sign":
                    statewides = statewides.OrderBy(s => s.SignSys);
                    break;
                case "sign_desc":
                    statewides = statewides.OrderByDescending(s => s.SignSys);
                    break;
                case "supp":
                    statewides = statewides.OrderBy(s => s.SuppDes);
                    break;
                case "supp_desc":
                    statewides = statewides.OrderByDescending(s => s.SuppDes);
                    break;
                case "local":
                    statewides = statewides.OrderBy(s => s.LocalName);
                    break;
                case "local_desc":
                    statewides = statewides.OrderByDescending(s => s.LocalName);
                    break;
                default:
                    statewides = statewides.OrderBy(s => s.ID);
                    break;
            }

            int pageSize = 100;
            int pageNumber = (page ?? 1);
            return View(statewides.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult AdvancedSearch(string sortOrder, string DistrictFilter, string DistrictString, int? page, string CountyFilter, string countySearch, string routeFilter, string routeSearch, string signFilter, string signSearch)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.IDSortParm = String.IsNullOrEmpty(sortOrder) ? "ID" : "";
            ViewBag.countySortParm = sortOrder == "county" ? "county_desc" : "county";
            ViewBag.routeSortParm = sortOrder == "route" ? "route_desc" : "route";
            ViewBag.SignSortParm = sortOrder == "sign" ? "sign_desc" : "sign";
            ViewBag.SuppSortParm = sortOrder == "supp" ? "supp_desc" : "supp";
            ViewBag.LocalNameSortParm = sortOrder == "local" ? "local_desc" : "local";
            ViewBag.StatusSortParm = sortOrder == "status" ? "status_desc" : "status";


            System.Diagnostics.Debug.WriteLine("The district was: " + DistrictString);
            System.Diagnostics.Debug.WriteLine("The county was: " + countySearch);
            System.Diagnostics.Debug.WriteLine("The sign was: " + signSearch);
            System.Diagnostics.Debug.WriteLine("The route was: " + routeSearch);
            ViewBag.DistrictFilter = DistrictString;
            ViewBag.CountyFilter = countySearch;
            ViewBag.signFilter = signSearch;
            ViewBag.routeFilter = routeSearch;

            var statewides = from s in db.Statewides
                             select s;

            if (!String.IsNullOrEmpty(DistrictString))
            {
                statewides = statewides.Where(s => s.District.ToString().Contains(DistrictString));

                if (!String.IsNullOrEmpty(countySearch))
                {
                    statewides = statewides.Where(s => s.COUNTY.Contains(countySearch.ToUpper()));

                    if (!String.IsNullOrEmpty(signSearch))
                    {
                        statewides = statewides.Where(s => s.SignSys.ToString().Contains(signSearch));

                        if (!String.IsNullOrEmpty(routeSearch))
                        {
                            statewides = statewides.Where(s => s.RouteNo.ToString().Contains(routeSearch));
                        }
                    }
                }
            }


            switch (sortOrder)
            {
                case "ID":
                    statewides = statewides.OrderBy(s => s.ID);
                    break;
                case "county":
                    statewides = statewides.OrderBy(s => s.COUNTY);
                    break;
                case "county_desc":
                    statewides = statewides.OrderByDescending(s => s.COUNTY);
                    break;
                case "route":
                    statewides = statewides.OrderBy(s => s.RouteNo);
                    break;
                case "route_desc":
                    statewides = statewides.OrderByDescending(s => s.RouteNo);
                    break;
                case "sign":
                    statewides = statewides.OrderBy(s => s.SignSys);
                    break;
                case "sign_desc":
                    statewides = statewides.OrderByDescending(s => s.SignSys);
                    break;
                case "supp":
                    statewides = statewides.OrderBy(s => s.SuppDes);
                    break;
                case "supp_desc":
                    statewides = statewides.OrderByDescending(s => s.SuppDes);
                    break;
                case "local":
                    statewides = statewides.OrderBy(s => s.LocalName);
                    break;
                case "local_desc":
                    statewides = statewides.OrderByDescending(s => s.LocalName);
                    break;
                default:
                    statewides = statewides.OrderBy(s => s.ID);
                    break;
            }

            int pageSize = 100;
            int pageNumber = (page ?? 1);
            return View(statewides.ToPagedList(pageNumber, pageSize));
        }

        // GET: Statewides/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.ID = id;
            var today = DateTime.Today;
            var todayAsString = today.ToString("MM/dd/yyyy");
            ViewBag.Today = todayAsString;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Statewide statewide = db.Statewides.Find(id);

            var documents = db.Documents.Where(s => s.Statewide_ID == id).ToList();

            var model = new ViewModel
            {
                Statewide = statewide,
                Document = documents
            };

            if (statewide == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [Authorize(Users = "EXECUTIVE\\E072340, EXECUTIVE\\E096752, EXECUTIVE\\E089025, EXECUTIVE\\E107097")]
        // GET: Statewides/Create
        public ActionResult Create()
        {
            var model = new Statewide();
            return View(model);
        }

        // POST: Statewides/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,COUNTY,SignSys,RouteNoOrigImport,RouteNo,SuppDes,LocalName,DateNumber,Method,OriginalL,OriginalD,Projects,Comments,District,Duplicate_OK,RightofWay,ReservedRoute,ReservedBy,ReservedDate,CP_WorkCompleted,Work_Comments,CP_ProjectNo,ReservedCat,CurrentStatus,Add_User,Date_Added")] Statewide statewide)
        {
            if (ModelState.IsValid)
            {
                statewide.Documents = null;
                statewide.Add_User = User.Identity.Name;
                statewide.Date_Added = DateTime.Now;
                try
                {
                    db.Statewides.Add(statewide);
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        if (ex.InnerException != null)
                        {
                            Response.Write(ex.InnerException.Message);
                        }
                    }
                }
                    return RedirectToAction("Error", new { error = 1, name = User.Identity.Name });
                }


            int lastAddedID = db.Statewides.Max(item => item.ID);
                return RedirectToAction("Create", "Documents", new { statewideID = lastAddedID});
            }

            return View(statewide);
        }

        // GET: Statewides/Edit/5
        [Authorize(Users = "EXECUTIVE\\E072340, EXECUTIVE\\E096752, EXECUTIVE\\E089025, EXECUTIVE\\E107097")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Statewide statewide = db.Statewides.Find(id);
            if (statewide == null)
            {
                return HttpNotFound();
            }
            return View(statewide);
        }

        // POST: Statewides/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,COUNTY,SignSys,RouteNoOrigImport,RouteNo,SuppDes,LocalName,DateNumber,Method,OriginalL,OriginalD,Projects,Documents,Comments,District,Duplicate_OK,RightofWay,ReservedRoute,ReservedBy,ReservedDate,CP_WorkCompleted,Work_Comments,CP_ProjectNo,ReservedCat,CurrentStatus")] Statewide statewide)
        {
            if (ModelState.IsValid)
            {
                db.Entry(statewide).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(statewide);
        }

        // GET: Statewides/Delete/5
        [Authorize(Users = "EXECUTIVE\\E072340, EXECUTIVE\\E096752, EXECUTIVE\\E089025, EXECUTIVE\\E107097")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Statewide statewide = db.Statewides.Find(id);
            if (statewide == null)
            {
                return HttpNotFound();
            }
            return View(statewide);
        }

        // POST: Statewides/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Statewide statewide = db.Statewides.Find(id);
            try
            {
                db.Statewides.Remove(statewide);
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                return RedirectToAction("Error", new { error = 2} );
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Error(int error, string userName)
        {
            if(error == 1)
            {
                ViewBag.Error = "";
            }
            else
            {
                ViewBag.Error = "You are trying to delete a Statewide record that has Documents attached to it. Please delete the Documents first.";
            }
            if (!String.IsNullOrEmpty(userName))
            {
                ViewBag.Name = "Cole";
            }
            return View("Error");
        }
    }
}
