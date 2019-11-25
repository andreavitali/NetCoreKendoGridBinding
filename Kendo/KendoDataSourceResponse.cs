using Artech.AspNetCore.Kendo.Descriptors;
using AutoMapper;
using NetCoreKendoAngularGridBinding.Kendo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Artech.AspNetCore.Kendo
{
    public class KendoDataSourceResponse<TEntity, TDto>
    {
        public IEnumerable Data { get; set; }
        public int Total { get; set; }
        public IEnumerable Groups { get; set; }
        //public object Aggregates { get; set; }

        private Dictionary<string, MapExpression<TEntity>> _mappings = null;

        public KendoDataSourceResponse(IQueryable<TEntity> queryable, KendoDataSourceRequest request, IMapper mapper = null)
        {
            SetModelMappings(mapper);

            // Filter the data first
            queryable = ApplyFilter(queryable, request.FilterWrapper);

            // Calculate the total number of records (needed for paging)
            this.Total = queryable.Count();

            // Calculate the aggregates
            //var aggregates = Aggregates(queryable, aggregates);

            // Add sort for grouped fields
            if (request.Groups != null && request.Groups.Any())
            {
                if (request.Sorts == null) request.Sorts = new List<SortDescriptor>();

                foreach (var source in request.Groups.Reverse())
                {
                    request.Sorts.Append(new SortDescriptor
                    {
                        Field = source.Field,
                        Dir = source.Dir
                    });
                }
            }

            // Sort the data
            queryable = ApplySort(queryable, request.Sorts);

            // Finally page the data
            if (request.PageSize > 0)
            {
                queryable = ApplyPaging(queryable, request.Page, request.PageSize);
            }

            // Group By
            if (request.Groups != null && request.Groups.Any())
            {
                this.Groups = ApplyGrouping(queryable, request.Groups);
            }
            else
            {
                if (mapper != null && typeof(TEntity) != typeof(TDto))
                {
                    this.Data = mapper.ProjectTo<TDto>(queryable).ToList();
                }
                else
                {
                    this.Data = queryable.ToList();
                }
            }
        }

        private IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> queryable, FilterDescriptor filterObject)
        {
            if (filterObject != null && filterObject.Filters.Count > 0)
            {
                var filters = filterObject.All().ToList();
                var values = filters.Select(f => f.Value).ToArray();

                string predicate = string.Empty;

                try
                {
                    predicate = filterObject.ToExpression<TDto>(filters, MapFieldFromDTOtoEntity);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return queryable;
                }
                // Use the Where method of Dynamic Linq to filter the data
                queryable = queryable.Where(predicate, values);
            }

            return queryable;
        }

        private IQueryable<TEntity> ApplySort(IQueryable<TEntity> queryable, IEnumerable<SortDescriptor> sorts)
        {
            if (sorts != null && sorts.Any())
            {
                var ordering = string.Join(",", sorts.Select(s => MapFieldFromDTOtoEntity(s.Field) + " " + s.Dir));
                return queryable.OrderBy(ordering);
            }

            return queryable;
        }

        private IEnumerable<GroupResult> ApplyGrouping(IQueryable<TEntity> queryable, IEnumerable<GroupDescriptor> groups)
        {
            var groupSelector = groups.Select(g => MapFieldFromDTOtoEntity(g.Field));
            return queryable.GroupByMany(groupSelector.ToArray());
        }

        private IQueryable<TEntity> ApplyPaging(IQueryable<TEntity> queryable, int page, int pageSize)
        {
            return queryable.Take(pageSize).Skip((page - 1) * pageSize);
        }

        private void SetModelMappings(IMapper mapper)
        {
            if (typeof(TEntity) == typeof(TDto) || mapper == null)
                return;

            var map = mapper.ConfigurationProvider?.FindTypeMapFor<TEntity, TDto>();
            if (map == null)
                return;

            this._mappings = new Dictionary<string, MapExpression<TEntity>>();

            // Custom expressions because they do not map field to field
            foreach (var propertyMap in map.PropertyMaps.Where(pm => pm.CustomMapExpression == null))
            {
                if(propertyMap.CustomMapExpression != null)
                {
                    // Get the linq expression body
                    string body = propertyMap.CustomMapExpression.Body.ToString();

                    // Get the item tag
                    string tag = propertyMap.CustomMapExpression.Parameters[0].Name;

                    string destination = body.Replace($"{tag}.", string.Empty);
                    string source = propertyMap.DestinationName/*.ToLower()*/;

                    var customExpression = new MapExpression<TEntity>
                    {
                        Path = destination,
                        Expression = propertyMap.CustomMapExpression.ToTypedExpression<TEntity>()
                    };

                    if (!_mappings.ContainsKey(source))
                    {
                        _mappings.Add(source, customExpression);
                    }
                }
            }

            foreach (var propertyMap in map.PropertyMaps.Where(pm => pm.CustomMapExpression == null))
            {
                // TODO
                // Automatic Automapper mapping (eg: Department.Name -> DepartmentName)
            }
        }

        private string MapFieldFromDTOtoEntity(string field)
        {
            var lField = field/*.ToLower()*/;
            return _mappings != null && field != null && _mappings.ContainsKey(lField) ? _mappings[lField].Path : field;
        }
    }
}
