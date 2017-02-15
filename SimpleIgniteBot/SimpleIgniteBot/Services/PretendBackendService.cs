using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using SimpleIgniteBot.Services.Entities;

namespace SimpleIgniteBot.Services
{
    public class PretendBackendService
    {
        async Task<Session> _getSession()
        {
            return new Session
            {
                Code = "CLD332",
                DateTime_Start = new DateTime(2017, 02, 16, 9, 45, 0),
                Description = @"This session is in the Cloud track and targets the following disciplines: Developer
                    Have you used the Ignite bot this week ? Are you thinking about creating your own bot ? Here is your chance to meet the team who worked on this project to find out more.Anyone thinking about developing a bot for their organisation should join this session. ",
                Name = "Making of the Ignite Bot",
                Room = "Central A",
                Presenters = await GetPresentersForSession("CLD323"),
                Track = "Cloud"
            };
        }
  
        public async Task<List<Session>> GetSessionByTime(string time)
        {
            var results = new List<Session> {await _getSession()};
            if (time.ToLower() == "afternoon")
            {
                results.Add(await _getSession());
            }
            return results;
        }

        public async Task<Session> GetByPresenter(string firstName, string lastName)
        {
            if ((firstName.ToLower() == "jordan" || firstName.ToLower() == "tim")
                && lastName.ToLower() == "knight" || lastName.ToLower() == "hill")
            {
                return await _getSession();
            }

            return null;
        }

        public async Task<List<Presenter>> GetPresentersForSession(string session)
        {
            var l = new List<Presenter>();

            l.Add(new Presenter
            {
                FirstName = "Jordan",
                LastName = "Knight"
            });
            l.Add(new Presenter
            {
                FirstName = "Tim",
                LastName = "Hill"
            });

            return l;
        }
    }
}