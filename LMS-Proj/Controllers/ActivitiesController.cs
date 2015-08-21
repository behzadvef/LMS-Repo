using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LMS_Proj.Models;

namespace LMS_Proj.Controllers
{
    [Authorize]
    public class ActivitiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Activities
        //public ActionResult Index()
        //{
        //    var activities = db.Activities.Include(a => a.Attachment).Include(a => a.Groups).Include(a => a.TimeSheet);
        //    return View(activities.ToList());
        //}

        //// GET: Activities/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Activity activity = db.Activities.Find(id);
        //    if (activity == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(activity);

        // GET: Activities
        public ActionResult Index(string Name, int? GroupId, int? FileId)
        {

            //var activities = db.Activities.Include(a => a.Attachment).Include(a => a.Groups).Include(a => a.Timesheet);
            //return View(activities.ToList());
            var activities = from s in db.Activities.Include(s => s.Attachment).Include(s => s.Groups)
                             select s;


            if (!String.IsNullOrEmpty(Name))
            {
                activities = activities.Where(s => s.Name.Contains(Name));
                ViewBag.Title = Name;
            }

            if (GroupId != null && GroupId != 0)
            {
                activities = activities.Where(s => s.GroupId == GroupId);
            }
            var Groups = from gr in db.Groups
                         select gr;
            List<Group> group = new List<Group>();
            Group groupType = new Group();
            groupType.GroupID = 0;
            groupType.GroupName = "All";
            group.Add(groupType);

            group.AddRange(Groups.ToList());
            SelectList list;
            list = new SelectList(group, "GroupId", "GroupName");


            //ViewBag.GroupId = list
            ViewData["GroupId"] = list;


            return View(activities);


        }


        // GET: Activities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }


        // GET: Activities/Create
        public ActionResult Create()
        {
            ViewBag.FileId = new SelectList(db.Files, "FileId", "FileName");
            ViewBag.GroupId = new SelectList(db.Groups, "GroupID", "GroupName");
            ViewBag.scheduleId = new SelectList(db.Schedules, "scheduleId", "room");
            return View();
        }


        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ActivityId,Name,StartDate,EndDate,Description,GroupId,FileId,scheduleId")] Activity activity)
        {
            if (ModelState.IsValid)
            {

                //if (upload != null && upload.ContentLength > 0)
                //{
                //    var avatar = new File
                //    {
                //        FileName = System.IO.Path.GetFileName(upload.FileName),
                //        Type = FileType.uploadFile,
                //        FilePath = upload.F

                //    };
                //    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                //    {
                //        avatar.Content = reader.ReadBytes(upload.ContentLength);
                //    }
                //    Activity.Files = new List<File> { avatar };
                //}



                db.Activities.Add(activity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FileId = new SelectList(db.Files, "FileId", "FileName", activity.FileId);
            ViewBag.GroupId = new SelectList(db.Groups, "GroupID", "GroupName", activity.GroupId);
            ViewBag.scheduleId = new SelectList(db.Schedules, "scheduleId", "room", activity.scheduleId);
            return View(activity);
        }

        // GET: Activities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            ViewBag.FileId = new SelectList(db.Files, "FileId", "FileName", activity.FileId);
            ViewBag.GroupId = new SelectList(db.Groups, "GroupID", "GroupName", activity.GroupId);
            ViewBag.scheduleId = new SelectList(db.Schedules, "scheduleId", "room", activity.scheduleId);
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ActivityId,Name,StartDate,EndDate,Description,GroupId,FileId,scheduleId")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FileId = new SelectList(db.Files, "FileId", "FileName", activity.FileId);
            ViewBag.GroupId = new SelectList(db.Groups, "GroupID", "GroupName", activity.GroupId);
            ViewBag.scheduleId = new SelectList(db.Schedules, "scheduleId", "room", activity.scheduleId);
            return View(activity);
        }

        // GET: Activities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Activity activity = db.Activities.Find(id);
            db.Activities.Remove(activity);
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
