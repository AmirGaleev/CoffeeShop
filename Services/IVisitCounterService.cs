namespace CoffeeShop.Services
{
    public interface IVisitCounterService
    {
        int GetVisitCount();
        void IncrementVisitCount();
    }
}