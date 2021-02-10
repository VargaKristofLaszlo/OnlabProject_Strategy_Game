using System.Collections.Generic;
using System.Linq;

namespace BackEnd.Services.Extensions
{
    public static class ViewServiceExtension
    {

        public static void ValidatePageNumber(this ref int pageNumber)
        {
            if (pageNumber < 1)
                pageNumber = 1;
        }

        public static void ValideatePageSize(this ref int pageSize)
        {
            if (pageSize < 5)
                pageSize = 5;

            if (pageSize > 50)
                pageSize = 50;
        }
       
    }
}
