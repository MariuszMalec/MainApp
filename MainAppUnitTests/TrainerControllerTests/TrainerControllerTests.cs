using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tracking.Controllers;
using Tracking.Models;
using Tracking.Services;
using Xunit;

namespace MainAppUnitTests.TrainerTests
{
    public class TrainerControllerTests
    {

        [Fact]
        public async Task GetTrainer_ReturnTrainer_WhenExist()
        {
            // Arrange
            var userView = new Trainer () 
            { 
                Id = 1,
                FirstName="Hej",
                LastName = "Ho"
            };
            var mockRepo = new Mock<IRepositoryService<Trainer>>();
            mockRepo.Setup(r => r.Get(1))
                .ReturnsAsync(userView);
            
            var controller = new TrainerController(mockRepo.Object);

            // Act
            var result = await controller.GetTrainer(1) as OkObjectResult;

            // Assert
            Assert.Equal(userView,result.Value);
        }

        [Fact]
        public async Task GetTrainer_ReturnNoTrainer_WhenNotExist()
        {
            // Arrange
            var userView = new Trainer()
            {
                Id = 1,
                FirstName = "Hej",
                LastName = "Ho"
            };
            var mockRepo = new Mock<IRepositoryService<Trainer>>();
            mockRepo.Setup(r => r.Get(1))
                .ReturnsAsync(userView);

            var controller = new TrainerController(mockRepo.Object);

            // Act
            var result = await controller.GetTrainer(2) as NotFoundObjectResult;

            // Assert
            Assert.Equal($"Brak uzytkownika!", result.Value);
        }

        [Fact]
        public async Task Get_ReturnsTrainers_WhenStatusOkObjectResult()
        {
            // Arrange
            var mockRepo = new Mock<IRepositoryService<Trainer>>();
            mockRepo.Setup(repo => repo.GetAll())
                .ReturnsAsync(GetTrainers());
                ;              
            var controller = new TrainerController(mockRepo.Object);

            // Act
            var result = await controller.Get();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Get_ReturnsNoTrainers_WhenStatusNotFoundObjectResult()
        {
            // Arrange
            var mockRepo = new Mock<IRepositoryService<Trainer>>();
            mockRepo.Setup(repo => repo.GetAll())
                .ReturnsAsync(new List<Trainer>());//TODO pusta lista
            ;
            var controller = new TrainerController(mockRepo.Object);

            // Act
            var result = await controller.Get();

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }


        [Fact]
        public async Task Insert_Trainer_WhenSatusIsNotOkCreatedAtActionResult_ReturnFalse()
        {
            // Arrange
            var trainer = new Trainer
            {
                Id = 1,
                FirstName = "Piotr"
            };

            var mockRepo = new Mock<IRepositoryService<Trainer>>();
            mockRepo.Setup(repo => repo.Insert(trainer));        
            var controller = new TrainerController(mockRepo.Object);

            // Act
            var result = await controller.Insert(trainer);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public async Task DeleteTrainer_WhenExist_ReturnOk()
        {
            // Arrange
            var trainer = new Trainer
            {
                Id = 1,
                FirstName = "Piotr"
            };

            var mockRepo = new Mock<IRepositoryService<Trainer>>();
            mockRepo.Setup(r => r.Get(1))
                .ReturnsAsync(trainer);

            var controller = new TrainerController(mockRepo.Object);

            // Act
            var result = await controller.Delete(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteTrainer_WhenNotExist_ReturnNotFound()
        {
            // Arrange
            var trainer = new Trainer
            {
                Id = 1,
                FirstName = "Piotr"
            };

            var mockRepo = new Mock<IRepositoryService<Trainer>>();
            mockRepo.Setup(r => r.Get(1))
                .ReturnsAsync(trainer);

            var controller = new TrainerController(mockRepo.Object);

            // Act
            var result = await controller.Delete(2);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        private List<Trainer> GetTrainers()
        {
            var sessions = new List<Trainer>();
            sessions.Add(new Trainer()
            {
                CreatedDate = new DateTime(2021, 7, 2),
                Id = 1,
                FirstName = "TestOne"
            });
            sessions.Add(new Trainer()
            {
                CreatedDate = new DateTime(2021, 7, 2),
                Id = 2,
                FirstName = "TestTwo"
            });
            return sessions;
        }
    }
}
