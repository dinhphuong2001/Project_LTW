using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DealineMVC.Models;

namespace DealineMVC.Controllers
{
    public class DemoAjaxController : Controller
    {
        QuanLyBanHangEntities2 db = new QuanLyBanHangEntities2();
        // GET: DemoAjax
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DemoAjax ()
        {
            return View();
        }
        public ActionResult TestAjaxActionLink()
        {
            System.Threading.Thread.Sleep(3000);
            return Content("Nhất thế giới");
        }
        public ActionResult LoadAjaxBeginForm(FormCollection f)
        {
            System.Threading.Thread.Sleep(3000);
            string kq = f["txt1"].ToString();
            return Content(kq);
        }
        // su dung load ajax tra ve kq partial view
        public ActionResult LoadSPAjax()
        {
            var lstSanPham = db.SanPhams;
            /*var lstSanPhamLTM = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == 1);*/
            return PartialView(lstSanPham);
        }

    }
}