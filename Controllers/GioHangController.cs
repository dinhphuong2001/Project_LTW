using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DealineMVC.Models;

namespace DealineMVC.Controllers
{
    public class GioHangController : Controller
    {
        QuanLyBanHangEntities3 db = new QuanLyBanHangEntities3();
        //lay gio hang
        public List<ItemGioHang> LayGioHang()
        {
            // gio hang da ton tai
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if(lstGioHang==null)
            {
                // neu session gio hang chua ton tai thi khoi tao gio hang moi
                lstGioHang = new List<ItemGioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        // them gio hang cach thuong (load lai trang)
        public ActionResult ThemGioHang(int MaSP, string strURL)
        {
            // ktra san pham co ton tai trong csdl hay khong
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                // trang duiong dan khong hop le
                Response.StatusCode = 404;
                return null;
            }
            // lay gio hang
            List<ItemGioHang> lstGioHang = LayGioHang();
            // TH1: neu sp da ton tai trong gio hang
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck != null)
            {
                // ktra so so luong ton 
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return View("ThongBao");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                return Redirect(strURL);
            }
            ItemGioHang itemGH = new ItemGioHang(MaSP);
            lstGioHang.Add(itemGH);
            return Redirect(strURL);

        }
        // tinh tong so luong
        public double TinhTongSoLuong()
        {
            // lay gio hang
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if(lstGioHang==null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.SoLuong);
        }
        // tinh tong tien
        public decimal TinhTongTien()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.ThanhTien);
        }
        




        // GET: XemGioHang
        public ActionResult XemGioHang()
        {
            //lay gio hang
            List<ItemGioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }
        public ActionResult GioHangPartial()
        {
            if(TinhTongSoLuong()==0)
            {
                ViewBag.TinhTongSoLuong = 0;
                ViewBag.TinhTongTien = 0;
                return PartialView();
            }
            ViewBag.TinhTongSoLuong = TinhTongSoLuong();
            ViewBag.TinhTongTien = TinhTongTien();

            return PartialView();

        }
        [HttpGet]
        public ActionResult SuaGioHang(int MaSP)
        {
            // ktra session gio hang ton tai chua
            if(Session["GioHang"]==null)
            {
                return RedirectToAction("Index1", "Home");

            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                // trang duiong dan khong hop le
                Response.StatusCode = 404;
                return null;
            }
            //lay list gio hang tu session
            List<ItemGioHang> lstGioHang = LayGioHang();
            // ktra sp co ton tai trong gio hang hayk 
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index1", "Home");

            }
            //lay list gio hang tao giaodien
            ViewBag.GioHang = lstGioHang;
            return View(spCheck);
        }
        [HttpPost]
        public ActionResult CapNhatGioHang(ItemGioHang itemGH)
        {
            //ktra so luong ton
            SanPham spCheck = db.SanPhams.Single(n => n.MaSP == itemGH.MaSP);
            if(spCheck.SoLuongTon<itemGH.SoLuong)
            {
                return View("ThongBao");
            }
            //cap nhat sl trong session gio hang
            List<ItemGioHang> lstGH = LayGioHang();
            ItemGioHang itemGHupdate = lstGH.Find(n => n.MaSP == itemGH.MaSP);
            itemGHupdate.SoLuong = itemGH.SoLuong;
            itemGHupdate.ThanhTien = itemGHupdate.SoLuong * itemGHupdate.DonGia;
            return RedirectToAction("XemGioHang");
            
        }
        public ActionResult XoaGioHang (int MaSP)
        {
            // ktra session gio hang ton tai chua
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index1", "Home");

            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                // trang duiong dan khong hop le
                Response.StatusCode = 404;
                return null;
            }
            //lay list gio hang tu session
            List<ItemGioHang> lstGioHang = LayGioHang();
            // ktra sp co ton tai trong gio hang hayk 
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index1", "Home");

            }


            lstGioHang.Remove(spCheck);

            return RedirectToAction("XemGioHang");
        }
        public ActionResult DatHang(KhachHang kh)
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index1", "Home");

            }
            KhachHang khang = new KhachHang();
            if(Session["TaiKhoan"]==null)
            {
                // them kh doi voi kh chua co tai khoan
                khang = kh;
                db.KhachHangs.Add(khang);
                db.SaveChanges();

            }
            else
            {
                //doi vs kh la thanh vien
                ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
                khang.TenKH = tv.HoTen;
                khang.DiaChi = tv.DiaChi;
                khang.Email = tv.Email;
                khang.SoDienThoai = tv.SoDienThoai;
                db.KhachHangs.Add(khang);
                db.SaveChanges();
            }

            //them don hang
            DonDatHang ddh = new DonDatHang();
            ddh.MaKH = khang.MaKH;
            ddh.NgayDat = DateTime.Now;
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            ddh.UuDai = 0;
            ddh.DaHuy = false;
            ddh.DaXoa = false;
            db.DonDatHangs.Add(ddh);
            db.SaveChanges();
            //them ct don dat hang
            List<ItemGioHang> lstGH = LayGioHang();
            foreach (var item in lstGH)
            {
                ChiTietDonDatHang ctdh = new ChiTietDonDatHang();
                ctdh.MaDDH = ddh.MaDDH;
                ctdh.MaSP = item.MaSP;
                ctdh.TenSP = item.TenSP;
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = item.DonGia;
                db.ChiTietDonDatHangs.Add(ctdh);
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang");
        }
        // them gio hang ajax
        public ActionResult ThemGioHangAjax(int MaSP, string strURL)
        {
            // ktra san pham co ton tai trong csdl hay khong
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                // trang duiong dan khong hop le
                Response.StatusCode = 404;
                return null;
            }
            // lay gio hang
            List<ItemGioHang> lstGioHang = LayGioHang();
            // TH1: neu sp da ton tai trong gio hang
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck != null)
            {
                // ktra so so luong ton 
                if (sp.SoLuongTon < spCheck.SoLuong)
                {

                    return Content("<script> alert(\"Sản phẩm đã hết hàng\")");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                ViewBag.TinhTongSoLuong = TinhTongSoLuong();
                ViewBag.TinhTongTien = TinhTongTien();
                return PartialView("GioHangPartial");
            }
            ItemGioHang itemGH = new ItemGioHang(MaSP);
            lstGioHang.Add(itemGH);
            ViewBag.TinhTongSoLuong = TinhTongSoLuong();
            ViewBag.TinhTongTien = TinhTongTien();
            return PartialView("GioHangPartial");

        }

    }
}