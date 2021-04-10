using System;
using System.Collections.Generic;
using System.Text;

namespace JJS.Application.Wrappers
{
    public class Response<T>
    {
        public Response()
        {
        }
        public Response(T data, string message = null, int pageIndex = 0, int totalPages = 0, int totalItems = 0, bool hasPreviousPage = false, bool hasNextPage = false)
        {
            Succeeded = true;
            Message = message;
            Data = data;
            PageIndex = pageIndex;
            TotalPages = totalPages;
            TotalItems = totalItems;
            HasPreviousPage = hasPreviousPage;
            HasNextPage = hasNextPage;

        }
        public Response(string message)
        {
            Succeeded = false;
            Message = message;
        }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }
}
