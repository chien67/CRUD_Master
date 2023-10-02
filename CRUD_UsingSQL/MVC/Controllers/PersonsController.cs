using MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class PersonsController : Controller
    {
        private Person_CRUDEntities db = new Person_CRUDEntities();

        // GET: Persons
        public ActionResult Index()
        {
            ViewBag.person = db.Persons.ToList();
            return View();
        }
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(Person person, HttpPostedFileBase imageFile)
        {
            if(imageFile!= null && imageFile.ContentLength > 0)
            {
                byte[] imageData = new byte[imageFile.ContentLength];
                imageFile.InputStream.Read(imageData, 0, imageFile.ContentLength);

                // Lưu dữ liệu hình ảnh vào trường Avatar của đối tượng Person
                person.Avatar = imageData;
            }
            //person.Avatar = new byte[imageFile.ContentLength];
            //imageFile.InputStream.Read(person.Avatar, 0, imageFile.ContentLength);

            db.Persons.Add(person);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            // Lấy thông tin Person từ cơ sở dữ liệu
            var person = db.Persons.Find(id);
            return View("Edit", person);
        }
        [HttpPost]
        public ActionResult Edit(Person person, HttpPostedFileBase newImageFile)
        {
            if (ModelState.IsValid)
            {
                if (newImageFile != null && newImageFile.ContentLength > 0)
                {
                    // Đọc dữ liệu từ tệp hình ảnh mới và cập nhật dữ liệu hình ảnh của Person
                    byte[] newImageData = new byte[newImageFile.ContentLength];
                    newImageFile.InputStream.Read(newImageData, 0, newImageFile.ContentLength);
                    //person.Avatar = newImageData;
                }

                // Cập nhật thông tin khác của Person
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            var person = db.Persons.Find(id);
            db.Persons.Remove(person);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            Person person = db.Persons.Find(id);
            // Lấy dữ liệu hình ảnh từ đối tượng mô hình (assumption: model.ImageData là mảng byte)
            byte[] imageData = person.Avatar;
            // Trả về view và chuyển dữ liệu hình ảnh qua ViewBag hoặc ViewModel
            ViewBag.ImageData = imageData;

            ViewBag.Gender = person.Gender; // model.Gender là kiểu bool

            if (person == null)
            {
                return HttpNotFound();
            }
            return View("Details", person);
        }

    }
}