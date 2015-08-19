using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LMS_Proj.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace LMS_Proj.Controllers
{
    [Authorize]
    public class FilesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Files
        public ActionResult Index()
        {
            var files = db.Files.Include(f => f.Owner).Include(f => f.Receiver);
            return View(files.ToList());
        }

        // GET: Files/Details/5
        public ActionResult Details(int? id)
        {
 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File file = db.Files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }

 //           var gr = file.Groups; //db.Files.Include(g => g.Groups);

            return View(file);
        }


        private SelectList AddFirstItem(SelectList list)
        {
            List<SelectListItem> _list = list.ToList();
            _list.Insert(0, new SelectListItem() { Value = null, Text = "   " });
            return new SelectList((IEnumerable<SelectListItem>)_list, "Value", "Text");
        }

        // GET: Files/Create
        public ActionResult Create()
        {

            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            ApplicationUser user = userManager.FindByNameAsync(User.Identity.Name).Result;

            if (user == null)
                return View();

            ViewBag.OwnerId = new SelectList(db.Users.Where(u => u.Id == user.Id), "Id", "FirstName");
            ViewBag.ReceiverId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.GroupId = AddFirstItem(new SelectList(db.Groups, "GroupID", "GroupName"));

            return View();
        }

        // POST: Files/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FileId,Type,SubmissionDate,FileName,FilePath,FileLink,Comment,ReadByReciever,ReceiverId,OwnerId, Groups ")] File file)
        {
            if (ModelState.IsValid)
            {
                file.SubmissionDate = DateTime.Now;
                db.Files.Add(file);
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            //ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", file.OwnerId);
            //ViewBag.ReceiverId = new SelectList(db.Users, "Id", "FirstName", file.ReceiverId);
            //ViewBag.GroupId = new SelectList(db.Groups, "GroupID", "GroupName", group.GroupID);
            return View(file);
        }

        // GET: Files/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            File file = db.Files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }

            ViewBag.OwnerId = new SelectList(db.Users.Where(u => u.Id == file.OwnerId), "Id", "FirstName", file.OwnerId);
            ViewBag.ReceiverId = new SelectList(db.Users, "Id", "FirstName", file.ReceiverId);
            ViewBag.GroupId = AddFirstItem(new SelectList(db.Groups, "GroupID", "GroupName"));
            return View(file);
        }

        // POST: Files/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FileId,Type,SubmissionDate,FileName,FilePath,FileLink,Comment,ReadByReciever,ReceiverId,OwnerId, Groups")] File file)
        {
            if (ModelState.IsValid)
            {
                db.Entry(file).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", file.OwnerId);
            ViewBag.ReceiverId = new SelectList(db.Users, "Id", "FirstName", file.ReceiverId);
            return View(file);
        }

        // GET: Files/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File file = db.Files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }

        // POST: Files/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            File file = db.Files.Find(id);
            db.Files.Remove(file);
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
