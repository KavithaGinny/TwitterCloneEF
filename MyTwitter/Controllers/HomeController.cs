using MyTwitter.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyTwitter.Controllers
{
    
    public class HomeController : Controller
    {
        private TwitterModel1Container db = new TwitterModel1Container();

        // GET: Home
        [Authorize]
        [OutputCache(Duration = 0, NoStore = true)]
        public ActionResult Index()
            {
            //Person person = db.People.Find((string)Session["username"]);
            Person person = db.People.Find(User.Identity.Name);
            return View(person);
        }



        [Authorize]
        [OutputCache(Duration = 0, NoStore = true)]
        public JsonResult SearchPerson(string PersonUserId)
        {
            string person_Id = string.Empty;
            var Srhperson = db.People.Find(PersonUserId);            
            if (Srhperson != null)
                person_Id = Srhperson.user_Id;
            var FollowingChk = db.Followers.Where(x => x.user_id == User.Identity.Name && x.following_id == PersonUserId).Select(x => x.user_id).Count();
            Dictionary<string, string> dt = new Dictionary<string, string>();
            dt.Add("PersonID", person_Id);
            dt.Add("FollowCheck", FollowingChk.ToString());
            var jsonobject = JsonConvert.SerializeObject(dt);
            return Json(jsonobject, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        [OutputCache(Duration = 0, NoStore = true)]
        public ActionResult PostTwitter(string messagePost)
        {
            Tweet twts = new Tweet();
            Person person = db.People.Find((string)Session["username"]);
            twts.message = messagePost;
            twts.created = DateTime.Now;
            twts.user_id = User.Identity.Name;
            twts.Person = person;
            db.Tweets.Add(twts);
            db.SaveChanges();
            string message = "SUCCESS";
            return Json(new { Message = message, JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        [Authorize]
        [OutputCache(Duration = 0, NoStore = true)]
        public ActionResult FollowPerson(string PersionUserId)
        {
            Followers flw = new Followers();
            flw.user_id = User.Identity.Name;
            flw.following_id = PersionUserId;
            Person person = db.People.Find(User.Identity.Name);
            flw.Person = person;
            object message = "";
            var FollowingChk = db.Followers.Where(x => x.user_id == User.Identity.Name && x.following_id == PersionUserId).Select(x => x.user_id).Count();
            if (FollowingChk == 0)
            {
                db.Followers.Add(flw);
                message = "SUCCESS";
            }
            else
            {
                Followers FOLLOWINGperson = db.Followers.Where(x => x.user_id == flw.user_id && x.following_id == flw.following_id).FirstOrDefault();
                db.Followers.Remove(FOLLOWINGperson);
                message = "DeleteSUCCESS";
            }
            db.SaveChanges();
            var jsonobject = JsonConvert.SerializeObject(message);
            return Json(jsonobject, JsonRequestBehavior.AllowGet);
        }


        [Authorize]
        [OutputCache(Duration = 0, NoStore = true)]
        public JsonResult GetTwitter()
        {
            
            var twt = db.Tweets.OrderByDescending(cr => cr.created).Select(x => new { user_id = x.user_id, messaage = x.message, created = x.created }).ToList();           
            var jsonobject = JsonConvert.SerializeObject(twt);
            return Json(jsonobject, JsonRequestBehavior.AllowGet);
        }


        [Authorize]
        [OutputCache(Duration = 0, NoStore = true)]
        public JsonResult GetTwittecount()
        {
            var twt = db.Tweets.Select(x => x.tweet_id).Count();

            var jsonobject = JsonConvert.SerializeObject(twt);
            return Json(jsonobject, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [OutputCache(Duration = 0, NoStore = true)]
        public JsonResult GetFollowingFollowercount()
        {

            var FollowingCnt = db.Followers.Where(x => x.user_id == User.Identity.Name).Select(x => x.user_id).Count();
            var FollowerCnt = db.Followers.Where(x => x.following_id == User.Identity.Name).Select(x => x.following_id).Count();
            Dictionary<string, string> cnt = new Dictionary<string, string>();
            cnt.Add("FollowingCnt", FollowingCnt.ToString());
            cnt.Add("FollowerCnt", FollowerCnt.ToString());

            var jsonobject = JsonConvert.SerializeObject(cnt);
            return Json(jsonobject, JsonRequestBehavior.AllowGet);
        }
    }
}