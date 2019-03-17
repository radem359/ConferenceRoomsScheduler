using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using ConferenceRoomsScheduler.Models;
using System.Linq;
using ConferenceRoomsScheduler.ViewModels;

namespace ConferenceRoomsScheduler.Controllers
{
    public class ConferenceRoomController : Controller
    {
        private ApplicationDbContext myContext = new ApplicationDbContext();
        private ConferenceRoom choosenRoom = new ConferenceRoom();
        private Services.ServiceEmail emailService = new Services.ServiceEmail();
        private Services.CompareService compareService = new Services.CompareService();

        public ActionResult ShowTable(int roomId)
        {
            List<Reservation> listToShow = new List<Reservation>();
            ViewBag.ConfRoomId = roomId;
            foreach (var res in myContext.Reservations)
            {
                if (res.ConfRoomId == roomId)
                    listToShow.Add(res);
            }
            return View("TableOfConferenceRoom", listToShow);
        }

        public ActionResult ShowTableReserved(int roomId)
        {
            List<Reservation> listToShow = new List<Reservation>();
            ViewBag.ConfRoomId = roomId;
            ViewBag.Message = "Conference room " + roomId + " is reserved at that time. Please see the schedule again.";
            foreach (var res in myContext.Reservations)
            {
                if (res.ConfRoomId == roomId)
                    listToShow.Add(res);
            }
            return View("TableOfConferenceRoom", listToShow);
        }

        [Authorize]
        public ActionResult ChooseRoom()
        {
            List<SelectListItem> roomNames = new List<SelectListItem>();
            roomNames.Add(new SelectListItem { Text = " " });
            foreach (ConferenceRoom room in myContext.ConferenceRooms)
            {
                roomNames.Add(new SelectListItem { Text = room.Name });
            }

            ViewBag.RoomNames = roomNames;
            return View();
        }

        [HttpPost]
        public ActionResult PerformChoosing()
        {
            string roomName = Request.Form["RoomNames"].ToString();
            if (roomName.Equals(""))
            {
                return View("RoomIsNotChoosen");
            }
            else
            {
                string[] splitted = roomName.Split(' ');
                int choosenId = 0;
                try
                {
                    choosenId = int.Parse(splitted[2]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
                
                choosenRoom = myContext.ConferenceRooms.Find(choosenId);
                ViewBag.Title = "Table of reservations for " + choosenRoom.Name;
                ViewBag.ConfRoomId = choosenId;
                List<Reservation> listToShow = new List<Reservation>();
                foreach (var res in myContext.Reservations)
                {
                    if (res.ConfRoomId == choosenId)
                        listToShow.Add(res);
                }
                return View("TableOfConferenceRoom", listToShow);
            }
        }
       
        // GET: ConferenceRoom/Delete/5

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Reservation reservation = myContext.Reservations.Find(id);
            ViewBag.ConfRoomId = reservation.ConfRoomId;
            if (reservation == null)
            {
                return HttpNotFound();
            }
            if (User.Identity.Name.Equals(reservation.CreatorId))
            {
                return View(reservation);
            }else
            {
                return View("NotAuthorized",reservation);
            }
        }

        // POST: ConferenceRoom/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Reservation reservation = myContext.Reservations.Find(id);
            myContext.Reservations.Remove(reservation);
            myContext.SaveChanges();
            return RedirectToAction("ShowTable", "ConferenceRoom", new { roomId = reservation.ConfRoomId });
        }

        public void FillUsers(Reservation res)
        {
            List<SelectListItem> users = new List<SelectListItem>();
            List<SelectListItem> invited = new List<SelectListItem>();
            if (res.InvitedUserNames != null)
            {
                foreach (User u in res.InvitedUserNames)
                {
                    invited.Add(new SelectListItem { Text = u.UserName, Value=u.UserName });
                }
            }
            foreach (User user in myContext.Users)
            {
                if (!user.UserName.Equals(User.Identity.Name)&&(!res.InvitedUserNames.Contains(user)))
                    users.Add(new SelectListItem { Text = user.UserName, Value=user.UserName });
            }
            ViewBag.Users = users;
            ViewBag.InvitedUserNames = invited;
        }

        // GET: Reservations/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = await myContext.Reservations.Include(x=> x.InvitedUserNames).FirstOrDefaultAsync(x => x.Id==id);
            FillUsers(reservation);
            
            ViewBag.ConfRoomId = reservation.ConfRoomId;
            ViewBag.CreatorId = reservation.CreatorId;

            if (reservation == null)
            {
                return HttpNotFound();
            }
            if (User.Identity.Name.Equals(reservation.CreatorId))
            {
                List<string> userNames = new List<string>();
                if (reservation.InvitedUserNames != null)
                {
                    foreach(User u in reservation.InvitedUserNames)
                    {
                        userNames.Add(u.UserName);
                    }
                }
                ReservationViewModel reservationViewModel = new ReservationViewModel {
                    Id = reservation.Id,
                    StartDateAndTime = reservation.StartDateAndTime,
                    EndDateAndTime = reservation.EndDateAndTime,
                    ConfRoomId = reservation.ConfRoomId,
                    CreatorId = reservation.CreatorId,
                    Description = reservation.Description,
                    InvitedUserNames = userNames
                };
                
                return View(reservationViewModel);
            }
            else
            {
                ViewBag.Title = "Not authorized";
                return View("NotAuthorized", reservation);
            }

        }
        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ReservationViewModel reservation)
        {
            if (ModelState.IsValid)
            {
                
                List<User> invited = new List<Models.User>();
                if (reservation.InvitedUserNames != null)
                {
                    foreach (string s in reservation.InvitedUserNames)
                    {
                        User u = (User)myContext.Users.FirstOrDefault(x => x.UserName.Equals(s));
                        invited.Add(u);
                    }
                }
                Reservation newRes = new Reservation
                {
                    Id = reservation.Id,
                    ConfRoomId = reservation.ConfRoomId,
                    CreatorId = reservation.CreatorId,
                    Description = reservation.Description,
                    EndDateAndTime = reservation.EndDateAndTime,
                    StartDateAndTime = reservation.StartDateAndTime,
                    InvitedUserNames = invited
                };
                Reservation oldRes = myContext.Reservations.Find(reservation.Id);
                if (!compareService.CompareStartAndEnd(newRes))
                {
                    TempData["Message"] = "Choose the same day for start and end date.";
                    return Redirect(Request.UrlReferrer.PathAndQuery);
                }else if (compareService.CompareReservations(newRes))
                {
                    return RedirectToAction("ShowTableReserved", "ConferenceRoom", new { roomId = reservation.ConfRoomId });
                }
                else
                {
                    myContext.Reservations.Remove(oldRes);
                    myContext.Reservations.Add(newRes);
                    Services.ServiceEmail emailService = new Services.ServiceEmail();
                    if (newRes.InvitedUserNames.Count != 0)
                    {
                        emailService.SendInvitationEmail(newRes.InvitedUserNames, newRes);
                    }
                    myContext.SaveChanges();
                    return RedirectToAction("ShowTable", "ConferenceRoom", new { roomId = reservation.ConfRoomId });
                }
            }
            return View(reservation);
        }
    }
}