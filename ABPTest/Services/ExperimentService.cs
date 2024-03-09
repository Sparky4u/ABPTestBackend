using System.Data.SqlTypes;
using ABPTest.Data.Repositories;
using ABPTest.Models;

namespace ABPTest.Services
{
    public class ExperimentService : IExperimentService
    {
        private static readonly string[] Colors = { "#FF0000", "#00FF00", "#0000FF" };
        private readonly IExperimentDatabaseRepository _experimentsRepository;

        public ExperimentService(IExperimentDatabaseRepository experimentsRepository)
        {
            _experimentsRepository = experimentsRepository;
        }

        public async Task<string> GetButtonColor(string deviceToken)
        {
            var experimentResult =
                await _experimentsRepository.GetExperimentResult(deviceToken, ExperimentType.ButtonColor);
            if (experimentResult != null)
                return experimentResult.Value;

            var color = GetRandomColor();
            if (!await _experimentsRepository.TryInsertExperimentResult(new ExperimentResult
                    { DeviceToken = deviceToken, ExperimentType = ExperimentType.ButtonColor, Value = color }))
                throw new SqlNotFilledException();

            return color;
        }

        public async Task<string> GetPrice(string deviceToken)
        {
            var experimentResult =
                await _experimentsRepository.GetExperimentResult(deviceToken, ExperimentType.Price);
            if (experimentResult != null)
                return experimentResult.Value;

            var price = GetRandomPrice();
            if (!await _experimentsRepository.TryInsertExperimentResult(new ExperimentResult
                    { DeviceToken = deviceToken, ExperimentType = ExperimentType.Price, Value = price }))
                throw new SqlNotFilledException();

            return price;
        }

        private static string GetRandomColor()
        {
            var random = new Random();
            return Colors[random.Next(Colors.Length)];
        }

        private static string GetRandomPrice()
        {
            var random = new Random();
            var randomNumber = random.Next(1, 101);

            return randomNumber switch
            {
                <= 75 => "10",
                <= 85 => "20",
                <= 90 => "50",
                _ => "5"
            };
        }
    }
}