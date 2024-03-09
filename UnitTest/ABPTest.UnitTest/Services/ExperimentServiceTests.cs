using ABPTest.Data.Repositories;
using ABPTest.Models;
using ABPTest.Services;
using AutoFixture;
using Moq;

namespace ABPTest.UnitTest.Services
{
    public class ExperimentServiceTests
    {
        private static readonly string[] Colors = { "#FF0000", "#00FF00", "#0000FF" };
        private static readonly string[] Prices = { "5", "10", "20", "50" };
        private Mock<IExperimentDatabaseRepository> _experimentRepositoryMock;
        private ExperimentService _exepimentService;
        private Fixture _fixture;

        [OneTimeSetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _experimentRepositoryMock = new Mock<IExperimentDatabaseRepository>();
            _exepimentService = new ExperimentService(_experimentRepositoryMock.Object);
        }

        [TearDown]
        public void Teardown()
        {
            _experimentRepositoryMock.Reset();
        }

        #region ButtonColor Tests

        [Test]
        public async Task TestNewDeviceButtonColor()
        {
            var deviceToken = GetNewRandomDeviceToken();
            _experimentRepositoryMock.Setup(x => x.GetExperimentResult(deviceToken, ExperimentType.ButtonColor))
                .ReturnsAsync((ExperimentResult?)null);
            _experimentRepositoryMock.Setup(x => x.TryInsertExperimentResult(It.IsAny<ExperimentResult>()))
                .ReturnsAsync(true);

            var result = await _exepimentService.GetButtonColor(deviceToken);

            _experimentRepositoryMock.Verify(x => x.GetExperimentResult(deviceToken, ExperimentType.ButtonColor),
                Times.Exactly(1));
            _experimentRepositoryMock.Verify(x => x.GetExperimentResult(deviceToken, ExperimentType.Price),
                Times.Never);
            _experimentRepositoryMock.Verify(x => x.TryInsertExperimentResult(It.IsAny<ExperimentResult>()),
                Times.Exactly(1));
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(Colors, Does.Contain(result));
            });
        }

        [Test]
        public async Task TestExistingDeviceButtonColor()
        {
            var deviceToken = GetNewRandomDeviceToken();
            var color = GetRandomColor();
            var dbResult = _fixture.Build<ExperimentResult>()
                .With(x => x.DeviceToken, deviceToken)
                .With(x => x.ExperimentType, ExperimentType.ButtonColor)
                .With(x => x.Value, color)
                .WithAutoProperties()
                .Create();
            _experimentRepositoryMock.Setup(x => x.GetExperimentResult(deviceToken, ExperimentType.ButtonColor))
                .ReturnsAsync(dbResult);

            var result = await _exepimentService.GetButtonColor(deviceToken);

            _experimentRepositoryMock.Verify(x => x.GetExperimentResult(deviceToken, ExperimentType.ButtonColor),
                Times.Exactly(1));
            _experimentRepositoryMock.Verify(x => x.GetExperimentResult(deviceToken, ExperimentType.Price),
                Times.Never);
            _experimentRepositoryMock.Verify(x => x.TryInsertExperimentResult(It.IsAny<ExperimentResult>()),
                Times.Never);
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(color));
                Assert.That(Colors, Does.Contain(result));
            });
        }

        #endregion

        #region Price Tests

        [Test]
        public async Task TestNewDevicePrice()
        {
            var deviceToken = GetNewRandomDeviceToken();
            _experimentRepositoryMock.Setup(x => x.GetExperimentResult(deviceToken, ExperimentType.Price))
                .ReturnsAsync((ExperimentResult?)null);
            _experimentRepositoryMock.Setup(x => x.TryInsertExperimentResult(It.IsAny<ExperimentResult>()))
                .ReturnsAsync(true);

            var result = await _exepimentService.GetPrice(deviceToken);

            _experimentRepositoryMock.Verify(x => x.GetExperimentResult(deviceToken, ExperimentType.Price),
                Times.Exactly(1));
            _experimentRepositoryMock.Verify(x => x.GetExperimentResult(deviceToken, ExperimentType.ButtonColor),
                Times.Never);
            _experimentRepositoryMock.Verify(x => x.TryInsertExperimentResult(It.IsAny<ExperimentResult>()),
                Times.Exactly(1));
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(Prices, Does.Contain(result));
            });
        }

        [Test]
        public async Task TestExistingDevicePrice()
        {
            var deviceToken = GetNewRandomDeviceToken();
            var price = GetRandomPrice();
            var dbResult = _fixture.Build<ExperimentResult>()
                .With(x => x.DeviceToken, deviceToken)
                .With(x => x.ExperimentType, ExperimentType.Price)
                .With(x => x.Value, price)
                .WithAutoProperties()
                .Create();
            _experimentRepositoryMock.Setup(x => x.GetExperimentResult(deviceToken, ExperimentType.Price))
                .ReturnsAsync(dbResult);

            var result = await _exepimentService.GetPrice(deviceToken);

            _experimentRepositoryMock.Verify(x => x.GetExperimentResult(deviceToken, ExperimentType.Price),
                Times.Exactly(1));
            _experimentRepositoryMock.Verify(x => x.GetExperimentResult(deviceToken, ExperimentType.ButtonColor),
                Times.Never);
            _experimentRepositoryMock.Verify(x => x.TryInsertExperimentResult(It.IsAny<ExperimentResult>()),
                Times.Never);
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(price));
                Assert.That(Prices, Does.Contain(result));
            });
        }

        #endregion

        #region Repository throws exception

        [Test]
        public async Task TestRepositoryThrowsExceptionOnGetExperimentResult()
        {
            var deviceToken = GetNewRandomDeviceToken();
            _experimentRepositoryMock.Setup(x => x.GetExperimentResult(deviceToken, It.IsAny<ExperimentType>()))
                .ThrowsAsync(new Exception());

            string? result = null;
            try
            {
                result = await _exepimentService.GetButtonColor(deviceToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            _experimentRepositoryMock.Verify(x => x.GetExperimentResult(deviceToken, It.IsAny<ExperimentType>()),
                Times.Exactly(1));
            _experimentRepositoryMock.Verify(x => x.TryInsertExperimentResult(It.IsAny<ExperimentResult>()),
                Times.Never);
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(null));
                Assert.That(Colors, Does.Not.Contain(result));
            });

            result = null;
            try
            {
                result = await _exepimentService.GetPrice(deviceToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            _experimentRepositoryMock.Verify(x => x.GetExperimentResult(deviceToken, It.IsAny<ExperimentType>()),
                Times.Exactly(2));
            _experimentRepositoryMock.Verify(x => x.TryInsertExperimentResult(It.IsAny<ExperimentResult>()),
                Times.Never);
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(null));
                Assert.That(Colors, Does.Not.Contain(result));
            });
        }

        [Test]
        public async Task TestRepositoryThrowsExceptionOnTryInsertExperimentResult()
        {
            var deviceToken = GetNewRandomDeviceToken();
            _experimentRepositoryMock.Setup(x => x.GetExperimentResult(deviceToken, It.IsAny<ExperimentType>()))
                .ReturnsAsync((ExperimentResult?)null);
            _experimentRepositoryMock.Setup(x => x.TryInsertExperimentResult(It.IsAny<ExperimentResult>()))
                .ThrowsAsync(new Exception());

            string? result = null;
            try
            {
                result = await _exepimentService.GetButtonColor(deviceToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            _experimentRepositoryMock.Verify(x => x.GetExperimentResult(deviceToken, ExperimentType.ButtonColor),
                Times.Exactly(1));
            _experimentRepositoryMock.Verify(x => x.TryInsertExperimentResult(It.IsAny<ExperimentResult>()),
                Times.Exactly(1));
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(null));
                Assert.That(Colors, Does.Not.Contain(result));
            });

            result = null;
            try
            {
                result = await _exepimentService.GetPrice(deviceToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            _experimentRepositoryMock.Verify(x => x.GetExperimentResult(deviceToken, It.IsAny<ExperimentType>()),
                Times.Exactly(2));
            _experimentRepositoryMock.Verify(x => x.TryInsertExperimentResult(It.IsAny<ExperimentResult>()),
                Times.Exactly(2));
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(null));
                Assert.That(Colors, Does.Not.Contain(result));
            });
        }

        #endregion

        private static string GetNewRandomDeviceToken() => Guid.NewGuid().ToString();

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