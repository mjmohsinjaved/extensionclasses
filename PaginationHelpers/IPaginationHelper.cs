using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Filter;
using SharedLibrary.Services.ServiceUri;
using SharedLibrary.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaginationHelpers
{
    public interface IPaginationHelper
    {
        Task<PagedResponse<List<T>>> CreatePagingAsync<T>(IQueryable<T> source, int pageIndex, int pageSize, string route);
        Task<PagedResponse<List<TDestination>>> CreatePagingAsync<TSource, TDestination>(IQueryable<TSource> source, int pageIndex, int pageSize, string route);
        PagedResponse<List<T>> CreatePagedReponse<T>(List<T> pagedData, PaginationFilter validFilter, int totalRecords, string route);
    }
}
