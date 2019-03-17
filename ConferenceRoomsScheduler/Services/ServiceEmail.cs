using ConferenceRoomsScheduler.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace ConferenceRoomsScheduler.Services
{
    public class ServiceEmail
    {
        public ServiceEmail() { }
        public void SendConfirmationEmail(User user,string url)
        {

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 25);
            NetworkCredential basicCredential =
                    new NetworkCredential("conferenceroom40@gmail.com", "scheduler2017");
            MailAddress fromAddress = new MailAddress("conferenceroom40@gmail.com");
            
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = basicCredential;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;

            MailMessage message = new MailMessage(fromAddress.ToString(),user.Email);
            message.From = fromAddress;
            message.Subject = "Confirm your account on ConferenceRoomScheduler";
            message.IsBodyHtml = true;
            message.Body = "<h1>You are successfully registered to Conference Room Scheduler!</h1> <h3> Your info: </h3> <br/> <p>User name: " + user.UserName + "</p> <br/> <p> Password:" + user.Password + "<p>"
                + "<br/><p> <strong> Please confirm your account by clicking <a href =\""
                  + url + "\">here</a></strong> <br/><br/> Conference Room Scheduler </p>";
            smtpClient.Send(message);
        }

        public void SendInvitationEmail(List<User> invitedUsers, Reservation reservation)
        {

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 25);
            NetworkCredential basicCredential =
                    new NetworkCredential("conferenceroom40@gmail.com", "scheduler2017");
            MailAddress fromAddress = new MailAddress("conferenceroom40@gmail.com");

            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = basicCredential;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;

            MailMessage message = new MailMessage();
            message.From = fromAddress;
            foreach(User u in invitedUsers)
            {
                message.To.Add(u.Email);
            }
            message.Subject = "Invitation";
            message.IsBodyHtml = true;
            message.Body = "<h3> You are invited to atend event:</h3><br/> <p>Event name: " + reservation.Description + "<br/> Event duration - from: " + reservation.StartDateAndTime +
                " to: " + reservation.EndDateAndTime +" <br/> at: Conference Room " + reservation.ConfRoomId + ". <br/> User: " + reservation.CreatorId + " called you. <p>"+
                "<br/> <br/> Conference Room Scheduler </p>";
            smtpClient.Send(message);
        }
    }
}