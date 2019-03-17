using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ConferenceRoomsScheduler.Models;
using ConferenceRoomsScheduler.ViewModels;

namespace ConferenceRoomsScheduler.Controllers
{
    public class ReservationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Services.ServiceEmail emailService = new Services.ServiceEmail();
        private Services.CompareService compareService = new Services.CompareService();

        private void FillUsers()
        {
            List<SelectListItem> users = new List<SelectListItem>();
            foreach (User user in db.Users)
            {
                if(!user.UserName.Equals(User.Identity.Name))
                    users.Add(new SelectListItem { Text = user.UserName });
            }
            ViewBag.Users = users;
         }
        // GET: Reservations/Create
        public ActionResult CreateReservation(int confRoomId)
        {
            FillUsers();
            ViewBag.ConfRoomId = confRoomId;
            ViewBag.CreatorId = User.Identity.Name;
            return View();
        }

        // POST: Reservations/CreateReservation
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReservation(ReservationViewModel model)
        {
            var reservation = new Reservation { Id=Guid.NewGuid(), ConfRoomId=model.ConfRoomId,CreatorId=model.CreatorId,Description=model.Description,StartDateAndTime=model.StartDateAndTime, EndDateAndTime = model.EndDateAndTime };
            if (model.InvitedUserNames != null)
            {
                reservation.InvitedUserNames = new List<Models.User>();
                foreach(string s in model.InvitedUserNames)
                {
                    User u = (User)db.Users.FirstOrDefault(x => x.UserName.Equals(s));
                    reservation.InvitedUserNames.Add(u);
                }
                callInvitedUsers(reservation);
            }
            if (!compareService.CompareStartAndEnd(reservation))
            {
                TempData["Message"] = "Choose the same day for start and end date.";
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
            else if (compareService.CompareReservations(reservation)) {
                return RedirectToAction("ShowTableReserved", "ConferenceRoom", new { roomId = reservation.ConfRoomId });
            }
            else {
                db.Reservations.Add(reservation);
                db.SaveChanges();
                return RedirectToAction("ShowTable", "ConferenceRoom", new { roomId = reservation.ConfRoomId });
            }
        }
        
        private void callInvitedUsers(Reservation res)
        {
            emailService.SendInvitationEmail(res.InvitedUserNames,res);
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