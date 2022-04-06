using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Tracking;
using Tracking.Controllers;
using Tracking.Models;
using Tracking.Services;
using Xunit;

namespace MainAppIntegrationTests
{
    public class TrainerControllerIntegrationsTests
    {

        [Fact]
        public async Task Index_ReturnsTrainersFromController_WhenStatusOK()
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
