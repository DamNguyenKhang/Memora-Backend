using Application.Abstractions.Services;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Application.Services;

public class EmailService(IConfiguration configuration) : IEmailService
{
    public async Task SendVerificationEmailAsync(string toEmail, string verifyToken)
    {
        // 🔹 Lấy cấu hình
        var emailSection = configuration.GetSection("EmailSettings");

        var host = emailSection["Host"];
        var port = int.Parse(emailSection["Port"]!);
        var senderName = emailSection["SenderName"];
        var senderEmail = emailSection["SenderEmail"];
        var username = emailSection["Username"];
        var password = emailSection["Password"];

        // 🔹 Verify link
        var baseUrl = configuration["App:VerifyEmailUrl"];
        var verifyLink = $"{baseUrl}?token={verifyToken}&email={Uri.EscapeDataString(toEmail)}";

        // 🔹 HTML email
        var htmlBody = $"""
        <div style="font-family:Arial,sans-serif;max-width:600px;margin:auto">
            <h2>🎉 Chào mừng bạn đến với Memora</h2>
            <p>Cảm ơn bạn đã đăng ký tài khoản.</p>
            <p>Vui lòng nhấn nút bên dưới để xác thực email:</p>

            <p style="text-align:center;margin:30px 0">
                <a href="{verifyLink}"
                   style="background:#4f46e5;color:#fff;
                          padding:12px 24px;
                          text-decoration:none;
                          border-radius:6px;
                          display:inline-block">
                   Xác thực email
                </a>
            </p>

            <p>Link này sẽ hết hạn sau <b>15 phút</b>.</p>
            <p>Nếu bạn không đăng ký, vui lòng bỏ qua email này.</p>

            <hr />
            <p style="font-size:12px;color:#666">
                © {DateTime.UtcNow.Year} Memora
            </p>
        </div>
        """;

        // 🔹 Build email
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(senderName, senderEmail));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = "Xác thực email đăng ký Memora";

        message.Body = new BodyBuilder
        {
            HtmlBody = htmlBody
        }.ToMessageBody();

        // 🔹 Send
        using var smtp = new MailKit.Net.Smtp.SmtpClient();

        await smtp.ConnectAsync(host, port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(username, password);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }
}
