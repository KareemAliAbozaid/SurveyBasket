
namespace SurveyBasket.Services
{
    public interface IPollServices
    {

        IEnumerable<Poll> GetAll();
        Poll? Get(int id);
        Poll Add(Poll poll);
        bool Update(int id,Poll poll);
        bool Delete(int id);
    }
}
