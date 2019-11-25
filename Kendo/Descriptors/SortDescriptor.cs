using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artech.AspNetCore.Kendo.Descriptors
{
    public class SortDescriptor : IKendoDescriptor
    {
        public string Dir { get; set; }
        public string Field { get; set; }

        public SortDescriptor()
        {
            this.Dir = "asc";
        }

        public void Deserialize(string source)
        {
            var parts = source.Split(new[] { '-' });
            Field = parts.First();
            if (parts.Length > 1)
            {
                Dir = parts[1];
            }
        }
    }
}
