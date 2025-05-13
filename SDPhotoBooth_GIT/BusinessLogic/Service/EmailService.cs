using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using BusinessLogic.Service.IService;
using BusinessLogic.DTO.SendEmailDTO;

namespace BusinessLogic.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendResetPasswordEmail(string toEmail, string resetLink)
        {
            var smtpClient = new SmtpClient(_configuration["EmailSettings:SmtpServer"])
            {
                Port = int.Parse(_configuration["EmailSettings:Port"]),
                Credentials = new NetworkCredential(
                    _configuration["EmailSettings:Username"],
                    _configuration["EmailSettings:Password"]
                ),
                EnableSsl = bool.Parse(_configuration["EmailSettings:EnableSsl"])
            };

            var body = $@"
                <!DOCTYPE html>
                <html lang=""vi"">
                <head>
                  <meta charset=""UTF-8"" />
                  <title>Đặt lại mật khẩu</title>
                </head>
                <body style=""margin:0;padding:0;font-family:Arial,sans-serif;background-color:#f4f4f7;"">
                  <div style=""max-width:600px;margin:40px auto;background:#fff;border-radius:8px;box-shadow:0 2px 8px rgba(0,0,0,0.1);overflow:hidden;"">
                    <div style=""background-color:#fc9330;padding:20px;text-align:center;"">
                      <img src=""https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/44db9204-ac28-4d42-984f-689c93786901.jpg?alt=media"" alt=""Logo Công ty"" style=""max-width:120px;""/>
                    </div>
                    <div style=""padding:30px;color:#51545e;line-height:1.5;"">
                      <h2 style=""margin-top:0;color:#333;"">Xin chào,</h2>
                      <p>Bạn vừa yêu cầu đặt lại mật khẩu cho tài khoản của mình. Để tiếp tục, hãy nhấp vào nút bên dưới:</p>
                      <div style=""text-align:center;margin:30px 0;"">
                        <a href=""{resetLink}"" 
                           style=""display:inline-block;
                                  padding:12px 24px;
                                  background-color:#fc9330;
                                  color:#ffffff !important;
                                  text-decoration:none;
                                  border-radius:4px;
                                  font-weight:bold;"">
                          Đặt lại mật khẩu
                        </a>
                      </div>
                      <p>Nếu bạn không yêu cầu tính năng này, bạn có thể bỏ qua email này. Liên kết sẽ hết hạn sau 24 giờ.</p>
                      <p>Trân trọng,<br/>Đội ngũ Hỗ trợ</p>
                    </div>
                    <div style=""background-color:#f4f4f7;padding:20px;text-align:center;font-size:12px;color:#a8aaaf;"">
                      &copy; 2025 TECH X. Tất cả quyền được bảo lưu.
                    </div>
                  </div>
                </body>
                </html>";

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["EmailSettings:FromEmail"], "Support Team"),
                Subject = "Đặt lại mật khẩu",
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }


        public async Task<bool> SendEmailAsync(string toEmail, string[] photoUrls)
        {
            try
            {
                var smtpClient = new SmtpClient(_configuration["EmailSettings:SmtpServer"])
                {
                    Port = int.Parse(_configuration["EmailSettings:Port"]),
                    Credentials = new NetworkCredential(
                        _configuration["EmailSettings:Username"],
                        _configuration["EmailSettings:Password"]
                    ),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["EmailSettings:FromEmail"]),
                    Subject = "Ảnh của bạn từ PhotoBooth",
                    Body = "Chào bạn, <br><br> Dưới đây là ảnh của bạn:<br><br>",
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                foreach (var photoUrl in photoUrls)
                {
                    mailMessage.Body += $"<a href='{photoUrl}'>{photoUrl}</a><br>";
                }

                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
