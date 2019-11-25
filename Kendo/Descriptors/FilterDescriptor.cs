using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Artech.AspNetCore.Kendo.Descriptors
{
    public class FilterDescriptor
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public object Value { get; set; }
        public string Logic { get; set; }
        public IList<FilterDescriptor> Filters { get; set; }

        private static readonly IDictionary<string, string> Operators = new Dictionary<string, string>
        {
            {"eq", "="},
            {"neq", "!="},
            {"lt", "<"},
            {"lte", "<="},
            {"gt", ">"},
            {"gte", ">="},
            {"startswith", "StartsWith"},
            {"endswith", "EndsWith"},
            {"contains", "Contains"},
            {"doesnotcontain", "Contains"},
            {"isnull", "="},
            {"isnotnull", "!="},
            {"isempty", "="},
            {"isnotempty", "!="},
            {"isnullorempty", ""},
            {"isnotnullorempty", "!"}
        };

        //public string ActualField => Field ?? Filters[0].Field;

        /// <summary>
        /// Get a flattened list of all child filter expressions.
        /// </summary>
        public IList<FilterDescriptor> All()
        {
            var filters = new List<FilterDescriptor>();
            Collect(filters);
            return filters;
        }

        private void Collect(IList<FilterDescriptor> filters)
        {
            if (Filters != null && Filters.Any())
            {
                foreach (var filter in Filters)
                {
                    filter.Collect(filters);
                }
            }
            else
            {
                filters.Add(this);
            }
        }

        public string ToExpression<T>(IList<FilterDescriptor> filters, Func<string,string> mappingLambda)
        {
            if (Filters != null && Filters.Any())
            {
                return "(" + String.Join(" " + Logic + " ", Filters.Select(filter => filter.ToExpression<T>(filters, mappingLambda)).ToArray()) + ")";
            }

            int index = filters.IndexOf(this);
            var comparison = Operators[Operator];
            var currentPropertyType = typeof(T).GetRuntimeProperties().FirstOrDefault(f => f.Name.Equals(Field, StringComparison.OrdinalIgnoreCase))?.PropertyType;
            Field = mappingLambda.Invoke(Field);

            if (Operator == "doesnotcontain")
            {
                if (currentPropertyType == typeof(string))
                    return String.Format("!{0}.{1}(@{2})", Field, comparison, index);
                else
                    return String.Format("({0} != null && !{0}.ToString().{1}(@{2}))", Field, comparison, index);
            }

            if (Operator == "isnull" || Operator == "isnotnull")
            {
                return String.Format("{0} {1} NULL", Field, comparison);
            }

            if (Operator == "isempty" || Operator == "isnotempty")
            {
                if (currentPropertyType == typeof(string))
                    return String.Format("{0} {1} String.Empty", Field, comparison);
                else
                    throw new NotSupportedException(String.Format("Operator {0} not support non-string type", Operator));
            }

            if (Operator == "isnullorempty" || Operator == "isnotnullorempty")
            {
                if (currentPropertyType == typeof(System.String))
                    return String.Format("{0}String.IsNullOrEmpty({1})", comparison, Field);
                else
                    throw new NotSupportedException(String.Format("Operator {0} not support non-string type", Operator));
            }

            if (comparison == "StartsWith" || comparison == "EndsWith" || comparison == "Contains")
            {
                if (currentPropertyType == typeof(System.String))
                    return String.Format("{0}.{1}(@{2})", Field, comparison, index);
                else
                    return String.Format("({0} != null && {0}.ToString().{1}(@{2}))", Field, comparison, index);
            }

            return String.Format("{0} {1} @{2}", Field, comparison, index);
        }
    }
}
