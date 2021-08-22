using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DealineMVC.Models;
using PagedList;
namespace DealineMVC.Controllers
{
    public class TimKiemController : Controller
    {
        // GET: TimKiem
        QuanLyBanHangEntities3 db = new QuanLyBanHangEntities3();
        [HttpGet]
        public ActionResult KQTimKiem(string sTuKhoa, int? page)
        {
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            //phan trang
            //tao bien so trang 
            int PageSize = 6;

            // tao bien so trang hien tai
            int PageNumber = (page ?? 1);
            // tim theo ten sp
            var lstSP = db.SanPhams.Where(n => n.TenSP.Contains(sTuKhoa));
            ViewBag.TuKhoa = sTuKhoa;
         
            return View(lstSP.OrderBy(n=>n.TenSP).ToPagedList(PageNumber, PageSize));
        }
        [HttpPost]
        public ActionResult KQTimKiem(string sTuKhoa, int? page, FormCollection f )
        {
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            //phan trang
            //tao bien so trang 
            int PageSize = 6;

            // tao bien so trang hien tai
            int PageNumber = (page ?? 1);
            // tim theo ten sp
            var lstSP = db.SanPhams.Where(n => n.TenSP.Contains(sTuKhoa));
            ViewBag.TuKhoa = sTuKhoa;

            return View(lstSP.OrderBy(n => n.TenSP).ToPagedList(PageNumber, PageSize));
        }
        public ActionResult KQTimKiemPartial (string sTuKhoa)
        {
            var lstSP = db.SanPhams.Where(n => n.TenSP.Contains(sTuKhoa));
            ViewBag.TuKhoa = sTuKhoa;
            return PartialView(lstSP.OrderBy(n=>n.DonGia));
        }
    }
}