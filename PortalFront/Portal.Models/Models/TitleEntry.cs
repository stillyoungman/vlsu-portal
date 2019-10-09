using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Portal.Models
{
    public class TitleEntry : IPlanTitle
    {
        public EducationDegree _degree { get; set; }
        public long Id { get; set; }
        public TitleEntry() { }
        public TitleEntry(string code, string name, string ext, string term, EducationDegree degree)
        {
            Code = code;
            Name = name;
            Extension = ext;
            Term = term;
            _degree = degree;
        }
        [DisplayName("Шифр")]
        public string Code { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Term { get; set; }
        public string Degree { get => _degree.GetStringValue(); }

        [DisplayName("Наименование направления (специальности)")]
        public string Full
        {
            get
            {
                var ext = string.IsNullOrEmpty(Extension) ? $"{Degree}. {Term}." : $"{Extension}. {Degree}. {Term}.";
                return $"{Name}. {ext}";
            }
        }

        public string Short { get => Name + "."; }
    }
}
