using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DealineMVC.Models;

namespace DealineMVC.Controllers
{
    [Authorize(Roles = "QuanTri")]
    public class QuanLySanPhamController : Controller
    {
        // GET: QuanLySanPham
        QuanLyBanHangEntities3 db = new QuanLyBanHangEntities3();

        public ActionResult Index()
        {

            return View(db.SanPhams.Where(n => n.DaXoa == 0));
        }

        [HttpGet]
        public ActionResult TaoMoi()
        {
            //load drodowlist nha cung cap va drop loai sp , ma nsx
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult TaoMoi(HttpPostedFileBase[] HinhAnh, SanPham sp)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);
            int loi = 0;
            for (int i = 0; i < HinhAnh.Count(); i++)
            {
                if (HinhAnh[i] != null)
                {
                    //ktra noi dung hinh anh
                    if (HinhAnh[i].ContentLength > 0)
                    {
                        //ktra dinh dang hinh anh
                        if (HinhAnh[i].ContentType != "image/jpeg" && HinhAnh[i].ContentType != "image/png" && HinhAnh[i].ContentType != "image/jpg")
                        {
                            ViewBag.upload = "Hình ảnh" + i + "không hợp lệ <br/>";
                            loi++;
                        }
                        else
                        {
                            //ktra hinh anh ton tai
                            var fileName = Path.GetFileName(HinhAnh[0].FileName);
                            var path = Path.Combine(Server.MapPath("~/Content/HinhSP"), fileName);
                            // neu hinh anh trongh thu muc co r thi xuat ra thong bao
                            if (System.IO.File.Exists(path))
                            {
                                ViewBag.upload = "Hình ảnh " + i + "đã tồn tại <br/>";
                                loi++;
                            }
                        }
                    }
                }



            }
            if (loi > 0)
            {
                return View(sp);
            }
            sp.HinhAnh = HinhAnh[0].FileName;
            sp.HinhAnh = HinhAnh[1].FileName;
            sp.HinhAnh = HinhAnh[2].FileName;
            sp.HinhAnh = HinhAnh[3].FileName;
            sp.HinhAnh = HinhAnh[4].FileName;
            //ktra hinh anh ton tai trong csdl chua
            for (int i = 0; i < HinhAnh.Count(); i++)
            {
                if (HinhAnh[i].ContentLength > 0)
                {
                    var fileName = Path.GetFileName(HinhAnh[i].FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/HinhSP"), fileName);
                    // neu hinh anh trongh thu muc co r thi xuat ra thong bao
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.upload = "Hình đã tồn tại";
                        return View();
                    }
                    else
                    {
                        //lay ha dua vao thu muc
                        HinhAnh[i].SaveAs(path);
                        sp.HinhAnh = fileName;
                    }
                    db.SanPhams.Add(sp);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult ChinhSua(int id)
        {
            var sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);

            //lay sp can chinh sua vao id
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);
            return View(sp);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(HttpPostedFileBase[] HinhAnh, SanPham model)
        {
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", model.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", model.MaNSX);
            int loi = 0;
            for (int i = 0; i < HinhAnh.Count(); i++)
            {
                if (HinhAnh[i] != null)
                {
                    //ktra noi dung hinh anh
                    if (HinhAnh[i].ContentLength > 0)
                    {
                        //ktra dinh dang hinh anh
                        if (HinhAnh[i].ContentType != "image/jpeg" && HinhAnh[i].ContentType != "image/png" && HinhAnh[i].ContentType != "image/jpg")
                        {
                            ViewBag.upload = "Hình ảnh" + i + "không hợp lệ <br/>";
                            loi++;
                        }
                        else
                        {
                            //ktra hinh anh ton tai
                            var fileName = Path.GetFileName(HinhAnh[0].FileName);
                            var path = Path.Combine(Server.MapPath("~/Content/HinhSP"), fileName);
                            // neu hinh anh trongh thu muc co r thi xuat ra thong bao
                            if (System.IO.File.Exists(path))
                            {
                                ViewBag.upload = "Hình ảnh " + i + "đã tồn tại <br/>";
                                loi++;
                            }
                        }
                    }
                }



            }
            if (loi > 0)
            {
                return View(model);
            }
            model.HinhAnh = HinhAnh[0].FileName;
            model.HinhAnh = HinhAnh[1].FileName;
            model.HinhAnh = HinhAnh[2].FileName;
            model.HinhAnh = HinhAnh[3].FileName;
            model.HinhAnh = HinhAnh[4].FileName;
            //ktra hinh anh ton tai trong csdl chua
            for(int i = 0;i<HinhAnh.Count();i++)
            {
                if (HinhAnh[i].ContentLength > 0)
                {
                    var fileName = Path.GetFileName(HinhAnh[i].FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/HinhSP"), fileName);
                    // neu hinh anh trongh thu muc co r thi xuat ra thong bao
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.upload = "Hình đã tồn tại";
                        return View();
                    }
                    else
                    {
                        //lay ha dua vao thu muc
                        HinhAnh[i].SaveAs(path);
                        model.HinhAnh = fileName;
                    }
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult Xoa(int? id)
        {
            //lay sp can chinh sua vao id
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);

            return View(sp);

        }


        /*public ActionResult Xoa(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham model = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if(model == null)
            {
                return HttpNotFound();
            }
            db.SanPhams.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }*/
        [HttpPost]
        public ActionResult Xoa(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham model = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            db.SanPhams.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}