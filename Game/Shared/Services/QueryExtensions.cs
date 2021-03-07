using System;
using System.Web;

namespace Game.Shared.Services
{
    public static class QueryExtensions
    {
        public static void SetQueryStringWithCityIndex(this UriBuilder builder, int cityIndex) 
        {
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["cityIndex"] = cityIndex.ToString();
            builder.Query = query.ToString();
        }

        public static void SetPagingQueryString(this UriBuilder builder, int pageNumber, int pageSize)
        {
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["pageNumber"] = pageNumber.ToString();
            query["pageSize"] = pageSize.ToString();
            builder.Query = query.ToString();
        }

        public static void SetQueryStringWithBuildingNameAndStage(this UriBuilder builder, string buildingName, int buildingStage)
        {
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["buildingName"] = buildingName;
            query["buildingStage"] = buildingStage.ToString();
            builder.Query = query.ToString();
        }

        public static void SetQueryStringForBuildingStageModification(this UriBuilder builder, int cityIndex, int newStage)
        {
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["cityIndex"] = cityIndex.ToString();
            query["newStage"] = newStage.ToString();
            builder.Query = query.ToString();
        }
       
    }
}
