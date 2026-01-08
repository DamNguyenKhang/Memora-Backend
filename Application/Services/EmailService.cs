using Application.Abstractions.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Application.Services;

public class EmailService : IEmailService
{
    private readonly string _host;
    private readonly int _port;
    private readonly string _senderName;
    private readonly string _senderEmail;
    private readonly string _username;
    private readonly string _password;
    private readonly string _verifyEmailBaseUrl;

    public EmailService(IConfiguration configuration)
    {
        var emailSection = configuration.GetSection("Email");

        _host = emailSection["Host"]!;
        _port = int.Parse(emailSection["Port"]!);
        _senderName = emailSection["SenderName"]!;
        _senderEmail = emailSection["SenderEmail"]!;
        _username = emailSection["Username"]!;
        _password = emailSection["Password"]!;

        _verifyEmailBaseUrl = $"{configuration["App:FrontendBaseUrl"]!}{configuration["App:VerifyEmailPath"]!}";
    }

    public async Task SendVerificationEmailAsync(string toEmail, string verifyToken)
    {
        var verifyLink =
            $"{_verifyEmailBaseUrl}?token={verifyToken}&email={Uri.EscapeDataString(toEmail)}";

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

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_senderName, _senderEmail));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = "Xác thực email đăng ký Memora";

        message.Body = new BodyBuilder
        {
            HtmlBody = htmlBody
        }.ToMessageBody();

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_host, _port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_username, _password);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }
}
