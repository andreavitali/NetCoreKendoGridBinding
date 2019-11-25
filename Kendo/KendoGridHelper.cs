using Artech.AspNetCore.Kendo.Descriptors;
using AutoMapper;
using System;
using System.Collections.Generic;

namespace Artech.AspNetCore.Kendo
{
    public class KendoGridHelper
    {
        readonly INamingService _namingService;

        public KendoGridHelper(INamingService namingService)
        {
            _namingService = namingService;
        }

        #region Filter

        public FilterDescriptor ParseFilter(string input)
        {
            if (input.StartsWith("("))
                input = input.Substring(1, input.Length - 2);

            // Remove all of the ' characters from the string and split
            var ss = input.Replace("'", "").Split('~');

            var filters = new List<FilterDescriptor>(); // Used to store all of the parsed filters (one for each field)
            int fIndex = -1; // Used to track filter index.
            int cIndex = 0; // Used to track filter index within a composite filter object.
            bool isComposite = false; // Used to indicate if a composite filter is currently being parsed.

            for (var i = 0; i < ss.Length; i++)
            {
                if (i % 4 == 0) // Field
                {
                    string fieldName = ss[i].Replace("(", "");
                    string convertedFieldName = _namingService.Convert(fieldName);

                    if ((i > 0 && ss[i].IndexOf('(') > -1) || (ss.Length > i + 4 && fieldName == ss[i+4]))  // We're starting a composite object
                    {
                        filters.Add(new FilterDescriptor() { Filters = new List<FilterDescriptor>() }); // create a composite object and add it to the parsed filters
                        fIndex++; // We added an object to the array, so increment the counter.
                        filters[fIndex].Filters.Add(new FilterDescriptor() { Field = convertedFieldName });
                        cIndex = 0; // We added the first filter to the composite object, so set the counter.
                        isComposite = true;
                    }
                    else if (isComposite)
                    {
                        // If we're parsing the second filter in a composite filter object, then add the field to the child filter.
                        filters[fIndex].Filters.Add(new FilterDescriptor() { Field = convertedFieldName });
                        cIndex++; // We added the second filter to the composite object, so increment the counter.
                    }
                    else // Add the field as normal.
                    {
                        filters.Add(new FilterDescriptor() { Field = convertedFieldName });
                        fIndex++; // We added an object to the array, so increment the counter.
                    }
                }
                if (i % 4 == 1) // Operator
                {
                    if (isComposite)
                    {
                        filters[fIndex].Filters[cIndex].Operator = ss[i];
                    }
                    else
                    {
                        filters[fIndex].Operator = ss[i];
                    }
                }
                if (i % 4 == 2)  // Value
                {
                    string sVal = ss[i].IndexOf(')') > -1 ? ss[i].Replace(")", "") : ss[i];
                    object value = ParseValue(sVal);

                    if (i != ss.Length - 1 && ss[i].IndexOf(')') > -1)
                    {
                        filters[fIndex].Filters[cIndex].Value = value;/* ss[i].Replace(")", "");*/
                        isComposite = false;
                    }
                    else if (isComposite)
                    {
                        filters[fIndex].Filters[cIndex].Value = value/*ss[i]*/;
                    }
                    else
                    {
                        filters[fIndex].Value = value/*ss[i]*/;
                    }
                }
                if (i % 4 == 3) // Logic
                {
                    if (isComposite)
                    {
                        filters[fIndex].Logic = ss[i]; // Add the logic to the composite filter object.
                    }
                    // If the filter is not composite, the logic will always be "and". So, we just don't do anything if that's the case.
                }
            }

            return new FilterDescriptor()
            {
                Filters = filters,
                Logic = "and"
            };
        }

        private object ParseValue(string sVal)
        {
            bool bVal;
            if (bool.TryParse(sVal, out bVal))
            {
                return bVal;
            }

            int iVal;
            if (int.TryParse(sVal, out iVal))
            {
                return iVal;
            }

            if (sVal.StartsWith("datetime"))
            {
                return DateTime.ParseExact(sVal.Substring(8), "yyyy-MM-ddTHH-mm-ss", null);
            }

            if (sVal == "null")
            {
                return null;
            }
            else
            {
                return sVal;
            }
        }

        #endregion

        #region Sort / Group

        public IList<T> Deserialize<T>(string from) where T : IKendoDescriptor, new()
        {
            var result = new List<T>();

            if (string.IsNullOrEmpty(from))
            {
                return result;
            }

            var components = from.Split('~', StringSplitOptions.RemoveEmptyEntries);

            foreach (string component in components)
            {
                var descriptor = new T();
                descriptor.Deserialize(component);
                descriptor.Field = _namingService.Convert(descriptor.Field);
                result.Add(descriptor);
            }

            return result;
        }

        #endregion
    }
}
