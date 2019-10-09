using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.Models
{
    public interface IInstitute
    {
        long Id { get; }
        string Name { get; }
        string NameAbbreviation { get; }
        IEnumerable<IPlanEntry> Bachelor { get; }
        IEnumerable<IPlanEntry> Master { get; }
        bool HasBachelor { get; }
        bool HasMaster { get; }
    }
}
