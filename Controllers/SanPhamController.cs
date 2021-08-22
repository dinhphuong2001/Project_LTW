using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DealineMVC.Models;
using PagedList;

namespace DealineMVC.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: SanPham
        QuanLyBanHangEntities3 db = new QuanLyBanHangEntities3();
        public ActionResult SanPham1()
        {
            var lstSanPhamLTM = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == 1);
            ViewBag.LstSP = lstSanPhamLTM;

            var lstSanPhamDT = db.SanPhams.Where(n => n.MaLoaiSP == 1 &&n.Moi == 1);
            ViewBag.LstDT = lstSanPhamDT;
            var lstSanPhamMTB = db.SanPhams.Where(n => n.MaLoaiSP == 3&& n.Moi == 1);
            ViewBag.LstMTB = lstSanPhamMTB;
            return View();
        }
        public ActionResult SanPham2()
        {
            var lstSanPhamLTM = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == 1);
            ViewBag.LstSP = lstSanPhamLTM;
            return View();
        }
        [ChildActionOnly]
        public ActionResult SanPhamPartial()
        {
            /*var lstSanPhamLTM = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == 1);*/
            return PartialView();
        }
        public ActionResult MenuPartial()
        {
            var lstSP = db.SanPhams;
            return PartialView(lstSP);
        }
        public ActionResult XemChiTietSanPham(int? id)
        {
            // ktra tham so truyen vao co rong hay  khong
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // neu khong ti xuat ra csdl lay sp tuong ung
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id && n.DaXoa == 0);
                if(sp==null)
                {
                // thong bao neu nhu khong co san pham do
                return HttpNotFound();
            }
            return View(sp);
        }
        // action load sp theo ma loai sp va ma nsx
        public ActionResult SanPham (int? MaLoaiSP, int? MaNSX, int? page)
        {
           /* if (Session["TaiKhoan"] == null)
            {
                return RedirectToAction("Index1", "Home");
            }*/
            if (MaLoaiSP == null || MaNSX == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var lstSP = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP && n.MaNSX == MaNSX);
            if (lstSP.Count() == 0)
            {
                return HttpNotFound();
            }
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            //phan trang
            //tao bien so trang 
            int PageSize = 6;

            // tao bien so trang hien tai
            int PageNumber = (page ?? 1);
            ViewBag.MaLoaiSP = MaLoaiSP;
            ViewBag.MaNSX = MaNSX;
            return View(lstSP.OrderBy(n=>n.MaSP).ToPagedList(PageNumber, PageSize));
        }


    }
}