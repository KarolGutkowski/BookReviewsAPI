namespace BookReviewsAPI.Registration
{
    public interface IRegistrationHelper
    {
        bool TryToRegisterUser(string username, string password);
    }
}
