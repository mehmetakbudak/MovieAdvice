using Microsoft.EntityFrameworkCore;
using MovieAdvice.Storage.Enums;
using MovieAdvice.Storage.Models;
using System.Threading.Tasks;

namespace MovieAdvice.Repository.Repositories
{
    public interface IMailTemplateRepository
    {
        Task<TemplateResponseModel> GetTemplateByType<T>(T data, TemplateType type);
    }

    public class MailTemplateRepository : IMailTemplateRepository
    {
        private readonly MovieAdviceContext _context;

        public MailTemplateRepository(MovieAdviceContext context)
        {
            _context = context;
        }

        public async Task<TemplateResponseModel> GetTemplateByType<T>(T data, TemplateType type)
        {
            TemplateResponseModel result = null;

            var mailTemplate = await _context.MailTemplates.FirstOrDefaultAsync(x => x.TemplateType == type);

            if (mailTemplate != null)
            {
                var properties = data.GetType().GetProperties();

                var body = mailTemplate.Body;

                foreach (var property in properties)
                {
                    body = body.Replace("{{" + property.Name + "}}", property.GetValue(data).ToString());
                }

                result = new TemplateResponseModel()
                {
                    Subject = mailTemplate.Title,
                    Body = body
                };
            }
            return result;
        }
    }
}
