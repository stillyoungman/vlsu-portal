using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Portal.Models
{
    public enum DocumentType
    {
        [Description("Нет")]
        None = -1,
        [Description("Копия")]
        Copy = 0, //? check in DB
        [Description("Оригинал")]
        Original = 1
    }

    public static class DocTypeExt
    {
        public static string StringValue(this DocumentType doc)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])doc
                .GetType()
                .GetField(doc.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}
