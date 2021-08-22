using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DealineMVC.Models;
namespace DealineMVC.Controllers
{
    [Authorize(Roles = "QuanTri")]
    public class QuanLyDonHangController : Controller
    {
        // GET: QuanLyDonHang
        QuanLyBanHangEntities3 db = new QuanLyBanHangEntities3();
        public ActionResult DaThanhToanChuaGiao()
        {
            //lay ds cac don hang da duyet
            var lst = db.DonDatHangs.Where(n => n.DaThanhToan == true && n.TinhTrangGiaoHang == false);

            return View(lst);
        }
        public ActionResult ChuaGiaoChuaThanhToan()
        {
            //lay ds cac don hang chua giao va chua thanh toan
            var lstDSHCG = db.DonDatHangs.Where(n => n.TinhTrangGiaoHang == false && n.DaThanhToan == false);

            return View(lstDSHCG);
        }
        public ActionResult DaGiaoDaThanhToan()
        {
            //lay ds cac don hang chua giao
            var lstDSHDGDTT = db.DonDatHangs.Where(n => n.TinhTrangGiaoHang == true && n.DaThanhToan == true);

            return View(lstDSHDGDTT);
        }
        [HttpGet]
        public ActionResult DuyetDonHang(int? id)
        {
            //ktra id hop le
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang model = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            //lay ds chi tiet don hang de hien thi cho nguoi dung
            var lstChiTietDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == id);
            ViewBag.ListChiTietDH = lstChiTietDH;
            return View(model);
        }
        [HttpPost]
        public ActionResult DuyetDonHang(DonDatHang ddh)
        {
            // truy van lay du lieu cua don hang do
            DonDatHang ddhUpdate = db.DonDatHangs.Single(n => n.MaDDH == ddh.MaDDH);
            ddhUpdate.DaThanhToan = ddh.DaThanhToan;
            ddhUpdate.TinhTrangGiaoHang = ddh.TinhTrangGiaoHang;
            db.SaveChanges();
            //lay ds chi tiet don hang
            var lstChiTietDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == ddh.MaDDH);
            ViewBag.ListChiTietDH = lstChiTietDH;
            return View(ddhUpdate);





        }


    }
}