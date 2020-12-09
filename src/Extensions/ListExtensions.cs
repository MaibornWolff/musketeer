using System.Collections.Generic;
using System.Linq;
using Musketeer.Exceptions;

namespace Musketeer.Extensions
{
    public static class ListExtensions
    {
        public static List<T> SortBy<T>(this List<T> list, string field, string direction)
        {
            var fieldName = char.ToUpper(field[0]) + field.Substring(1);
            
            if(direction == "asc")
                return list.OrderBy(listElement => listElement.GetType().GetProperty(fieldName)?.GetValue(listElement)).ToList();
            if(direction == "desc")
                return list.OrderByDescending(listElement => listElement.GetType().GetProperty(fieldName)?.GetValue(listElement)).ToList();
            throw new InvalidSortDirectionException(direction);
        }

        public static List<T> FilterBy<T>(this List<T> list, string field, string filterText)
        {
            var fieldName = char.ToUpper(field[0]) + field.Substring(1);
            return list.Where(listElement => listElement.GetType().GetProperty(fieldName)?.GetValue(listElement)
                .ToString().Contains(filterText) ?? false).ToList();
        }
    }
}