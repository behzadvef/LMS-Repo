using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LMS_Proj.Models;
using System.IO;

namespace LMS_Proj.Controllers
{
    [Authorize]
    public class GroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Groups
        //public ActionResult Index()
        //{
        //    return View(db.Groups.ToList());
        //}

        public ActionResult Index(string GroupName)
        {
            var groups = from gr in db.Groups
                         select gr;
            if(!String.IsNullOrWhiteSpace(GroupName))
            {
                groups = groups.Where(gr => gr.GroupName.Contains(GroupName));
                ViewBag.GroupNames = GroupName;
            }
            //if (!string.IsNullOrWhiteSpace(GroupDescription))
            //{
            //    groups = from g in db.Groups
            //             select g;

            //}
          
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
               return View(groups);
          
        }


        //public ActionResult Index(string Name, int? GroupId, int? FileId)
        //{

        //    //var activities = db.Activities.Include(a => a.Attachment).Include(a => a.Groups).Include(a => a.Timesheet);
        //    //return View(activities.ToList());
        //    var activities = from s in db.Activities.Include(s => s.Attachment).Include(s => s.Groups)
        //                     select s;


        //    if (!String.IsNullOrEmpty(Name))
        //    {
        //        activities = activities.Where(s => s.Name.Contains(Name));
        //        ViewBag.Title = Name;
        //    }

        //    if (GroupId != null && GroupId != 0)
        //    {
        //        activities = activities.Where(s => s.GroupId == GroupId);
        //    }
        //    var Groups = from gr in db.Groups
        //                 select gr;
        //    List<Group> group = new List<Group>();
        //    Group groupType = new Group();
        //    groupType.GroupID = 0;
        //    groupType.GroupName = "All";
        //    group.Add(groupType);

        //    group.AddRange(Groups.ToList());
        //    SelectList list;
        //    list = new SelectList(group, "GroupId", "GroupName");


        //    //ViewBag.GroupId = list
        //    ViewData["GroupId"] = list;


        //    return View(activities);




        // GET: Groups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // GET: Groups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GroupID,MaxMembers,GroupName,ClassLocation,GroupDescription")] Group group, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var uploadFile = new BinaryWriter(System.IO.File.Open(Server.MapPath("/Documents/Files/") + upload.FileName, System.IO.FileMode.Create));
                    using (var reader = new BinaryReader(upload.InputStream))
                    {
                        uploadFile.Write(reader.ReadBytes(upload.ContentLength));
                    }
                }
                db.Groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(group);
        }

        // GET: Groups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GroupID,MaxMembers,GroupName,ClassLocation,GroupDescription")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        // GET: Groups/Delete/5



        public ActionResult Delete(int? id, bool? ErrorinSaveChanges = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ErrorinSaveChanges.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Failed to Delete. Try again or Contact to System Administrator";
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }


        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Group group = db.Groups.Find(id);
        //    if (group == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(group);
        //}

        //// POST: Groups/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Group group = db.Groups.Find(id);
        //    db.Groups.Remove(group);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}







        // GET: Groups/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == 0)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    Group group = db.Groups.Find(id);

        //    if (group == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    if (group.Users.Any())
        //    {
        //        return View("GroupNotEmpty");
        //    }

        //    //            db.Groups.Remove(group);
        //    //            db.SaveChanges();

        //    return View(group);
        //}

        //// POST: Groups/Delete/5

//        [HttpPost]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Group group = db.Groups.Find(id);


                if (group != null && !group.Users.Any())
                {

                    db.Groups.Remove(group);
                    db.SaveChanges();
                }
                //else
                //{
                //    ViewBag.ErrorMessage = "Either the group was not empty  or was not found!";

                //}

            }
            catch (DataException /*DataEx*/)
            {
                return RedirectToAction("Delete", new { id = id, errorinsavechanges = true });
            }
            return RedirectToAction("Index");
        }


        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    if (id == 0)
        //    {

        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    Group group = db.Groups.Find(id);

        //    if (group != null && !group.Users.Any())
        //    {
        //        db.Groups.Remove(group);
        //        db.SaveChanges();
        //    }

        //    return RedirectToAction("Index");
        //}


        // GET: GroupNotEmpty/Delete/5
        //       public ActionResult GroupNotEmpty()
        //       {
        //           //if (id == null)
        //           //{
        //           //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //           //}
        //           //Activity activity = db.Activities.Find(id);
        //           //if (activity == null)
        //           //{
        //           //    return HttpNotFound();
        //           //}

        //           return RedirectToAction("Delete");
        ////           return View("Delete");
        //       }

        //       // POST: Activities/Delete/5
        //       [HttpPost, ActionName("GroupNotEmpty")]
        //       [ValidateAntiForgeryToken]
        //       public ActionResult GroupNotEmptyConfirmed()
        //       {
        //           ////Activity activity = db.Activities.Find(id);
        //           ////db.Activities.Remove(activity);
        //           ////db.SaveChanges();
        //           return RedirectToAction("Delete");
        //       }


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

