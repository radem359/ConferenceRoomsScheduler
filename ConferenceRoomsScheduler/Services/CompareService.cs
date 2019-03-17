using ConferenceRoomsScheduler.Models;

namespace ConferenceRoomsScheduler.Services
{
    public class CompareService
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public CompareService()
        {
        }

        public bool CompareReservations(Reservation reservation)
        {
            foreach (Reservation item in db.Reservations)
            {
                if (item.ConfRoomId == reservation.ConfRoomId && item.Id != reservation.Id)
                {
                    if (reservation.StartDateAndTime.Equals(item.StartDateAndTime))
                    {
                        return true;
                    }
                    else if ((reservation.EndDateAndTime.Equals(item.EndDateAndTime)))
                    {
                        return true;
                    }
                    else if (!(reservation.StartDateAndTime.Equals(item.StartDateAndTime)) && (reservation.EndDateAndTime.Equals(item.EndDateAndTime)))
                    {
                        return true;
                    }
                    else if (reservation.StartDateAndTime < item.StartDateAndTime && reservation.EndDateAndTime > item.StartDateAndTime)
                    {
                        return true;
                    }
                    else if (!(reservation.StartDateAndTime.Equals(item.StartDateAndTime)) && reservation.StartDateAndTime > item.StartDateAndTime && reservation.StartDateAndTime < item.EndDateAndTime)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool CompareStartAndEnd(Reservation reservation)
        {
            if (!reservation.StartDateAndTime.Date.Equals(reservation.EndDateAndTime.Date))
            {
                return false;
            }
            return true;
        }
    }
}