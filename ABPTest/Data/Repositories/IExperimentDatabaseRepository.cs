using ABPTest.Models;

namespace ABPTest.Data.Repositories
{
    public interface IExperimentDatabaseRepository
    {
        Task<ExperimentResult?> GetExperimentResult(string deviceToken, ExperimentType experimentType);
        Task<bool> TryInsertExperimentResult(ExperimentResult experimentResult);
    }
}