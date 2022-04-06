using FluentAssertions;
using MainApp.BLL.Context;
using MainApp.BLL.Entities;
using MainApp.BLL.Repositories;
using MainApp.BLL.Services;
using MainApp.Web.Controllers;
using MainApp.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MainAppUnitTests.TrainerTests
{
    public class TrainerServiceTests
    {
        private readonly UserService _sut; 
        private readonly Mock<IRepository<ApplicationUser>> _userMockRepo = new Mock<IRepository<ApplicationUser>>();
        private readonly IRepository<ApplicationUser> Users;
        private readonly ApplicationDbContext _context;

        public TrainerServiceTests()
        {
            _sut = new UserService(_userMockRepo.Object);
        }


        [Fact]
        public async Task GetUsers_ShoudReturnUsers_WhenUsersExist()
        {
            // Arrange
            var userFromDataBase = _context.Users.ToList();

            _userMockRepo.Setup(x => x.GetAll()).ReturnsAsync(userFromDataBase);

            // Act
            var users = await _sut.GetAll();

            // Assert
            users.Should().NotBeEmpty();
        }


        [Fact]
        public async Task GetById_ShoudReturnUser_WhenUserExist()
        {
            // Arrange
            var userId = 1;
            var userName = "";
            var userDto = new ApplicationUser
            {
                Id = userId,
                FirstName = userName
            };
            _userMockRepo.Setup(x => x.GetById(userId)).ReturnsAsync(userDto);

            // Act
            var user = await _sut.GetById(userId);

            // Assert
            Assert.Equal(userId, user.Id);
        }

        [Fact]
        public async Task GetById_ShoudReturnNothing_WhenUserDoesnNotExist()
        {
            // Arrange
            _userMockRepo.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(()=>null);

            // Act
            Random rnd = new Random();
            var user = await _sut.GetById(rnd.Next(10, 200));

            // Assert
            Assert.Null(user);
        }

        [Fact]
        public async Task Id_Set_DuplicateId_ThenReturnWrong()
        {
            // Arrange
            _userMockRepo.Setup(repo => repo.GetAll())
                .ReturnsAsync(GetUsersDto());
            var service = new UserService(_userMockRepo.Object);

            // Act
            var userDtos = await service.GetAll();
            var userD = userDtos.Select(x => x.Id).ToList().Count();    

            var result = userDtos.Select(x => x.Id)
                .DistinctBy(x => x).Count();
            
            // Assert
            Assert.Equal(userD, result);   
        }

        private List<ApplicationUser> GetUsersDto()
        {
            var sessions = new List<ApplicationUser>();
            sessions.Add(new ApplicationUser()
            {
                Created = new DateTime(2016, 7, 2),
                Id = 1,
                FirstName = "dsdsd"
            });
            sessions.Add(new ApplicationUser()
            {
                Created = new DateTime(2018, 7, 2),
                Id = 2,
                FirstName = "dsdsd"
            });
            sessions.Add(new ApplicationUser()
            {
                Created = new DateTime(2018, 7, 2),
                Id = 3,
                FirstName = "dsdsd"
            });
            return sessions;
        }
    }
}
