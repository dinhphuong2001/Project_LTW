using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DealineMVC.Models;
namespace DealineMVC.Controllers
{
    [Authorize(Roles = "QuanTri")]
    public class QuanLyPhieuNhapController : Controller
    {
        // GET: QuanLyPhieuNhap
        QuanLyBanHangEntities3 db = new QuanLyBanHangEntities3();
        [HttpGet]
        public ActionResult NhapHang()
        {
            ViewBag.MaNCC = db.NhaCungCaps;
            ViewBag.ListSanPham = db.SanPhams;
            return View();
        }
        [HttpPost]
        public ActionResult NhapHang(PhieuNhap model, IEnumerable<ChiTietPhieuNhap> lstModel)
        {

            ViewBag.LstModel = db.ChiTietPhieuNhaps;
            ViewBag.MaNCC = db.NhaCungCaps;
            ViewBag.ListSanPham = db.SanPhams;
            return View();
        }
        /*public ActionResult NhapHang(PhieuNhap model, IEnumerable<ChiTietPhieuNhap> lstModel)
        {
           
            ViewBag.MaNCC = db.NhaCungCaps;
            ViewBag.ListSanPham = db.SanPhams;


            *//*SanPham sp;
            foreach(var item in lstModel)
            {
                //cap nhat so luong ton
                sp = db.SanPhams.Single(n=>n.MaSP==item.MaSP);
                sp.SoLuongTon += item.SoLuongNhap;
                item.MaPN = model.MaPN;

            }
            db.ChiTietPhieuNhaps.AddRange(lstModel);
            db.SaveChanges();*//*

            model.DaXoa = 1;
            db.PhieuNhaps.Add(model);
            foreach(var item in lstModel)
            {
                item.MaPN = model.MaPN;
            }
            db.ChiTietPhieuNhaps.AddRange(lstModel);
            db.SaveChanges();
            return View();
        }*/


    }
}