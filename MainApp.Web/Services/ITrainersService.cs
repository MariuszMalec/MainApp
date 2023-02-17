using MainApp.BLL.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MainApp.Web.Services
{
    public interface ITrainersService
    {
        Task<bool> CheckIfEmailExis(string email, HttpContext httpContext);
        Task<bool> CreateTrainer(TrainerView model, HttpContext httpContext);
        Task<bool> DeleteTrainer(int id, TrainerView model, HttpContext httpContext);
        Task<bool> EditTrainer(int id, TrainerView model, HttpContext httpContext);
        Task<List<TrainerView>> GetAll(string userEmail, HttpContext httpContext);
        Task<TrainerView> GetTrainerById(int id, string userEmail, HttpContext httpContext);
    }
}