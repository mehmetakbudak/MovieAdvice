using Microsoft.EntityFrameworkCore;
using MovieAdvice.Storage.Consts;
using MovieAdvice.Storage.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MovieAdvice.Repository.Repositories
{
    public interface IWebsiteParameterRepository
    {
        Task<EmailSettingModel> GetEmailSettings();
    }

    public class WebsiteParameterRepository : IWebsiteParameterRepository
    {
        private readonly MovieAdviceContext _context;

        public WebsiteParameterRepository(MovieAdviceContext context)
        {
            _context = context;
        }

        public async Task<EmailSettingModel> GetEmailSettings()
        {
            var emailSetting = await GetParametersByType<EmailSettingModel>(WebsiteParameterTypes.EmailSettings);
            return emailSetting;
        }
        private  async Task<T> GetParametersByType<T>(string code) where T : class, new()
        {
            T model = new T();

            var parentParameter = await _context.WebsiteParameters.FirstOrDefaultAsync(p => p.Code == code && p.ParentId == null);

            if (parentParameter != null)
            {
                var parameters = await _context.WebsiteParameters.Where(x => x.ParentId == parentParameter.Id).ToListAsync();

                var properties = typeof(T).GetProperties();

                foreach (var property in properties)
                {
                    var parameter = parameters.FirstOrDefault(x => x.Code == property.Name);
                    if (parameter != null)
                    {
                        property.SetValue(model, parameter.Value);
                    }
                }
            }
            return model;
        }
    }
}
