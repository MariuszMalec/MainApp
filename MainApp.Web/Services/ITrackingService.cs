using MainApp.BLL.Entities;
using MainApp.BLL.Enums;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MainApp.Web.Services
{
    public interface ITrackingService
    {
        Task<bool> DeleteAllEvents();
        Task<bool> DeleteEvent(int id, Event model, HttpContext httpContext);
        Task<List<Event>> GetAll(string sortOrder, string searchString);
        Task<Event> GetEventById(int id, string userEmail, HttpContext httpContext);
        Task<bool> Insert(Event myEvent);
        Task<Event> InsertEvent(ActivityActions activityActions, HttpContext httpContext, string email);
        Task<List<Event>> SelectedEvents(string sortOrder, string searchString, List<Event> events);
    }
}