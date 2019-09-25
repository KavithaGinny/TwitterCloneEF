using MyTwitter.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyTwitter.Controllers
{
    public class ProfileController : Controller
    {
        private TwitterModel1Container db = new TwitterModel1Container();

        // GET: People/Details/5
        [Authorize]
        [OutputCache(Duration = 0, NoStore = true)]
        public ActionResult Details()
        {
            if (User.Identity.Name == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(User.Identity.Name);
            if (person == null)
            {
                return HttpNotFound();
            }
            NewProfile ep = new NewProfile();
            ep.active = person.active;
            ep.emailID = person.email;
            ep.fullName = person.fullName;
            ep.Joined = person.joined;
            return View(ep);
        }


        // GET: People/Edit/5
        [Authorize]
        [OutputCache(Duration = 0, NoStore = true)]
        public ActionResult Edit()
        {
            if (User.Identity.Name == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(User.Identity.Name);
            if (person == null)
            {
                return HttpNotFound();
            }
            NewProfile editprof = new NewProfile();
            editprof.fullName = person.fullName;
            editprof.emailID = person.email;
            editprof.active = person.active;
            return View(editprof);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [OutputCache(Duration = 0, NoStore = true)]
        public ActionResult Edit(NewProfile editProf)
        {
            if (ModelState.IsValid)
            {
                Person persondetails = db.People.Find(User.Identity.Name);
                if (persondetails != null)
                {
                    persondetails.fullName = editProf.fullName;
                    persondetails.email = editProf.emailID;
                    persondetails.active = editProf.active;


                    db.Entry(persondetails).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details");
                }
            }
            return View(editProf);
        }
        [Authorize]
        [OutputCache(Duration = 0, NoStore = true)]
        public ActionResult ChangePassword()
        {
            if (User.Identity.Name == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(User.Identity.Name);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [OutputCache(Duration = 0, NoStore = true)]
        public ActionResult ChangePassword(NewPassword updpass)
        {
            if (ModelState.IsValid)
            {
                Person person = db.People.Find(User.Identity.Name);
                if (Helper.EncodePasswordMd5(updpass.oldpassWord) == person.password)
                {
                    if (Helper.EncodePasswordMd5(updpass.NewpassWord) == Helper.EncodePasswordMd5(updpass.ConfirmpassWord))
                    {
                        person.password = Helper.EncodePasswordMd5(updpass.ConfirmpassWord);
                        db.Entry(person).State = EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.PasswordUpdateSuccess = true;
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "New password not matching with confirm password..!");
                        return View();

                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid old password..!");
                    return View();
                }

            }
            return View();
        }

        // GET: People/Delete/5
        [Authorize]
        [OutputCache(Duration = 0, NoStore = true)]
        public ActionResult Delete()
        {
            if (User.Identity.Name == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(User.Identity.Name);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        [OutputCache(Duration = 0, NoStore = true)]
        public ActionResult DeleteConfirmed()
        {
            var twt = db.Tweets.Where(x => x.user_id == User.Identity.Name);
            db.Tweets.RemoveRange(twt);
            db.SaveChanges();
            Person person = db.People.Find(User.Identity.Name);
            db.People.Remove(person);
            db.SaveChanges();
            System.Web.Security.FormsAuthentication.SignOut();
            return RedirectToAction("login", "Login");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}