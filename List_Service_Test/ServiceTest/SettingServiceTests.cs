using AutoMapper;
using List_Dal.Interfaces;
using List_Domain.CreateModel;
using List_Domain.Enums;
using List_Domain.Models;
using List_Domain.ViewModel;
using List_Service.Interfaces;
using List_Service.Mapper;
using List_Service.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace List_Service_Test.ServiceTest
{
    [TestFixture]
    public class SettingServiceTests
    {
        private Mock<ISettingsRepository> _mockRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<IAutorizationService<Settings>> _mockAuthService;
        private SettingsService _service;
        private DefaultHttpContext _context;
        private int _userId;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<ISettingsRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockAuthService = new Mock<IAutorizationService<Settings>>();
            _service = new SettingsService(_mockRepository.Object, _mockAuthService.Object, _mockMapper.Object);

            var userId = 1111;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };
            var context = new DefaultHttpContext();
            context.User = new ClaimsPrincipal(new ClaimsIdentity(claims));

            _context = context;
            _userId = userId;

            ConfigureMapper();
        }

        [Test]
        public async Task Add_WhenValidOptions_ReturnsAddedItemId()
        {
            // Arrange
            var createModel = new ViewSettings
            {
                AllowNotification = true,
                DefaultListId = 1,
                Id = 1,
            };

            _mockAuthService.Setup(x => x.GetUserId()).Returns(_userId);

            var itemToDb = new Settings()
            {
                Id = 1,
                AllowNotification = true,
                DefaultListId = 1,
            };

            _mockMapper.Setup(x => x.Map<Settings>(createModel)).Returns(itemToDb);
            _mockRepository.Setup(x => x.CreateSettings(itemToDb)).ReturnsAsync(itemToDb.Id);

            // Act
            var result = await _service.CreateSettings(createModel);

            // Assert
            Assert.AreEqual(itemToDb.Id, result);

        }

        [Test]
        public async Task Update_Should_Return_Correct_Id()
        {
            // Arrange
            var expectedId = 1;
            var createModel = new ViewSettings { DefaultListId = expectedId, AllowNotification = true};
            var settings = new Settings
            {
                Id = 1,
                AllowNotification = true,
                DefaultListId = expectedId,
            };    
            
            _mockAuthService.Setup(x => x.GetUserId()).Returns(_userId);
            _mockRepository.Setup(x => x.UpdateSetings(It.IsAny<Settings>())).ReturnsAsync(true);
            _mockRepository.Setup(x => x.GetSettingsByUser(_userId)).ReturnsAsync(settings);
            _mockMapper.Setup(x => x.Map<Settings>(createModel)).Returns(new Settings
            {
                AllowNotification = createModel.AllowNotification,
                DefaultListId = createModel.DefaultListId
            });

            _service.HttpContext = _context;

            // Act
            var result = await _service.UpdateSetings(createModel);

            // Assert
            Assert.AreEqual(expectedId, result);

            _mockAuthService.Verify(x => x.AuthorizeUser(expectedId), Times.Once);
            _mockAuthService.Verify(x => x.GetUserId(), Times.Once);
            _mockRepository.Verify(x => x.UpdateSetings(It.Is<Settings>(y =>
                y.AllowNotification == createModel.AllowNotification &&
                y.DefaultListId == createModel.DefaultListId &&
                y.UserId == _userId)), Times.Once);
            _mockMapper.Verify(x => x.Map<Settings>(createModel), Times.Once);
        }

        private void ConfigureMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AppMappingProfile());
            });

            _mockMapper.Setup(x => x.ConfigurationProvider).Returns(config);
        }
    }
}
