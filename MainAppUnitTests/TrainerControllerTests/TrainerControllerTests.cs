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
        public async Task Index_ReturnsNoTrainers_WhenStatusIsNotOkObjectResult()
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
        public async Task Insert_ReturnsNoTrainer_WhenSatusIsNotOkCreatedAtActionResult()
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
            //return new List<Trainer>();
        }

    }


}
