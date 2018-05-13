using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheLearningCenterReworked.Context;
using TheLearningCenterReworked.Models;
using System.Web.Caching;

namespace TheLearningCenterReworked.Repositories
{
    public interface IClassRepository
    {
        IEnumerable<Class> Classes { get; }
    }

    public class ClassRepository : IClassRepository
    {
        public IEnumerable<Class> Classes
        {
            get
            {
                if (HttpContext.Current.Cache["Myclasses"] == null)
                {
                    var items = new TheLearningCenterReworkedEntities().Classes.ToList();

                    HttpContext.Current.Cache.Insert("MyClasses",
                                                 items,
                                                 null,
                                                 //DateTime.Now.AddSeconds(30),     //Absolute expiration
                                                 Cache.NoAbsoluteExpiration,
                                                 new TimeSpan(0, 0, 30));              //Sliding expiration
                }
                return (IEnumerable<Class>)HttpContext.Current.Cache["MyClasses"];


            }
        }
        public Class DropClass(int classId, int userId)
        {
            var learningCenterEntity = new TheLearningCenterReworkedEntities();
            List<Class> userClassEnrolled = new List<Class>();

            int currentUserId = new int();
            foreach (var userItem in learningCenterEntity.Users)
            {
                if (userItem.UserId == userId)
                {
                    currentUserId = userItem.UserId;
                    foreach (var user in learningCenterEntity.Users)
                    {
                        foreach (var classItem in user.Classes)
                        {
                            if (currentUserId == user.UserId) { userClassEnrolled.Add(classItem); }
                        }
                    }
                }
            }
            return null;
        }
    }
}