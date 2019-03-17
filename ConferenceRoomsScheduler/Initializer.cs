using ConferenceRoomsScheduler.Models;
using System.Collections.Generic;

namespace ConferenceRoomsScheduler
{
    public class Initializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        public Initializer()
        {
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var rooms = new List<ConferenceRoom>
            {
                new ConferenceRoom { Id=1, Name="Conference Room 1"},
                new ConferenceRoom { Id=2, Name="Conference Room 2"},
                new ConferenceRoom { Id=3, Name="Conference Room 3"},
                new ConferenceRoom { Id=4, Name="Conference Room 4"},
                new ConferenceRoom { Id=5, Name="Conference Room 5"}
            };
            rooms.ForEach(room => context.ConferenceRooms.Add(room));
            context.SaveChanges();
        }
    }
}