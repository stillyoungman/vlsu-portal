using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Serialization;
using System.IO;

namespace Portal.Models
{
    public class NewAbitModel
    {
        private Tuple<int, FinancingType>[] finTypes;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        [BindProperty]
        public long[] PickedPlans { get; set; }
        /// <summary>
        /// выглядят как c-0;c-1;b-1 (в конце НЕТ точки с запятой)
        /// что значит, на первое направление контракт(с);
        /// на второе контракт(с) и бюджет(b)
        /// </summary>
        public string RawFinTypes { get; set; }

        public IEnumerable<AbitPlan> ToDataObject => PickedPlans.Select((p, i) => new AbitPlan
        {
            Id = p,//where typle index == index
            FinSources = FinTypes.Where(f => f.Item1 == i).Select(f => (long)f.Item2).ToArray()
        }).ToArray();

        

        public string SerializationString { get
            {
                var aSerializer = new XmlSerializer(typeof(AbitPlan[]));
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                aSerializer.Serialize(sw, ToDataObject.ToArray()); 
                return sw.GetStringBuilder().ToString();
            } }

        public Tuple<int,FinancingType>[] FinTypes {
            get
            {
                if(finTypes == null)
                {
                    var rawItems = RawFinTypes.Split(';');
                    var result = new Tuple<int, FinancingType>[rawItems.Length];

                    try
                    {
                        for (var i = 0; i < rawItems.Length; i++)
                        {
                            var finIndex = int.Parse(rawItems[i].Split('-')[1]);
                            result[i] = rawItems[i][0] == 'c' ? new Tuple<int, FinancingType>(finIndex, FinancingType.Contract)
                                : new Tuple<int, FinancingType>(finIndex, FinancingType.Government);
                        }
                        finTypes = result;
                    }
                    catch { }
                }

                return finTypes;
            }
        }
    }

    public class AbitPlan
    {
        public long Id { get; set; }
        public long[] FinSources { get; set; }
    }
}
