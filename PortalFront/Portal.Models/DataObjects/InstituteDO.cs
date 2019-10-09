using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.Models.DataObjects
{
    public class InstituteDO
    {
        public InstituteDO() { }
        public InstituteDO(long id, string name, string abbreviation)
        {
            Id = id;
            Name = name;
            NameAbbreviation = abbreviation;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string NameAbbreviation { get; set; }

        public Institute ToModel() => new Institute(Id, Name, NameAbbreviation);
    }
}
