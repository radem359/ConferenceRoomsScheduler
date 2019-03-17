using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ConferenceRoomsScheduler.Models
{
    public class Reservation : IdentificatorClass
    {
        [DisplayName("Description")]
        public string Description { get; set; }
        public int ConfRoomId { get; set; }
        [DisplayName("Start Time")]
        public DateTime StartDateAndTime { get; set; }
        [DisplayName("End Time")]
        public DateTime EndDateAndTime { get; set; }
        [DisplayName("Creator of reservation")]
        public string CreatorId { get; set; }
        public List<User> InvitedUserNames { get; set; }
    }
}