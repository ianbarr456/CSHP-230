using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheLearningCenterReworked.Context;

namespace TheLearningCenterReworked.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> Users { get; }
        User LogIn(string email, string password);
        User Register(string email, string password);
    }

    public class UserRepository : IUserRepository
    {
        public IEnumerable<User> Users
        {
            get
            {
                List<User> items = new TheLearningCenterReworkedEntities().Users.ToList();

                return items;
            }
        }

        public User LogIn(string email, string password)
        {
            return Users.SingleOrDefault(t => t.UserEmail.ToLower() == email.ToLower()
                                        && t.UserPassword == password);
        }

        public User Register(string email, string password)
        {
            var item = Users.SingleOrDefault(t => t.UserEmail.ToLower() == email.ToLower());

            if (item == null)
            {
                var UserModelDb = new User
                {
                    UserEmail = email,
                    UserPassword = password
                };

                DatabaseManager.Instance.Users.Add(UserModelDb);
                DatabaseManager.Instance.SaveChanges();

                return UserModelDb;
            }
            else { return null; }
        }
    }
}