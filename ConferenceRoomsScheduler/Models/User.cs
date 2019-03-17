using System.Collections.Generic;

namespace ConferenceRoomsScheduler.Models
{
    public class User : ApplicationUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}