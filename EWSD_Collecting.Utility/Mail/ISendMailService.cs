using System.Threading.Tasks;
using EWSD_Collecting.Utility.Mail;


public interface ISendMailService
{
    Task SendMail(MailContent mailContent);

    Task IdeaSubmissionEmail(string email, string fullname, string title);
    Task CommentIdeaSubmissionEmail(string email, string fullname, string title);

}