using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DealineMVC.Models;
namespace DealineMVC.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult SanPhamStyle1Partial()
        {
            QuanLyBanHangEntities2 db = new QuanLyBanHangEntities2();
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
    }
}