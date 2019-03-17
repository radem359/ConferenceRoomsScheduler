using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ConferenceRoomsScheduler.ViewModels
{
    public class ReservationViewModel
    {
        public Guid Id { get; set; }
        [Required]
        [Display(Name="Description")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Start Time")]
        public DateTime StartDateAndTime { get; set; }
        [Required]
        [Display(Name="End Time")]
        public DateTime EndDateAndTime { get; set; }
        [DisplayName("Creator of reservation")]
        public string CreatorId { get; set; }
        public int ConfRoomId { get; set; }

        public List<string> InvitedUserNames { get; set; }
    }
}