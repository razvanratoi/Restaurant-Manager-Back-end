using MailKit.Net.Smtp;
using MimeKit;
using RestaurantManager.Models;

namespace RestaurantManager.Observer;

public class EmailSender : IObserver
{
    public void Update(Order order, int id)
    {
        if (order.Status == "Paid")
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Razvan Ratoi", "razvi.ratoi@icloud.com"));
            message.To.Add(new MailboxAddress(order.Client.Name, order.Client.Email));
            message.Subject = "Order payment confirmation";
            message.Body = new TextPart("plain") { Text = $"Dear {order.Client.Name},\n\nYour order with the total ${order.Total} has been paid. Thank you for your visit!\n\nBest regards,\nRazvan Ratoi" };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.mail.me.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                client.Authenticate("razvi.ratoi@icloud.com", "tbnr-bpxq-ucup-vupm");
                client.Send(message);
                client.Disconnect(true);
            }
            Console.WriteLine("Email sent successfully!");
        }
    }
}
