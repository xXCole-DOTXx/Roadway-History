using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Roadway_History.Models;

namespace Roadway_History.Controllers
{
    public class StatewidesController : Controller
    {
        private RoadWay_HistoryEntities db = new RoadWay_HistoryEntities();

        // GET: Statewides
        public ActionResult Index()
        {
            return View(db.Statewides.ToList());
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
