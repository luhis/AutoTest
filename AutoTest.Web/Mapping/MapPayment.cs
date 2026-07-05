using AutoTest.Domain.StorageModels;
using AutoTest.Web.Models.Save;

namespace AutoTest.Web.Mapping;

public static class MapPayment
{
    public static Payment Map(PaymentSaveModel payment, string currentUserEmail)
    {
        return new Payment(payment.PaidAt, payment.Method, payment.Timestamp, currentUserEmail);
    }
}
