using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MovieAdvice.Storage.Enums;
using MovieAdvice.Storage.Consts;
using MovieAdvice.Storage.Models;
using System.Collections.Generic;
using MovieAdvice.Infrastructure;
using MovieAdvice.Storage.Entities;
using MovieAdvice.Repository.Repositories;
using MovieAdvice.Infrastructure.Exceptions;
using MovieAdvice.Infrastructure.Configurations;

namespace MovieAdvice.Service.Services
{
    public interface IMovieService
    {
        PagedResponse<List<Movie>> GetByPagination(PaginationFilter filter);
        Task<UserMovieDetailModel> GetMovieDetail(int id, int? userId);
        Task AddRange(List<MovieApiItemModel> list);
        Task<ServiceResult> Advice(MovieAdviceModel model, int? userId);
        Task<ServiceResult> Rate(MovieRateModel model, int? userId);
        Task<ServiceResult> Note(MovieNoteModel model, int? userId);
    }

    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IUserMovieRateRepository _userMovieRateRepository;
        private readonly IUserMovieNoteRepository _userMovieNoteRepository;
        private readonly IWebsiteParameterRepository _websiteParameterRepository;
        private readonly IMailTemplateRepository _mailTemplateRepository;
        private readonly IPublisherService _publisherService;
        private readonly ISendMailService _sendMailService;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MovieService(
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            ISendMailService sendMailService,
            IMovieRepository movieRepository,
            IPublisherService publisherService,
            IUserMovieRateRepository userMovieRateRepository,
            IUserMovieNoteRepository userMovieNoteRepository,
            IWebsiteParameterRepository websiteParameterRepository,
            IMailTemplateRepository mailTemplateRepository)
        {
            _movieRepository = movieRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _sendMailService = sendMailService;
            _publisherService = publisherService;
            _userMovieRateRepository = userMovieRateRepository;
            _userMovieNoteRepository = userMovieNoteRepository;
            _websiteParameterRepository = websiteParameterRepository;
            _mailTemplateRepository = mailTemplateRepository;
        }

        public PagedResponse<List<Movie>> GetByPagination(PaginationFilter filter)
        {
            var list = _movieRepository.GetAll();

            var pagedReponse = PaginationHelper.CreatePagedReponse<Movie>(list, filter);

            return pagedReponse;
        }

        public async Task<UserMovieDetailModel> GetMovieDetail(int id, int? userId)
        {
            var movie = await _movieRepository.GetWithRatesById(id);

            if (movie == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }

            var model = new UserMovieDetailModel
            {
                BackdropPath = movie.BackdropPath,
                CreatedDate = movie.CreatedDate,
                Id = movie.Id,
                IsAdult = movie.IsAdult,
                IsVideo = movie.IsVideo,
                OriginalLanguage = movie.OriginalLanguage,
                OriginalTitle = movie.OriginalTitle,
                Overview = movie.Overview,
                Popularity = movie.Popularity,
                PosterPath = movie.PosterPath,
                ReleaseDate = movie.ReleaseDate,
                Title = movie.Title,
                AverageRate = movie.UserMovieRates.Count > 0 ?
                              movie.UserMovieRates.Average(x => x.Rate) : 0
            };

            if (!userId.HasValue)
            {
                return model;
            }

            var userRate = movie.UserMovieRates.FirstOrDefault(x => x.UserId == userId);
            model.UserRate = userRate?.Rate;

            var userNotes = await _userMovieNoteRepository.GetUserMovieNotes(id, userId.Value);

            if (userNotes != null && userNotes.Count > 0)
            {
                model.Notes = userNotes.Select(x => new UserMovieNoteModel
                {
                    CreatedDate = x.CreatedDate,
                    Id = x.Id,
                    Note = x.Note
                }).ToList();
            }

            return model;
        }

        public async Task AddRange(List<MovieApiItemModel> list)
        {
            var movies = await _movieRepository.AddRange(list);
            if (movies.Count > 0)
            {
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task<ServiceResult> Advice(MovieAdviceModel model, int? userId)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            if (model == null)
            {
                throw new BadRequestException("model null olamaz.");
            }

            if (!userId.HasValue)
            {
                throw new NotAcceptableException("Kullanıcı bilgisi zorunludur.");
            }
            var user = _userRepository.GetById(userId.Value);

            if (user == null)
            {
                throw new NotFoundException("Kullanıcı bulunamadı.");
            }

            var movie = await _movieRepository.GetWithRatesById(model.MovieId);

            if (movie == null)
            {
                throw new NotFoundException("Kayıt bulunamadı.");
            }

            var emailSetting = await _websiteParameterRepository.GetEmailSettings();

            if (emailSetting == null)
            {
                throw new NotFoundException("Email ayarları bulunamadı. Mail gönderilemedi.");
            }

            var mailModel = new MailModel
            {
                EmailAddress = model.Email,
                TemplateType = TemplateType.MovieAdvice,
                To = model.Email,
                EmailSetting = emailSetting,
                Data = new
                {
                    FullName = $"{user.FirstName} {user.LastName}",
                    Title = movie.Title,
                    Overview = movie.Overview,
                    AverageRate = movie.UserMovieRates.Count > 0 ?
                                      movie.UserMovieRates.Average(x => x.Rate) : 0
                }
            };

            if (WebApiConfiguration.RabbitMQSettings.Enabled)
            {
                var queueName = RabbitMQConsts.RabbitMqConstsList.QueueNameEmail.ToString();

                var mailTemplate = await _mailTemplateRepository.GetTemplateByType(mailModel.Data, mailModel.TemplateType);

                if (mailTemplate != null)
                {
                    mailModel.Subject = mailTemplate.Subject;
                    mailModel.Body = mailTemplate.Body;
                }
                _publisherService.Enqueue(new List<MailModel> { mailModel },
                                                  WebApiConfiguration.RabbitMQSettings,
                                                  queueName);
            }
            else
            {


                result = await _sendMailService.SendWithTemplate(mailModel);
            }
            return result;
        }

        public async Task<ServiceResult> Rate(MovieRateModel model, int? userId)
        {
            if (model == null)
            {
                throw new BadRequestException("model null olamaz.");
            }

            if (!userId.HasValue)
            {
                throw new NotAcceptableException("Kullanıcı bilgisi zorunludur.");
            }

            var result = await _userMovieRateRepository.CreateOrUpdate(model, userId.Value);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                await _unitOfWork.SaveAsync();
            }

            return result;
        }

        public async Task<ServiceResult> Note(MovieNoteModel model, int? userId)
        {
            if (model == null)
            {
                throw new BadRequestException("model null olamaz.");
            }

            if (!userId.HasValue)
            {
                throw new NotAcceptableException("Kullanıcı bilgisi zorunludur.");
            }

            var result = await _userMovieNoteRepository.Add(model, userId.Value);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                await _unitOfWork.SaveAsync();
            }

            return result;
        }
    }
}