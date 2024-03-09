namespace ABPTest.Services
{
    public interface IExperimentService
    {
        Task<string> GetButtonColor(string deviceToken);
        Task<string> GetPrice(string deviceToken);
    }
}
