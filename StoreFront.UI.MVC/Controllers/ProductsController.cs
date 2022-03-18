using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StoreFront.DATA.EF;
using StoreFront.UI.MVC.Utilities;
using StoreFront.UI.MVC.Models;

namespace StoreFront.UI.MVC.Controllers
{
    public class ProductsController : Controller
    {
        private DrumstoreEntities db = new DrumstoreEntities();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category).Include(p => p.Manufacturer).Include(p => p.StockStatu);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CatagorieID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ManufacturerID", "ManufacturerName");
            ViewBag.StockStatus = new SelectList(db.StockStatus, "StockStatusID", "StockStatusName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,Price,StockStatus,ManufacturerID,CatagorieID,Description,ProductImage")] Product product,
            HttpPostedFileBase productPicture)
        {
            if (ModelState.IsValid)
            {
                string file = "NoImage.png";

                if (productPicture != null)
                {
                    file = productPicture.FileName;

                    string ext = file.Substring(file.LastIndexOf('.'));

                    string[] goodExts = { ".jpeg", ".jpg", ".png", ".gif" };

                    if (goodExts.Contains(ext.ToLower()) && productPicture.ContentLength <= 4194304)
                    {
                        file = Guid.NewGuid() + ext;

                        string savePath = Server.MapPath("~/Content/imgstore/products");

                        Image convertedImage = Image.FromStream(productPicture.InputStream);

                        int maxImageSize = 500;

                        int maxThumbSize = 100;

                        ImageUtility.ResizeImage(savePath, file, convertedImage, maxImageSize, maxThumbSize);
                    }

                    product.ProductImage = file;
                }


                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CatagorieID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CatagorieID);
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ManufacturerID", "ManufacturerName", product.ManufacturerID);
            ViewBag.StockStatus = new SelectList(db.StockStatus, "StockStatusID", "StockStatusName", product.StockStatus);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CatagorieID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CatagorieID);
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ManufacturerID", "ManufacturerName", product.ManufacturerID);
            ViewBag.StockStatus = new SelectList(db.StockStatus, "StockStatusID", "StockStatusName", product.StockStatus);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,Price,StockStatus,ManufacturerID,CatagorieID,Description,ProductImage")] Product product,
            HttpPostedFileBase productPicture)
        {
            if (ModelState.IsValid)
            {
                string file = "NoImage.png";

                if (productPicture != null)
                {
                    file = productPicture.FileName;

                    string ext = file.Substring(file.LastIndexOf('.'));

                    string[] goodExts = { ".jpeg", ".jpg", ".png", ".gif" };

                    if (goodExts.Contains(ext.ToLower()) && productPicture.ContentLength <= 4194304)
                    {
                        file = Guid.NewGuid() + ext;

                        string savePath = Server.MapPath("~/Content/imgstore/products/");

                        Image convertedImage = Image.FromStream(productPicture.InputStream);

                        int maxImageSize = 500;

                        int maxThumbSize = 100;

                        ImageUtility.ResizeImage(savePath, file, convertedImage, maxImageSize, maxThumbSize);

                        if (product.ProductImage != null && product.ProductImage != "NoImage.png")
                        {
                            string path = Server.MapPath("~/Content/imgstore/products/");
                            ImageUtility.Delete(path, product.ProductImage);
                        }

                        product.ProductImage = file;
                    }


                }


                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CatagorieID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CatagorieID);
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ManufacturerID", "ManufacturerName", product.ManufacturerID);
            ViewBag.StockStatus = new SelectList(db.StockStatus, "StockStatusID", "StockStatusName", product.StockStatus);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
