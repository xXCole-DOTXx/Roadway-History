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
    public class StatewidesController : Controller
    {
        private RoadWay_HistoryEntities db = new RoadWay_HistoryEntities();

        // GET: Statewides
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.IDSortParm = String.IsNullOrEmpty(sortOrder) ? "ID_desc" : "";
            ViewBag.countySortParm = sortOrder == "county" ? "county_desc" : "county";

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
                statewides = statewides.Where(s => s.COUNTY.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "ID_desc":
                    statewides = statewides.OrderByDescending(s => s.ID);
                    break;
                case "county":
                    statewides = statewides.OrderBy(s => s.COUNTY);
                    break;
                case "county_desc":
                    statewides = statewides.OrderByDescending(s => s.COUNTY);
                    break;
                default:
                    statewides = statewides.OrderBy(s => s.ID);
                    break;
            }

            int pageSize = 1000;
            int pageNumber = (page ?? 1);
            return View(statewides.ToPagedList(pageNumber, pageSize));
        }

        // GET: Statewides/Details/5
        public ActionResult Details(int? id)
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

        // GET: Statewides/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Statewides/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,COUNTY,SignSys,RouteNoOrigImport,RouteNo,SuppDes,LocalName,DateNumber,Method,OriginalL,OriginalD,Projects,Documents,Comments,District,Duplicate_OK,RightofWay,ReservedRoute,ReservedBy,ReservedDate,CP_WorkCompleted,Work_Comments,CP_ProjectNo,ReservedCat")] Statewide statewide)
        {
            if (ModelState.IsValid)
            {
                db.Statewides.Add(statewide);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(statewide);
        }

        // GET: Statewides/Edit/5
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
        public ActionResult Edit([Bind(Include = "ID,COUNTY,SignSys,RouteNoOrigImport,RouteNo,SuppDes,LocalName,DateNumber,Method,OriginalL,OriginalD,Projects,Documents,Comments,District,Duplicate_OK,RightofWay,ReservedRoute,ReservedBy,ReservedDate,CP_WorkCompleted,Work_Comments,CP_ProjectNo,ReservedCat")] Statewide statewide)
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
            db.Statewides.Remove(statewide);
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
