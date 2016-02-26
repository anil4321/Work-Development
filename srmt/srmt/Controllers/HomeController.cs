using srmt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Diagnostics;
using srmt.ViewModel;
using System.Web.Routing;

namespace srmt.Controllers
{
    public class HomeController : Controller
    {
        private CRIDBAEntities db = new CRIDBAEntities();
        //public ActionResult Index()
        //{
        //    string id = "0652858337";
        //    if (this.Request.ClientCertificate.IsPresent)
        //    {
        //        try
        //        {
        //            id = ReadCert();
        //        }
        //        catch (Exception ex)
        //        {
        //            Response.Write(string.Format("An error occured:"));
        //            Response.Write(string.Format(ex.Message));
        //            Response.Write(string.Format(ex.StackTrace));
        //        }
        //    }

        //    if (string.IsNullOrEmpty(id))
        //    {
        //        return View("CertError");
        //        //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    List<CRIS_USER> list = (from a in db.EMPLOYEEs
        //                            join c in db.CRIS_USER on a.USER_ID equals c.USER_ID
        //                            where (a.EMPL_UID.Equals(id))
        //                            select c).ToList();
        //    ViewBag.crisUser = list;
        //    return View();
        //}

        //Entry Point
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult SignIn()
        {
            var SignInViewModel = new SignIn();
            string userUPN = "";
            //Test Code
            userUPN = "0652858337";
            SignInViewModel.uid = "0652858337";
            SignInViewModel.name = "Anil K Das";
            ViewBag.UserName = "Anil K Das";
            //----------------------------------------
            ViewBag.requestParm = Request.ClientCertificate.IsPresent;
            if (Request.ClientCertificate.IsPresent) { 
            try
            {
                string oid = "OID";
                string cn = "CN";
                string uscis = ".USCIS";

                X509Certificate2 x509Cert2 = new X509Certificate2(Request.ClientCertificate.Certificate);
                string id = string.Format(x509Cert2.Subject);
                //Response.Write(string.Format(x509Cert2.Subject));
                    //CN = ANIL K DAS (affiliate) + OID.0.9.2342.19200300.100.1.1 = 0652858337.USCIS, OU = People, OU = USCIS, OU = Department of Homeland Security, O = U.S.Government, C = US
                    /*
                    1. Get CN and read until + to get the Name
                    2. Get OID. 
                    3. Get the next token after = and until space
                    3. This token is the UID + USCIS
                    4. Suppress USCIS from UID
                    */

                    //Get UID
                    int position1 = id.IndexOf(oid);
                int position2 = id.IndexOf(uscis);

                string str1 = id.Substring(position1, position2 - position1);
                char[] delimiterChars = { '=' };
                string uid = str1.Replace(".USCIS", "").Split(delimiterChars).Last().Trim();
                SignInViewModel.uid = uid;
                ViewBag.uid = uid;
                //Get Name
                    position1 = id.IndexOf(cn);
                str1 = id.Substring(position1, id.Length - position1);
                position1 = str1.IndexOf("=");
                position2 = str1.IndexOf("+");
                int position3 = str1.IndexOf(",");
                if (position2 > 0 && position3 > 0)
                {
                    if (position3 < position2)
                    {
                        position2 = position3;
                    }
                }
                else {
                    if (position2 < 0)
                        position2 = position3;
                }

                string str2 = str1.Substring(position1 + 1, position2 - position1 - 1);
                SignInViewModel.name = str2;
                string userName = str2.Replace("(affiliate)", "").Trim() + "(" + uid + ")";
                ViewBag.UserName = userName;
                }
                catch (Exception ex)
            {
                Response.Write(string.Format(ex.Message));
                Response.Write(string.Format(ex.StackTrace));
                //return View("CertError");
            }
            }
            return View("SignIn", SignInViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult SignIn(SignIn si)
        {
            string id = si.uid;
            string name = si.name;
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("SignIn");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<CRIS_USER> list = (from a in db.EMPLOYEEs
                                    join c in db.CRIS_USER on a.USER_ID equals c.USER_ID
                                    where (a.EMPL_UID.Equals(id))
                                    select c).ToList();
            ViewBag.crisUser = list;
            @ViewBag.UserName = si.name;
            @ViewBag.uid = si.uid;
            HttpCookie myUID = new HttpCookie("uid");
            switch (list.Count)
            {
                case 0:
                    ViewBag.count = 0;
                    ModelState.Clear();
                    return View();
                    break;
                case 1:
                    myUID.HttpOnly = true;
                    myUID.Value = id;
                    this.ControllerContext.HttpContext.Response.Cookies.Add(myUID);
                    List<srmt.Models.CRIS_USER> items = list;
                    return RedirectToAction("Detail", new RouteValueDictionary(new { controller = "Home", action = "Detail", id = items[0].LOGIN_NAME }));
                    break;
                default:
                    myUID.HttpOnly = true;
                    myUID.Value = id;
                    this.ControllerContext.HttpContext.Response.Cookies.Add(myUID);
                    return View("Index");
                    break;

            }


            //if (list.Count <= 0)
            //{
            //    ViewBag.count = 0;
            //    ModelState.Clear();
            //    return View();
            //}
            //else {
            //    HttpCookie myUID = new HttpCookie("uid");
            //    myUID.HttpOnly = true;
            //    myUID.Value = id;
            //    this.ControllerContext.HttpContext.Response.Cookies.Add(myUID);
            //    return View("Index");
            //}
        }
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult LogOff()
        {
            Session["User"] = null; //it's my session variable
            Session.Clear();
            Session.Abandon();
            if (Request.Cookies["username"] != null)
            {
                var c = new HttpCookie("username");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }
            if (Request.Cookies["uid"] != null)
            {
                var c = new HttpCookie("uid");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }
            return RedirectToAction("SignIn", "Home");
        }

        // GET: employees/Edit/5
        public ActionResult Detail(string id)
        {
            if (Request.Cookies["uid"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                HttpCookie username = new HttpCookie("username");
                //username.Expires = DateTime.Now.AddMinutes(20.0);
                username.Value = id;
                this.ControllerContext.HttpContext.Response.Cookies.Add(username);
                return Redirect("/cris/employee/logonEmployee.do");
            }
            else
            {
                return Redirect("/srmt");
            }
        }
    }
}
