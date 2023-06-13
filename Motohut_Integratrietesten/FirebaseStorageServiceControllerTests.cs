using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Motohut_API.Controllers;
using Motohut_API;
using System.Threading.Tasks;
using Assert = NUnit.Framework.Assert;

namespace Motohut.Integratietesten
{
    [TestFixture]
    public class FirebaseStorageServiceControllerTests
    {
        private FirebaseStorageServiceController _controller;
        private IFirebaseStorageService _mockFirebaseStorageService;

        [SetUp]
        public void Setup()
        {
            _mockFirebaseStorageService = new MockFirebaseStorageService();
            _controller = new FirebaseStorageServiceController(_mockFirebaseStorageService);
        }

        [Test]
        public async Task AddUserAddress_ValidInput_ReturnsOkResultWithId()
        {
            // Arrange
            string email = "test@example.com";
            int huisnummer = 123;
            string postcode = "12345";
            string stad = "Amsterdam";
            string straat = "Main Street";

            // Act
            IActionResult result = await _controller.AddUserAddress(email, huisnummer, postcode, stad, straat);

            // Assert;
            Assert.IsInstanceOf<OkObjectResult>(result);
            OkObjectResult okResult = (OkObjectResult)result;
            string response = okResult.Value.ToString();
            Assert.AreEqual("{ Id = 101 }", response);
        }

        [Test]
        public async Task GetVideoFilesAsync_ValidInput_ReturnsListOfVideoFileInfo()
        {
            // Arrange
            string email = "test@example.com";

            // Act
            List<VideoFileInfo> result = await _mockFirebaseStorageService.GetVideoFilesAsync(email);

            // Assert
            Assert.IsInstanceOf<List<VideoFileInfo>>(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("video1.mp4", result[0].Name);
            Assert.AreEqual(12345, result[0].Size);
            Assert.AreEqual("https://downloadlink.com/video1.mp4", result[0].DownloadUrl);
            Assert.AreEqual("video2.mp4", result[1].Name);
            Assert.AreEqual(67890, result[1].Size);
            Assert.AreEqual("https://downloadlink.com/video2.mp4", result[1].DownloadUrl);
        }

        [Test]
        public async Task GetVideoFilesAsync_ValidInput_ReturnsEmptyList()
        {
            // Arrange
            string email = "test123@example.com";

            // Act
            List<VideoFileInfo> result = await _mockFirebaseStorageService.GetVideoFilesAsync(email);

            // Assert
            Assert.IsInstanceOf<List<VideoFileInfo>>(result);
            Assert.AreEqual(0, result.Count);
        }
    }

    // Dummy-implementatie van de FirebaseStorageService voor testdoeleinden
    public class MockFirebaseStorageService : IFirebaseStorageService
    {
        public async Task<string> AddUserAsync(string email, int huisnummer, string postcode, string stad, string straat)
        {
            return "101";
        }

        public async Task<long> CheckEmailAsync(string email)
        {
            return 0;
        }

        public async Task<string> GetDownloadUrlAsync(string folderName, string objectName)
        {
            return "https://downloadlink.com";
        }

        public async Task<byte[]> GetVideoDataAsync(string email, string videoName)
        {
            return new byte[0];
        }

        public async Task<List<VideoFileInfo>> GetVideoFilesAsync(string email)
        {
            if (email == "test@example.com")
            {
                return new List<VideoFileInfo>
            {
                new VideoFileInfo { Name = "video1.mp4", Size = 12345, DownloadUrl = "https://downloadlink.com/video1.mp4" },
                new VideoFileInfo { Name = "video2.mp4", Size = 67890, DownloadUrl = "https://downloadlink.com/video2.mp4" }
            };
            }
            else
            {
                return new List<VideoFileInfo>();
            }
        }
    }

}
