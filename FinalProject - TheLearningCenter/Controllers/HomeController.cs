using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheLearningCenterReworked.Repositories;
using TheLearningCenterReworked.Context;

namespace TheLearningCenterReworked.Controllers
{
    public class HomeController : Controller
    {

        private IClassRepository classRepository;
        private IUserRepository userRepository;

        public HomeController(IClassRepository classRepository, IUserRepository userRepository)
        {
            this.classRepository = classRepository;
            this.userRepository = userRepository;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize]
        public ActionResult Notes()
        {
            return View();
        }
        public ActionResult ClassList()
        {
            var classList = new TheLearningCenterReworkedEntities().Classes.ToList();
            ViewBag.ClassList = classList;

            return View();
        }
        public ActionResult DropClass(string className)
        {
            var user = userRepository.Users.Where(u => u.UserEmail == (string)Session["UserEmail"]).First();

            using (var dbCtx = new TheLearningCenterReworkedEntities())
            {
                /* Get existing data from database */
                var existingStudent = dbCtx.Users.Include("Classes")
                        .Where(s => s.UserEmail == user.UserEmail).FirstOrDefault<User>();

                /* Find deleted courses from student's course collection by 
                students' existing courses (existing data from database) minus students' 
                current course list (came from client in disconnected scenario) */

                var deletedClasses = UserClassNotEnrolledIn2();

                /* Find Added courses in student's course collection by students' 
                current course list (came from client in disconnected scenario) minus 
                students' existing courses (existing data from database)  */

                Class c = new Class();
                foreach (var classItem in dbCtx.Classes)
                {
                    if (className == classItem.ClassName)
                    {
                        c = classItem;
                    }
                }

                /* Remove deleted courses from students' existing course collection 
                (existing data from database)*/
                //deletedClasses.ForEach(c => existingStudent.Classes.Remove(c));   //**NOT WORKING**
                existingStudent.Classes.Remove(c);

                // Save changes which will reflect in StudentCourse table only
                dbCtx.SaveChanges();
            }
            return RedirectToAction("StudentClasses", "Home");
        }

        public ActionResult EnrollInClass()
        {
            if (Session["UserEmail"] != null)
            {
                ViewBag.ClassList = UserClassNotEnrolledIn2();

                return View();
            }
            else { return RedirectToAction("LogOn", "Account"); }
        }

        public ActionResult AddEnrollInClass(string className)
        {
            var user = userRepository.Users.Where(u => u.UserEmail == (string)Session["UserEmail"]).First();

            using (var dbCtx = new TheLearningCenterReworkedEntities())
            {
                /* Get existing data from database */
                var existingStudent = dbCtx.Users.Include("Classes")
                        .Where(s => s.UserEmail == user.UserEmail).FirstOrDefault<User>();

                /* Find Added courses in student's course collection by students' 
                current course list (came from client in disconnected scenario) minus 
                students' existing courses (existing data from database)  */

                Class c = new Class();
                foreach (var classItem in dbCtx.Classes)
                {
                    if (className == classItem.ClassName)
                    {
                        c = classItem;
                    }
                }

                // Add new courses
                /* Attach courses because it came from client 
                as detached state in disconnected scenario*/
                if (dbCtx.Entry(c).State == System.Data.Entity.EntityState.Detached)
                    dbCtx.Classes.Attach(c);

                // Add course in existing student's course collection
                existingStudent.Classes.Add(c);

                // Save changes which will reflect in StudentCourse table only
                dbCtx.SaveChanges();
            }
            return RedirectToAction("StudentClasses", "Home");
        }

        public ActionResult StudentClasses()
        {
            if (Session["UserEmail"] != null)
            {
                ViewBag.ClassList = currentUserClassEnrollment();
                return View();
            }
            else { return RedirectToAction("LogOn", "Account"); }
        }
        public List<Class> currentUserClassEnrollment()
        {
            var learningCenterEntityReworked = new TheLearningCenterReworkedEntities();
            List<Class> userClassEnrolled = new List<Class>();

            foreach (var user in learningCenterEntityReworked.Users)
            {
                if (user.UserEmail == (string)Session["UserEmail"])
                {
                    userClassEnrolled.AddRange(user.Classes);
                }
            }

            return userClassEnrolled;
        }
        //public List<Class> UserClassNotEnrolledIn()
        public IEnumerable<SelectListItem> UserClassNotEnrolledIn()
        {
            var allClassesList = DatabaseManager.Instance.Classes.ToList();
            List<Class> classList = currentUserClassEnrollment();
            var userClassSelectList = new List<SelectListItem>();

            var listDifference = allClassesList.Except(classList, new IdComparer()).ToList();

            foreach (var userClassItem in listDifference)
            {
                userClassSelectList.Add(new SelectListItem
                {
                    Text = userClassItem.ClassName,
                    Value = userClassItem.ClassName
                });
            }        
            return userClassSelectList;
        }
        public List<Class> UserClassNotEnrolledIn2()
        {
            var allClassesList = DatabaseManager.Instance.Classes.ToList();
            List<Class> classList = currentUserClassEnrollment();
            var userClassList = new List<Class>();

            var listDifference = allClassesList.Except(classList, new IdComparer()).ToList();

            foreach (var userClassItem in listDifference)
            {
                userClassList.Add(new Class()
                {
                    ClassId = userClassItem.ClassId,
                    ClassDescription = userClassItem.ClassDescription,
                    ClassName = userClassItem.ClassName,
                    ClassPrice = userClassItem.ClassPrice
                });
            }
            //return userClassSelectList;
            return userClassList;
        }
    }
    public class IdComparer : IEqualityComparer<Class>
    {
        public int GetHashCode(Class co)
        {
            if (co == null)
            {
                return 0;
            }
            return co.ClassId.GetHashCode();
        }

        public bool Equals(Class x1, Class x2)
        {
            if (object.ReferenceEquals(x1, x2))
            {
                return true;
            }
            if (object.ReferenceEquals(x1, null) || object.ReferenceEquals(x2, null))
            {
                return false;
            }
            return x1.ClassId == x2.ClassId;
        }
    }
}