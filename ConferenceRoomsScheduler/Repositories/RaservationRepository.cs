using ConferenceRoomsScheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferenceRoomsScheduler.Repositories
{
    public class RaservationRepository
    {
        private ApplicationDbContext dataBaseContext = new ApplicationDbContext();
        public ICollection<User> AddAtendees(string[] users)
        {
            List<User> invitedUsers = new List<User>();
            for (int index = 0; index < users.Count(); index++)
            {
                string user = users[index];
                foreach (User item in dataBaseContext.Users)
                {
                    if ((item.UserName).Equals(user))
                    {
                        if (!(invitedUsers.Contains(item)))
                        {
                            invitedUsers.Add(item);
                        }

                    }
                }
            }
            return invitedUsers;
        }
    }
}