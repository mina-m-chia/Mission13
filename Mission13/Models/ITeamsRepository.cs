using System;
using System.Linq;

namespace Mission13.Models
{
    public interface ITeamsRepository
    {
        IQueryable<Team> Teams { get; }
    }
}
