using AutoMapper;
using List_Dal.Interfaces;
using List_Domain.CreateModel;
using List_Domain.Models;
using List_Domain.ViewModel;
using List_Service.Interfaces;
using List_Service.Mapper;
using List_Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace List_Service_Test.ServiceTest
{
    [TestFixture]
    public class CustomListServiceTests
    {
        private Mock<ICustomListRepository> _mockRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<IAutorizationService<CustomList>> _mockAuthService;
        private CustomListService _service;
        private DefaultHttpContext _context;
        private int _userId;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<ICustomListRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockAuthService = new Mock<IAutorizationService<CustomList>>();
            _service = new CustomListService(_mockRepository.Object, _mockMapper.Object, _mockAuthService.Object);

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
            var createModel = new CreateCustomList { Name = "Test List" };

            _mockAuthService.Setup(x => x.GetUserId()).Returns(_userId);
            _mockRepository.Setup(x => x.CheckIfNameExist(createModel.Name, _userId)).ReturnsAsync(false);

            var itemToDb = new CustomList
            {
                Id = 1,
                Name = createModel.Name, 
                UserId = _userId 
            };

            _mockMapper.Setup(x => x.Map<CustomList>(createModel)).Returns(itemToDb);
            _mockRepository.Setup(x => x.Add(itemToDb)).ReturnsAsync(itemToDb.Id);

            // Act
            var result = await _service.Add(createModel);

            // Assert
            Assert.AreEqual(itemToDb.Id, result);
        }

        [Test]
        public async Task GetByUserId_WhenValidUserId_ReturnsListOfViewCustomList()
        {
            // Arrange
            var dbList = new List<CustomList> { new CustomList 
            {
                Id = 1,
                Name = "Test List",
                UserId = _userId }
            };
            var expectedList = new List<ViewCustomList> { new ViewCustomList { Id = 1, Name = "Test List" } };

            _mockAuthService.Setup(x => x.GetUserId()).Returns(_userId);
            _mockRepository.Setup(x => x.GetByUser(_userId)).ReturnsAsync(dbList.AsQueryable());
            _mockMapper.Setup(x => x.Map<ViewCustomList>(It.IsAny<CustomList>())).Returns(expectedList.First());
            _service.HttpContext = _context;

            // Act
            var result = await _service.GetByUserId();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(expectedList.Count));

            for (int i = 0; i < expectedList.Count; i++)
            {
                Assert.That(result.ElementAt(i).Id, Is.EqualTo(expectedList[i].Id));
                Assert.That(result.ElementAt(i).Name, Is.EqualTo(expectedList[i].Name));
            }
        }

        [Test]
        public async Task RemoveCustomLists_WhenValidUserId()
        {
            // Arrange
            var listIds = new List<int> { 1, 2, 3 };

            _mockAuthService.Setup(x => x.GetUserId()).Returns(_userId);
            _mockRepository.Setup(x => x.Remove(listIds)).ReturnsAsync(listIds);

            // Act
            var result = await _service.Remove(listIds);

            // Assert
            _mockAuthService.Verify(x => x.AuthorizeUser(It.IsAny<int>()), Times.Exactly(listIds.Count));
            _mockRepository.Verify(x => x.Remove(listIds), Times.Once);

            CollectionAssert.AreEqual(listIds, result);
        }

        [Test]
        public async Task Update_Should_Return_Correct_Id()
        {
            // Arrange
            var listId = 1;
            var createModel = new CreateCustomList { Name = "Test List" };
            var customList = new CustomList
            {
                Id = listId,
                Name = "Old List Name",
                UserId = _userId
            };
            var expectedId = listId;

            _mockAuthService.Setup(x => x.GetUserId()).Returns(_userId);
            _mockRepository.Setup(x => x.CheckIfNameExist(createModel.Name, _userId)).ReturnsAsync(false);
            _mockRepository.Setup(x => x.Update(It.IsAny<CustomList>())).ReturnsAsync(true);
            _mockRepository.Setup(x => x.GetById(listId)).ReturnsAsync(customList);
            _mockMapper.Setup(x => x.Map<CustomList>(createModel)).Returns(new CustomList { Name = createModel.Name });

            _service.HttpContext = _context;

            // Act
            var result = await _service.Update(createModel, listId);

            // Assert
            Assert.AreEqual(expectedId, result);

            _mockAuthService.Verify(x => x.AuthorizeUser(listId), Times.Once);
            _mockAuthService.Verify(x => x.GetUserId(), Times.Once);
            _mockRepository.Verify(x => x.CheckIfNameExist(createModel.Name, _userId), Times.Once);
            _mockRepository.Verify(x => x.Update(It.Is<CustomList>(y => y.Name == createModel.Name && y.Id == listId && y.UserId == _userId)), Times.Once);
            _mockMapper.Verify(x => x.Map<CustomList>(createModel), Times.Once);
        }

        [Test]
        public async Task Update_ShouldUpdateCustomList()
        {
            // Arrange
            var customList = new CustomList
            {
                Id = 1,
                Name = "Test List",
                UserId = _userId,
            };
            var updateModel = new CreateCustomList
            {
                Name = "Updated List"
            };
            var expectedId = 1;
            var expectedName = "Updated List";

            _mockAuthService.Setup(m => m.GetUserId()).Returns(customList.UserId);
            _mockRepository.Setup(m => m.GetById(customList.Id)).ReturnsAsync(customList);
            _mockRepository.Setup(m => m.CheckIfNameExist(updateModel.Name, customList.UserId)).ReturnsAsync(false);
            _mockRepository.Setup(m => m.Update(It.IsAny<CustomList>())).ReturnsAsync(true);

            // Act     
            var result = await _service.Update(updateModel, customList.Id);

            // Assert
            Assert.AreEqual(expectedId, result);
            Assert.AreEqual(expectedName, customList.Name);

            _mockAuthService.Verify(m => m.AuthorizeUser(customList.Id), Times.Once);
            _mockRepository.Verify(m => m.Update(It.IsAny<CustomList>()), Times.Once);
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