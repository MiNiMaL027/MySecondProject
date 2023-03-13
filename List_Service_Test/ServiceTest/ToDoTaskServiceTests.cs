using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class ToDoTaskServiceTests
    {
        private Mock<IToDoTaskRepository> _mockRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<IAutorizationService<ToDoTask>> _mockAuthService;
        private ToDoTaskService _service;
        private DefaultHttpContext _context;
        private int _userId;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IToDoTaskRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockAuthService = new Mock<IAutorizationService<ToDoTask>>();
            _service = new ToDoTaskService(_mockRepository.Object, _mockMapper.Object, _mockAuthService.Object);

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
            var createModel = new CreateToDoTask
            {
                Title = "Test",
                Description = "Test",
                Importance = Importance.Normal,
                CustomListId = 1,
                IsFavorite = true,
            };

            _mockAuthService.Setup(x => x.GetUserId()).Returns(_userId);
            _mockRepository.Setup(x => x.CheckIfNameExist(createModel.Title, _userId)).ReturnsAsync(false);

            var itemToDb = new ToDoTask()
            {
                Id = 1,
                Title = createModel.Title,
                Description = createModel.Description,
                Importance = createModel.Importance,
                CustomListId = createModel.CustomListId,
                IsFavorite = createModel.IsFavorite,
                UserId = _userId,
            };

            _mockMapper.Setup(x => x.Map<ToDoTask>(createModel)).Returns(itemToDb);
            _mockRepository.Setup(x => x.Add(itemToDb)).ReturnsAsync(itemToDb.Id);

            // Act
            var result = await _service.Add(createModel);

            // Assert
            Assert.AreEqual(itemToDb.Id, result);

        }

        [Test]
        public async Task GetByUserId_WhenValidUserId_ReturnsTaskOfViewToDoTask()
        {
            // Arrange
            var dbTask = new List<ToDoTask> { new ToDoTask
            {
                Id = 1,
                Title = "Test",
                Description = "Test",
                Importance = Importance.Normal,
                CustomListId = 1,
                IsFavorite = true,
                UserId = _userId,
                CreationDate = DateTime.Now,
            }};
            var expectedTask = new List<ViewToDoTask> { new ViewToDoTask
            {
                Id = 1,
                Title = "Test",
                Importance = Importance.Normal,
                CustomListId = 1,
                IsFavorite = true,
                UserId = _userId,
                CreationDate = dbTask[0].CreationDate,
                IsCompleted = false,
                IsDeleted = false,
                Description = "Test",
            }};

            _mockAuthService.Setup(x => x.GetUserId()).Returns(_userId);
            _mockRepository.Setup(x => x.GetByUser(_userId)).ReturnsAsync(dbTask.AsQueryable());
            _mockMapper.Setup(x => x.Map<ViewToDoTask>(It.IsAny<ToDoTask>())).Returns(expectedTask.First());
            _service.HttpContext = _context;

            // Act
            var result = await _service.GetByUserId();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(expectedTask.Count));

            for (int i = 0; i < expectedTask.Count; i++)
            {
                Assert.That(result.ElementAt(i).Id, Is.EqualTo(expectedTask[i].Id));
                Assert.That(result.ElementAt(i).Title, Is.EqualTo(expectedTask[i].Title));
                Assert.That(result.ElementAt(i).Description, Is.EqualTo(expectedTask[i].Description));
                Assert.That(result.ElementAt(i).Importance, Is.EqualTo(expectedTask[i].Importance));
                Assert.That(result.ElementAt(i).CreationDate, Is.EqualTo(expectedTask[i].CreationDate));
                Assert.That(result.ElementAt(i).DueToDate, Is.EqualTo(expectedTask[i].DueToDate));
                Assert.That(result.ElementAt(i).IsFavorite, Is.EqualTo(expectedTask[i].IsFavorite));
                Assert.That(result.ElementAt(i).CustomListId, Is.EqualTo(expectedTask[i].CustomListId));
            }
        }

        [Test]
        public async Task GetByBaseList_Should_Return_ViewToDoTask_List()
        {
            // Arrange
            var baseListId = 1;
            var todoTasks = new List<ToDoTask>
            {
                new ToDoTask
                {
                    Id = 1,
                    Title = "Test Task 1",
                    Importance = Importance.High,
                    DueToDate = DateTime.Now.Date,
                    CreationDate = DateTime.Now.AddDays(-2),
                    IsCompleted = false,
                    IsFavorite = true,
                    IsDeleted = false,
                    UserId = _userId
                },
                new ToDoTask
                {
                    Id = 2,
                    Title = "Test Task 2",
                    Importance = Importance.Normal,
                    DueToDate = DateTime.Now.AddDays(2).Date,
                    CreationDate = DateTime.Now.AddDays(-3),
                    IsCompleted = false,
                    IsFavorite = false,
                    IsDeleted = false,
                    UserId = _userId
                }
            }.AsQueryable();

            var expectedList = todoTasks.Where(x => x.DueToDate != null && x.DueToDate.Value.Date == DateTime.Now.Date)
                                        .ProjectTo<ViewToDoTask>(_mockMapper.Object.ConfigurationProvider);

            _mockAuthService.Setup(x => x.GetUserId()).Returns(_userId);
            _mockRepository.Setup(x => x.GetByUser(_userId)).ReturnsAsync(todoTasks);

            // Act
            var result = await _service.GetByBaseList(baseListId);

            // Assert
            Assert.AreEqual(expectedList.Count(), result.Count());
            Assert.AreEqual(expectedList.First().Title, result.First().Title);
            Assert.AreEqual(expectedList.First().Importance, result.First().Importance);
            Assert.AreEqual(expectedList.First().DueToDate, result.First().DueToDate);

            _mockAuthService.Verify(x => x.GetUserId(), Times.Once);
            _mockRepository.Verify(x => x.GetByUser(_userId), Times.Once);
        }

        [Test]
        public async Task RemoveToDoTask_WhenValidUserId()
        {
            // Arrange
            var taskIds = new List<int> { 1, 2, 3 };

            _mockAuthService.Setup(x => x.GetUserId()).Returns(_userId);
            _mockRepository.Setup(x => x.Remove(taskIds)).ReturnsAsync(taskIds);

            // Act
            var result = await _service.Remove(taskIds);

            // Assert
            _mockAuthService.Verify(x => x.AuthorizeUser(It.IsAny<int>()), Times.Exactly(taskIds.Count));
            _mockRepository.Verify(x => x.Remove(taskIds), Times.Once);

            CollectionAssert.AreEqual(taskIds, result);
        }

        [Test]
        public async Task Update_Should_Return_Correct_Id()
        {
            // Arrange
            var taskId = 1;
            var createModel = new CreateToDoTask
            {
                Title = "Title",
                Description = "Description",
                Importance = Importance.Normal,
                CustomListId = 1
            };
            var task = new ToDoTask
            {
                Id = taskId,
                Title = "Test Task 1",
                Importance = Importance.High,
                DueToDate = DateTime.Now.Date,
                CreationDate = DateTime.Now.AddDays(-2),
                IsCompleted = false,
                IsFavorite = true,
                IsDeleted = false,
                UserId = _userId
            };

            var expectedId = taskId;

            _mockAuthService.Setup(x => x.GetUserId()).Returns(_userId);
            _mockRepository.Setup(x => x.CheckIfNameExist(createModel.Title, _userId)).ReturnsAsync(false);
            _mockRepository.Setup(x => x.Update(It.IsAny<ToDoTask>())).ReturnsAsync(true);
            _mockRepository.Setup(x => x.GetById(taskId)).ReturnsAsync(task);
            _mockMapper.Setup(x => x.Map<ToDoTask>(createModel)).Returns(new ToDoTask
            {
                Title = createModel.Title,
                Importance = createModel.Importance,
                Description = createModel.Description,
                DueToDate = createModel.DueToDate,
                CustomListId = createModel.CustomListId,          
            });

            _service.HttpContext = _context;

            // Act
            var result = await _service.Update(createModel, taskId);

            // Assert
            Assert.AreEqual(expectedId, result);

            _mockAuthService.Verify(x => x.AuthorizeUser(taskId), Times.Once);
            _mockAuthService.Verify(x => x.GetUserId(), Times.Once);
            _mockRepository.Verify(x => x.CheckIfNameExist(createModel.Title, _userId), Times.Once);
            _mockRepository.Verify(x => x.Update(It.Is<ToDoTask>
            (
                y => y.Title == createModel.Title &&
                y.Id == taskId && 
                y.Importance == createModel.Importance &&
                y.Description == createModel.Description &&
                y.DueToDate == createModel.DueToDate &&
                y.CustomListId == createModel.CustomListId &&
                y.UserId == _userId
            )),
            Times.Once);
            _mockMapper.Verify(x => x.Map<ToDoTask>(createModel), Times.Once);
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
