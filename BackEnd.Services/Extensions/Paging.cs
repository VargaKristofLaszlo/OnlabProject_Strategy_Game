namespace Services.Extensions
{
    public static class Paging
    {
        public static int ValidatePageNumber(this int pageNumber)
        {
            if (pageNumber < 1)
                pageNumber = 1;
            return pageNumber;
        }

        public static int ValideatePageSize(this int pageSize)
        {
            if (pageSize < 5)
                pageSize = 5;

            if (pageSize > 50)
                pageSize = 50;

            return pageSize;
        }

        public static int CalculatePageCount(int modelCount, int pageSize)
        {
            int pagesCount = modelCount / pageSize;

            if (modelCount % pageSize != 0)
                return pagesCount + 1;

            return pagesCount;
        }
    }
}