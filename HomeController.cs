using CURDEF1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CURDEF1.Controllers
{
    public class HomeController : Controller
    {
        Test1Entities db = new Test1Entities();

        public ActionResult Login()
        {
            return View();

        }
        [HttpPost]
        public ActionResult Login(CURDEF1.Models.Login loginmodel)
        {
            using (Test1Entities db = new Test1Entities())
            {
                var userDetails = db.Logins.Where(x => x.UserName == loginmodel.UserName && x.Password == loginmodel.Password).FirstOrDefault();
                if (userDetails == null)
                {
                    loginmodel.LoginErrorMessage = "Wrong UserName or Password";
                    return View("Login","Home");
                }
                else
                {
                    Session["UserID"] = userDetails.UserID;
                    Session["UserName"] = userDetails.UserName;
                    return RedirectToAction("Index", "Home");
                }
            }

        }


        public ActionResult Logout()
        {
            int UserID = (int)Session["UserID"];
            Session.Abandon();
            return RedirectToAction("Login", "Home");
        }
        public ActionResult Index(FormCollection frm)
        {
            var courses = frm["arrcou"];
            List<Course> Courcelist = db.Courses.ToList();
            var list = db.Registrations.ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var item in list)
            {
                if(item.Cources !=null)
                {
                    string[] couarray = item.Cources.Split(',');
                    foreach (var c in Courcelist)
                    {
                        if(couarray.Contains(c.CouId.ToString()))
                        {
                            sb.Append(c.CouName + ",");
                            item.Cources = sb.ToString();

                        }
                    }
                }

                sb.Clear();
            }
            return View(list);
        }


        public ActionResult loaddata()
        {
            using (Test1Entities tc = new Test1Entities())
            {
                var data = tc.Registrations.OrderBy(a => a.FirstName).ToList();
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
        }

     



        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration regd = db.Registrations.Find(id);
            if (regd == null)
            {
                return HttpNotFound();
            }
            return View(regd);
        }

        [HttpGet]
        public ActionResult Create()
        {

            List<Country> Country = db.Countries.ToList();
            ViewBag.Country = new SelectList(Country, "cntId", "CntName");

            var State = db.States.ToList();
            ViewBag.State = new SelectList(State, "stId", "State");

            var City = db.Cities.ToList();
            ViewBag.State = new SelectList(City, "ctId", "City");

            List<Course> Courcelist = db.Courses.ToList();
            ViewBag.Courcelist = Courcelist;


            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Registration rec, FormCollection frm)
        {
            var cour = frm["arrcou"];
            rec.Cources = cour;

            var errors = ModelState.Where(x => x.Value.Errors.Any())
               .Select(x => new { x.Key, x.Value.Errors });

            List<Course> Courcelist = db.Courses.ToList();
            ViewBag.Course = new SelectList(Courcelist, "CouId", "CouName");

            List<Country> Country = db.Countries.ToList();
            ViewBag.Country = new SelectList(Country, "cntId", "CntName");

            var State = db.States.ToList();
            ViewBag.State = new SelectList(State, "stId", "State");

            var City = db.Cities.ToList();
            ViewBag.State = new SelectList(City, "ctId", "City");


            if (ModelState.IsValid)
            {
                StringBuilder sb = new StringBuilder();

                db.Registrations.Add(rec);
                db.SaveChanges();


                return RedirectToAction("Index");
            }
            return View();
        }


        [HttpGet]
        public ActionResult Edit(Int64? id, CourseModel model)
        {
            List<Country> Country = db.Countries.ToList();
             ViewBag.Country = new SelectList(Country, "cntId", "cntName");

            //List<UG> UGlist = entity.UGs.ToList();
            //ViewBag.UGlist = new SelectList(UGlist, "UGId", "UGName");

            List<Course> Courcelist = db.Courses.ToList();
            ViewBag.Courcelist = Courcelist;


            Registration Regrec = new Registration();
            if (id > 0)
            {
                // Regrec = entity.RegistrationDetails.Find(id);
                Regrec = db.Registrations.SingleOrDefault(p => p.Id == id);

            }
            Registration rec1 = new Registration();
            rec1.FirstName = Regrec.FirstName;
            rec1.MiddleName = Regrec.MiddleName;
            rec1.LastName = Regrec.LastName;
            rec1.Gender = Regrec.Gender;
            rec1.MobileNumber = Regrec.MobileNumber;
            rec1.EmailId = Regrec.EmailId;
            rec1.Cources = Regrec.Cources;

            foreach (var item in Courcelist)
            {

                string[] couarray = Regrec.Cources.Split(',');
                foreach (var item1 in Courcelist)
                {
                    if (couarray.Contains(item1.CouId.ToString()))
                    {
                        //sb.Append(u.UGName + ",");
                        //item.UG = sb.ToString();
                        item1.IsSelected = true;
                    }
                }

            }
            //var list = entity.RegistrationDetails.ToList();
            //StringBuilder sb = new StringBuilder();
            //foreach (var item in list)
            //{
            //    if (item.UG != null)
            //    {


            //        string[] ugarray = item.UG.Split(',');
            //        foreach (var u in UGlist)
            //        {
            //            if (ugarray.Contains(u.UGId.ToString()))
            //            {
            //                sb.Append(u.UGName + ",");
            //                item.UG = sb.ToString();
            //            }
            //        }

            //    }
            //}
            

           
            rec1.Country = Regrec.Country;
            rec1.State = Regrec.State;
            rec1.City = Regrec.City;
            rec1.NativePlace = Regrec.NativePlace;

            var State = db.States.Where(x => x.cntId == rec1.Country).ToList();
            ViewBag.State = new SelectList(State, "stId", "stName");

            var City = db.Cities.Where(x => x.stId == rec1.State).ToList();
            ViewBag.City = new SelectList(City, "ctId", "ctName");


            return View(Regrec);
        }

        [HttpPost]
        public ActionResult Edit(Int64 id, Registration rec, CourseModel model, FormCollection frm)
        {

            List<Country> Country = db.Countries.ToList();
            ViewBag.Country = new SelectList(Country, "cntId", "cntName");

            var State = db.States.ToList();
            ViewBag.State = new SelectList(State, "stId", "stName");

            var City = db.Cities.ToList();
            ViewBag.City = new SelectList(City, "ctId", "ctName");

            var courses = frm["arrcou"];
            Registration rec1 = db.Registrations.Find(rec.Id);
            List<Course> Courcelist = db.Courses.ToList();
            ViewBag.Courcelist =Courcelist;
            rec1.Id = rec.Id;
            rec1.FirstName = rec.FirstName;
            rec1.MiddleName = rec.MiddleName;
            rec1.LastName = rec.LastName;
            rec1.Gender = rec.Gender;
            rec1.MobileNumber = rec.MobileNumber;
            rec1.EmailId = rec.EmailId;
            rec1.Cources = courses;
            rec1.Country = rec.Country;
            rec1.State = rec.State;
            rec1.City = rec.City;
            rec1.NativePlace = rec.NativePlace;
            db.SaveChanges();

            return RedirectToAction("Index");

        }

        public ActionResult Delete(Int64? id)
        {
            Registration Regrec = new Registration();
            if (id > 0)
            {
                Regrec = db.Registrations.SingleOrDefault(p => p.Id == id);

            }
            return View(Regrec);

        }

        [HttpPost]
        public ActionResult Delete(Int64 id)
        {

            Registration rec1 = db.Registrations.Find(id);
            db.Registrations.Remove(rec1);
            db.SaveChanges();
            return RedirectToAction("Index");

        }


        public JsonResult State_Bind(int CntId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<State> list = db.States.Where(x => x.cntId == CntId).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult City_Bind(int stId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<City> list = db.Cities.Where(x => x.stId == stId).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsEmpNameandMailExist(int? Id, string EmailId)
        {
            bool result = true;

            if (Id == null)
            {

                var isExists = db.Registrations.Where(u => u.EmailId == EmailId).FirstOrDefault();
                if (isExists != null)
                {
                    result = false;
                }

            }
            else
            {
                var isExists = db.Registrations.Where(u => u.EmailId == EmailId && u.Id == Id).FirstOrDefault();
                if (isExists == null)
                {
                    result = false;
                }


            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}