namespace MovieAdvice.Repository.Repositories
{
    public interface IUserRepository
    {

    }

    public class UserRepository: IUserRepository
    {
        private readonly MovieContext _context;

        public UserRepository(MovieContext context)
        {
            _context = context;
        }


    }
}
