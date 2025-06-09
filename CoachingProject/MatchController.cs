using Microsoft.AspNetCore.Mvc;

namespace CoachingProject
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchController(IMatchRepo matchRepo) : ControllerBase
    {
        /// <summary>
        /// Updates a match event by match ID.
        /// </summary>
        [HttpPost]
        public string UpdateMatch(int matchId, MatchEvent matchEvent)
        {
            var 
            return 
        }
    }
} 