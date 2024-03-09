using ABPTest.Models;
using Microsoft.EntityFrameworkCore;

namespace ABPTest.Data.Repositories
{
    public class ExperimentDatabaseRepository : IExperimentDatabaseRepository
    {
        private readonly ExperimentContext _context;

        public ExperimentDatabaseRepository(ExperimentContext context)
        {
            _context = context;
        }

        public async Task<ExperimentResult?> GetExperimentResult(string deviceToken, ExperimentType experimentType)
        {
            var experimentResult = await _context.ExperimentResults
                .FirstOrDefaultAsync(e => e.DeviceToken == deviceToken && e.ExperimentType == experimentType);

            return experimentResult;
        }

        public async Task<bool> TryInsertExperimentResult(ExperimentResult experimentResult)
        {
            _context.ExperimentResults.Add(experimentResult);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}