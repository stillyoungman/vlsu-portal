using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Portal.Models
{
    public enum EducationDegree
    {
        College = 2,

        [Description("Бакалавриат")]
        Bachelor = 3,
        [Description("Магистратура")]
        Master = 4,
        [Description("Специалитет")]
        Spec = 5,
        [Description("Аспирантура")]
        GraduateStudent = 6,
        [Description("Прикладной бакалавриат")]
        PractialBachelor = 7
    }

    public static class EducationDegreeExt
    {
        public static string GetStringValue(this EducationDegree degree)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])degree.GetType().GetField(degree.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}
