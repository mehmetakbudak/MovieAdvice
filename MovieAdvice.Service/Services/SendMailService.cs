using MovieAdvice.Repository.Repositories;
using MovieAdvice.Storage.Consts;
using MovieAdvice.Storage.Models;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace MovieAdvice.Service.Services
{
    public interface ISendMailService
    {
        Task<ServiceResult> Send(MailModel model);
        Task<ServiceResult> SendWithTemplate(MailModel model);
    }

    public class SendMailService : ISendMailService
    {
        private readonly IWebsiteParameterRepository _websiteParameterRepository;
        private readonly IMailTemplateRepository _mailTemplateRepository;

        public SendMailService(
            IWebsiteParameterRepository websiteParameterRepository,
            IMailTemplateRepository mailTemplateRepository)
        {
            _websiteParameterRepository = websiteParameterRepository;
            _mailTemplateRepository = mailTemplateRepository;
        }

        public async Task<ServiceResult> Send(MailModel model)
        {
            ServiceResult result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            try
            {
                MailMessage mailMessage = model.GetMailMessage();
                var emailSetting = model.EmailSetting;

                Int32.TryParse(emailSetting.Port, out int port);

                using (var client = new SmtpClient(emailSetting.Host, port))
                {
                    client.UseDefaultCredentials = false;
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(emailSetting.EmailAddress, emailSetting.Password);
                    
                    await Task.Run(() => client.Send(mailMessage));

                    result.Data = mailMessage;
                    result.Message = $"{mailMessage.To} mail gönderildi.";
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }

                result = new ServiceResult
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = $"Hata: {ex.Message}"
                };
            }
            finally
            {
                Thread.Sleep(MailConsts.SendTimeout);
            }
            return result;
        }

        public async Task<ServiceResult> SendWithTemplate(MailModel model)
        {
            ServiceResult result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            try
            {
                var mailTemplate = await _mailTemplateRepository.GetTemplateByType(model.Data, model.TemplateType);

                if (mailTemplate != null)
                {
                    if (string.IsNullOrEmpty(model.Subject))
                    {
                        model.Subject = mailTemplate.Subject;
                    }
                    if (string.IsNullOrEmpty(model.Body))
                    {
                        model.Body = mailTemplate.Body;
                    }
                }

                MailMessage mailMessage = model.GetMailMessage();
                var emailSetting = model.EmailSetting;

                Int32.TryParse(emailSetting.Port, out int port);

                using (var client = new SmtpClient(emailSetting.Host, port))
                {
                    client.UseDefaultCredentials = false;
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(emailSetting.EmailAddress, emailSetting.Password);

                    await Task.Run(() => client.Send(mailMessage));                    
                    result.Message = $"{mailMessage.To} mail gönderildi.";
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }

                result = new ServiceResult
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = $"Hata: {ex.Message}"
                };
            }
            finally
            {
                Thread.Sleep(MailConsts.SendTimeout);
            }
            return result;
        }
    }
}
