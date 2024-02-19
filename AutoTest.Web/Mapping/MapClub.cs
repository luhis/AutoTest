using System.Linq;
using AutoTest.Domain.StorageModels;
using AutoTest.Web.Models.Save;

namespace AutoTest.Web.Mapping
{
    public static class MapClub
    {
        public static Club Map(ulong clubId, ClubSaveModel model)
        {
            var c = new Club(clubId, model.ClubName, model.ClubPaymentAddress, model.Website);
            c.SetAdminEmails(model.AdminEmails.Select(a => new AuthorisationEmail(a.Email)).ToArray());
            return c;
        }

        public static Payment Map(PaymentSaveModel payment, string currentUserEmail)
        {
            return new Payment(payment.PaidAt, payment.Method, payment.Timestamp, currentUserEmail);
        }


        public static Notification Map(ulong notificationId, ulong eventId, string emailAddress, NotificationSaveModel notification)
        {
            var p = new Notification(notificationId, eventId, notification.Message, notification.Created, emailAddress);
            return p;
        }
    }
}
