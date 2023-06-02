using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PaginationHelpers;
using UriService;
using SharedLibrary.Wrappers;
namespace PaginationHelpers
{
    public class PaginationHelper : IPaginationHelper
    {
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        public PaginationHelper(IMapper mapper, IUriService uriService){
            _mapper = mapper;
            _uriService = uriService;   
        }
        public  PagedResponse<List<T>> CreatePagedReponse<T>(List<T> pagedData, PaginationFilter paginationModel, int totalRecords,string route)
        {
            var respose = new PagedResponse<List<T>>(pagedData, paginationModel.PageNumber, paginationModel.PageSize);
            var totalPages = ((double)totalRecords / (double)paginationModel.PageSize);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            respose.NextPage =
                paginationModel.PageNumber >= 1 && paginationModel.PageNumber < roundedTotalPages
                ? _uriService.GetPageUri(new PaginationFilter(paginationModel.PageNumber + 1, paginationModel.PageSize), route)
                : null;
            respose.PreviousPage =
                paginationModel.PageNumber - 1 >= 1 && paginationModel.PageNumber <= roundedTotalPages
                ? _uriService.GetPageUri(new PaginationFilter(paginationModel.PageNumber - 1, paginationModel.PageSize), route)
                : null;
            respose.FirstPage = _uriService.GetPageUri(new PaginationFilter(1, paginationModel.PageSize), route);
            respose.LastPage = _uriService.GetPageUri(new PaginationFilter(roundedTotalPages, paginationModel.PageSize), route);
            respose.TotalPages = roundedTotalPages;
            respose.TotalRecords = totalRecords;
            return respose;
        }
        public  async Task<PagedResponse<List<T>>> CreatePagingAsync<T>(IQueryable<T> source, int pageIndex, int pageSize,string route)
        {
            var filter = new PaginationFilter(pageIndex, pageSize);
            var count = await source.CountAsync();
            var items = await source.Skip((filter.PageNumber- 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
            return CreatePagedReponse(items, filter, count,  route
            );
        }
        public  async Task<PagedResponse<List<TDestination>>> CreatePagingAsync<TSource,TDestination>(IQueryable<TSource> source, int pageIndex, int pageSize, string route)
        {
            var filter = new PaginationFilter(pageIndex, pageSize);
            var count = await source.CountAsync();
            var items = await source.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
            var data = (List<TDestination>)_mapper.Map(items, items.GetType(), typeof(List<TDestination>));
            return CreatePagedReponse(data,filter,count, route);
        }
    }
    
}
