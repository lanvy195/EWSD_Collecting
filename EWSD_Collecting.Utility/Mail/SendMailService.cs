﻿using System;
using System.Threading.Tasks;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using EWSD_Collecting.Utility.Mail;

public class SendMailService : ISendMailService
{
    private readonly MailSettings mailSettings;

    private readonly ILogger<SendMailService> logger;


    // mailSetting được Inject qua dịch vụ hệ thống
    // Có inject Logger để xuất log
    public SendMailService(IOptions<MailSettings> _mailSettings, ILogger<SendMailService> _logger)
    {
        mailSettings = _mailSettings.Value;
        logger = _logger;
        logger.LogInformation("Create SendMailService");
    }

    // Gửi email, theo nội dung trong mailContent
    public async Task SendMail(MailContent mailContent)
    {
        var email = new MimeMessage();
        email.Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail);
        email.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));
        email.To.Add(MailboxAddress.Parse(mailContent.To));
        email.Subject = mailContent.Subject;


        var builder = new BodyBuilder();
        builder.HtmlBody = mailContent.Body;
        email.Body = builder.ToMessageBody();

        // dùng SmtpClient của MailKit
        using var smtp = new MailKit.Net.Smtp.SmtpClient();

        try
        {
            smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
            await smtp.SendAsync(email);
        }
        catch (Exception ex)
        {
            // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
            System.IO.Directory.CreateDirectory("mailssave");
            var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
            await email.WriteToAsync(emailsavefile);

            logger.LogInformation("ERROR mail, mailsave - " + emailsavefile);
            logger.LogError(ex.Message);
        }

        smtp.Disconnect(true);

        logger.LogInformation("send mail to " + mailContent.To);

    }

    public async Task IdeaSubmissionEmail(string email, string fullname, string title)
    {
        await SendMail(new MailContent()
        {
            To = "hiepplmgbs16019@fpt.edu.vn",
            Subject = "Create idea for collecting idea",
            Body = "Thank for submitting your idea to website"
        });
    }
    public async Task CommentIdeaSubmissionEmail(string email, string fullname, string title)
    {
        await SendMail(new MailContent()
        {
            To = "hiepplmgbs16019@fpt.edu.vn",
            Subject = "New comment form collecting idea",
            Body = "New comment has been post to your idea"
        });
    }
}