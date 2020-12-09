using System;

namespace Musketeer.Exceptions
{
    public class InvalidSortDirectionException : Exception
    {
        public InvalidSortDirectionException(string sortDirection) : base($"[{sortDirection}] is not a valid sort direction")
        {
        }

    }
}