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
using System.Collections.ObjectModel;

namespace LMS_Proj.Controllers
{
    [Authorize]
    public class FilesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Files
        public ActionResult Index(int? GroupId)
        {


            var currentUserId = User.Identity.GetUserId();

            var currentUser = db.Users.Find(currentUserId);

            var currentGroupId = currentUser.GroupId;

            List<File> FileList;
            List<Group> group;

            if (User.IsInRole("admin"))
            {
                FileList = db.Files.ToList();
                group = db.Groups.ToList();
            }
            else
            {
                FileList = db.Files.Where(f => f.Groups.Any(g => g.GroupID == currentGroupId)).ToList();
                group = (db.Groups.Where(g => g.GroupID == currentGroupId)).ToList();
            }

            if (GroupId != null)
            {
                FileList = FileList.Where(f => f.Groups.Any(g => g.GroupID == GroupId)).ToList();
            }

            if (User.IsInRole("admin"))
            {
                ViewBag.GroupId = AddFirstItem(new SelectList(group, "GroupID", "GroupName"));
            }
            else
            {
                ViewBag.GroupId = new SelectList(group, "GroupID", "GroupName");
            }

            //            List<File> files ;
            //            var groups = db.Groups.Where(g=>g.GroupID == currentGroupId);
            //            var group = groups.First();

            //            if (User.IsInRole("admin"))
            //            {
            //                files = db.Files.Include(f => f.Owner).Include(f => f.Receiver).ToList();
            //            }
            //            else
            //            {
            ////                files = db.Files.Include(f => f.Owner).Include(f => f.Receiver).Where(f=>(f.Owner== User) || f.Groups.Where(h=>h.GroupID == currentGroupId)).ToList();
            //            }


            return View(FileList);
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
            ViewBag.ReceiverId = AddFirstItem(new SelectList(db.Users, "Id", "FirstName"));
            ViewBag.GroupId = AddFirstItem(new SelectList(db.Groups, "GroupID", "GroupName"));

            return View();
        }

        // POST: Files/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FileId,Type,SubmissionDate,FileName,FilePath,FileLink,Comment,ReadByReciever,ReceiverId,OwnerId, Groups ")] File file, HttpPostedFileBase uploadfiles, int? GroupId)
        {
            if (ModelState.IsValid)
            {
                if (uploadfiles != null && uploadfiles.ContentLength > 0)
                {
                    var uploadFile = new System.IO.BinaryWriter(System.IO.File.Open(Server.MapPath("/Documents/Files/") + uploadfiles.FileName, System.IO.FileMode.Create));
                    using (var reader = new System.IO.BinaryReader(uploadfiles.InputStream))
                    {
                        uploadFile.Write(reader.ReadBytes(uploadfiles.ContentLength));
                    }
                }
                //try
                //{
                //    foreach (HttpPostedFileBase Upload in uploadfiles)
                //    {
                //        string filename = System.IO.Path.GetFileName(file.FileName);
                //        Upload.SaveAs(Server.MapPath("~/Documents/Files/" + filename));
                //        string filepathtosave = "/Documents/Files/" + filename;
                //    }
                //    ViewBag.Message = "File Uploaded successfully.";


                //}
                //catch
                //{
                //    ViewBag.Message = "Error While uploading the files.";
                //}

                // foreach (var Upload in uploadfiles)
                //  {
                //var allowedExtensions = new[] { ".doc", ".xlsx", ".txt", ".jpeg" };
                //var extension = System.IO.Path.GetExtension(file.FileName);
                //if (!allowedExtensions.Contains(extension))
                //{
                //    // Not allowed
                //}
                // if(uploadfiles.ContentLength > 0)
                //{
                //var fileName = System.IO.Path.GetFileName(file.FileName);
                //var path = System.IO.Path.Combine(Server.MapPath("~/Documents/Files"), fileName);
                //uploadfiles.SaveAs(path);
                // }
                //  }
                //if (Request.Files != null)
                //{
                //    foreach (string requestFile in Request.Files)
                //    {
                //  uploadfiles = Request.Files[requestFile];
                //        if (uploadfiles.ContentLength > 0)
                //        {
                //            string fileName = System.IO.Path.GetFileName(file.FileName);
                //            string directory = Server.MapPath("~/Documents/Files/");
                //            if (!System.IO.Directory.Exists(directory))
                //            {
                //                System.IO.Directory.CreateDirectory(directory);
                //            }
                //            string path = System.IO.Path.Combine(directory, fileName);
                //            uploadfiles.SaveAs(path);
                //        }
                //    }
                //}

                var tempUserId = User.Identity.GetUserId();
                var tempUser = db.Users.Where(u => u.Id == tempUserId).First();
                file.Owner = tempUser;
                var grp = db.Groups.Find(GroupId);
                if (grp != null)
                {
                    file.Groups = db.Groups.Where(g => g.GroupID == GroupId).ToList();
                    //file.Groups = new Collection<Group>();
                    //file.Groups.Add(grp);
                }


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
            //    var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            //    var userManager = new UserManager<ApplicationUser>(store);
            //    ApplicationUser user = userManager.FindByNameAsync(User.Identity.Name).Result;
            //    User.Identity.GetUserId();

            //    if (user == null)
            //        return View();



            File file = db.Files.Find(id);

            if (!(User.Identity.GetUserId() == file.OwnerId || User.IsInRole("admin")))
            {
                // Error message 
                return RedirectToAction("Index");
            }

            if (file != null)
            {
                var act = file.Activities;
                foreach (Activity a in act)
                {
                    a.FileId = null;
                }
                db.SaveChanges();

                db.Files.Remove(file);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        //[HttpPost]
        //public ActionResult UploadFile(string uploadFiles, HttpPostedFileBase file)
        //{
        //    //string path = @"C:\Users\User\Documents\Visual Studio 2013\Projects\LMS-Proj\LMS-Proj-Group\LMS-Proj\Documents\Files" + uploadFiles;
        //    //if (file != null)
        //    //    file.SaveAs(path);
        //    //return RedirectToAction("Index");

        //}

        //[HttpPost, ActionName("DeleteFile")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteFile(string filepdfName)
        //{
        //    var fileName = "";
        //    fileName = filepdfName;
        //    string fullPath = Request.MapPath("~/Documents/Files/"
        //    + fileName);

        //    if (System.IO.File.Exists(fullPath))
        //    {
        //        System.IO.File.Delete(fullPath);
        //        //Session["DeleteSuccess"] = "Yes";
        //    }
        //    return RedirectToAction("Index");
        //}




















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
