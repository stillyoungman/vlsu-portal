using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.Models
{
    public interface IExamEntry
    {
        long Id { get; }
        string Name { get; }
        string ShortName { get; }
        bool IsEge { get; }
    }
}
