using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LMS_Proj.Models;
//using System.IO;
//using System.IO;

//Second Branch
namespace LMS_Proj.Controllers
{
    public class FilesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Files
        public ActionResult Index()
        {
           
            var files = db.Files.Include(f => f.Owner);
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
            return View(file);
        }

        // GET: Files/Create
        public ActionResult Create()
        {
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: Files/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FileId,Type,SubmissionDate,FileName,FilePath,FileLink,Comment,ReadByReciever,uploadFile,ApplicationUserId,GroupId")] File file,IEnumerable<HttpPostedFileBase> uploadfiles)
        {
            if (ModelState.IsValid)
            {
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

                foreach(var Upload in uploadfiles)
                {
                    var allowedExtensions = new[] { ".doc", ".xlsx", ".txt", ".jpeg" };
                    var extension = System.IO.Path.GetExtension(file.FileName);
                    if (!allowedExtensions.Contains(extension))
                    {
                        // Not allowed
                    }
                  // if(Upload.ContentLength > 0)
                   //{
                       var fileName = System.IO.Path.GetFileName(file.FileName);
                       var path = System.IO.Path.Combine(Server.MapPath("~/Documents/Files/"), fileName);
                       Upload.SaveAs(path);
                  // }
                }

                db.Files.Add(file);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", file.ApplicationUserId);
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
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", file.ApplicationUserId);
            return View(file);
        }

        // POST: Files/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FileId,Type,SubmissionDate,FileName,FilePath,FileLink,Comment,ReadByReciever,uploadFile,ApplicationUserId,GroupId")] File file )
        {
            if (ModelState.IsValid)
            {
                db.Entry(file).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", file.ApplicationUserId);
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


        //[HttpPost]
        //public ActionResult UploadFile(string uploadFiles, HttpPostedFileBase file)
        //{
        //    //string path = @"C:\Users\User\Documents\Visual Studio 2013\Projects\LMS-Proj\LMS-Proj-Group\LMS-Proj\Documents\Files" + uploadFiles;
        //    //if (file != null)
        //    //    file.SaveAs(path);
        //    //return RedirectToAction("Index");

        //}

        [HttpPost, ActionName("DeleteFile")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFile(string filepdfName)
        {
            var fileName = "";
                fileName = filepdfName;
                string fullPath = Request.MapPath("~/Documents/Files/"
                + fileName);
 
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                    //Session["DeleteSuccess"] = "Yes";
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
    }
}
