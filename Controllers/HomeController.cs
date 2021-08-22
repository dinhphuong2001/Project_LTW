using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DealineMVC.Models;
namespace DealineMVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        QuanLyBanHangEntities3 db = new QuanLyBanHangEntities3();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Index1()
        {
            var lstSanPhamLTM = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == 1);
            ViewBag.LstSP = lstSanPhamLTM;

            var lstSanPhamDT = db.SanPhams.Where(n => n.MaLoaiSP == 1 && n.Moi == 1);
            ViewBag.LstDT = lstSanPhamDT;
            var lstSanPhamMTB = db.SanPhams.Where(n => n.MaLoaiSP == 3 && n.Moi == 1);
            ViewBag.LstMTB = lstSanPhamMTB;
            return View();
        }
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
        public ActionResult Index2()
        {
            // tao viewbag de lay gia tri tu csdl
            // list dt moi nhat
            var lstDTM = db.SanPhams.Where(n => n.MaLoaiSP == 1 && n.Moi == 1);
            // gan vao viewbag
            ViewBag.ListDTM = lstDTM;
            var lstLT = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == 1);
            // gan vao viewbag
            ViewBag.ListLTM = lstLT;
            var lstMTB = db.SanPhams.Where(n => n.MaLoaiSP == 3 && n.Moi == 1);
            // gan vao viewbag
            ViewBag.ListMTB = lstMTB;
            return View();
        }
        public ActionResult DangNhap(FormCollection f)
        {
            string sTaiKhoan = f["txtTenDangNhap"].ToString();
            string sMatKhau = f["txtMatKhau"].ToString();
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan && n.MatKhau == sMatKhau);
            if (tv != null)
            {
                var ltvQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == tv.MaLoaiTV);
                String Quyen = "";
                foreach (var item in ltvQuyen)
                {
                    Quyen += item.Quyen.MaQuyen + ",";
                }
                Quyen = Quyen.Substring(0, Quyen.Length - 1);
                PhanQuyen(tv.TaiKhoan.ToString(), Quyen);
                Session["TaiKhoan"] = tv;
                return Content("<script>window.location.reload();</script>");
            }
            return Content("Tài khoản hoặc mật khẩu không chính xác");
        }
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            return RedirectToAction("Index1");
        }
        public ActionResult Register(ThanhVien tv,FormCollection f)
        {
            db.ThanhViens.Add(tv);
            db.SaveChanges();
            return View();
        }
        public ActionResult Contact(LienHe lh ,FormCollection f)
        {
            try
            {
                db.LienHes.Add(lh);
                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }
            return View();
        }
        public ActionResult About_us(FormCollection f)
        {
            return View();
        }
        public void PhanQuyen(String TaiKhoan, String Quyen)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1, TaiKhoan, DateTime.Now, DateTime.Now.AddMinutes(15), false, Quyen, FormsAuthentication.FormsCookiePath);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
            Response.Cookies.Add(cookie);
        }
        public ActionResult LoiPhanQuyen()
        {
            return View();
        }

    }
}