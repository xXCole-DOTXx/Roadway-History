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

namespace Roadway_History.Controllers
{
    public class DocumentsController : Controller
    {
        private RoadWay_HistoryEntities db = new RoadWay_HistoryEntities();

        // GET: Documents
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? statewideID)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.IDSortParm = String.IsNullOrEmpty(sortOrder) ? "ID_desc" : "";
            ViewBag.stateIDSortParm = sortOrder == "stateID" ? "state_desc" : "stateID";
            ViewBag.DateSortParm = sortOrder == "date" ? "date_desc" : "date";
            ViewBag.statewideID = statewideID;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var documents = from s in db.Documents
                            select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                documents = documents.Where(s => s.Order_Date.ToString().Contains(searchString)
                                               || s.Comment.Contains(searchString));
            }

            //IF INDEX IS LOADED FROM STATEWIDE ID
            if (statewideID != null)
            {
                documents = documents.Where(s => s.Statewide_ID == statewideID);
                //Get County
                var county = db.Statewides.Where(c => c.ID == statewideID)
                                          .Select(x => x.COUNTY);
                foreach (var name in county)
                    ViewBag.County = name;
                //Get RouteNo
                var route = db.Statewides.Where(c => c.ID == statewideID)
                                         .Select(x => x.RouteNo);
                foreach (var rt in route)
                    ViewBag.Route = rt;
            }

                switch (sortOrder)
            {
                case "ID_desc":
                    documents = documents.OrderByDescending(s => s.ID);
                    break;
                case "stateID":
                    documents = documents.OrderBy(s => s.Statewide_ID);
                    break;
                case "state_desc":
                    documents = documents.OrderByDescending(s => s.Statewide_ID);
                    break;
                case "date":
                    documents = documents.OrderBy(s => s.Order_Date);
                    break;
                case "date_desc":
                    documents = documents.OrderByDescending(s => s.Order_Date);
                    break;
                default:
                    documents = documents.OrderBy(s => s.ID);
                    break;
            }

            int pageSize = 100;
            int pageNumber = (page ?? 1);
            return View(documents.ToPagedList(pageNumber, pageSize));
        }

        // GET: Documents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // GET: Documents/Create
        public ActionResult Create(int? statewideID)
        {
            ViewBag.statewideID = statewideID;
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Statewide_ID,Doc_Type,Order_Date,Doc_Location,File_Contents,Comment,Source,Latitude,Longitude,Add_User,Date_Added")] Document document)
        {
            if (ModelState.IsValid)
            {

                var userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                document.Add_User = userName;
                document.Date_Added = DateTime.Today;
                db.Documents.Add(document);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(document);
        }

        // GET: Documents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Statewide_ID,Doc_Type,Order_Date,Doc_Location,File_Contents,Comment,Source,Latitude,Longitude")] Document document)
        {
            if (ModelState.IsValid)
            {
                db.Entry(document).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(document);
        }

        // GET: Documents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Document document = db.Documents.Find(id);
            db.Documents.Remove(document);
            db.SaveChanges();
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
    }
}
