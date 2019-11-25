using System;

namespace Artech.AspNetCore.Kendo.Descriptors
{
    public interface IKendoDescriptor
    {
        string Field { get; set; }
        void Deserialize(string source);
    }
}
