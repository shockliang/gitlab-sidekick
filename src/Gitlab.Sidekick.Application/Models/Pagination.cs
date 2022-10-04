using RestSharp;

namespace Gitlab.Sidekick.Application.Models;


    public class Pagination<T> : Pagination
    {
        public Pagination(IReadOnlyCollection<HeaderParameter> headers, T data)
            : base(headers)
        {
            Data = data;
        }

        public Pagination(IReadOnlyCollection<HeaderParameter> headers)
            : base(headers)
        {
        }

        public T Data { get; set; }
    }

    public class Pagination
    {
        public int NextPage { get; set; }
        public int Page { get; set; }
        public int PrevPage { get; set; }
        public int TotalPages { get; set; }
        public int PerPage { get; set; }
        public int Total { get; set; }

        public Pagination(IReadOnlyCollection<HeaderParameter> headers)
        {
            var nextPage = headers.FirstOrDefault(x => x.Name.Equals("X-Next-Page"))?.Value;
            var page = headers.FirstOrDefault(x => x.Name.Equals("X-Page"))?.Value;
            var prevPage = headers.FirstOrDefault(x => x.Name.Equals("X-Prev-Page"))?.Value;
            var totalPages = headers.FirstOrDefault(x => x.Name.Equals("X-Total-Pages"))?.Value;
            var perPage = headers.FirstOrDefault(x => x.Name.Equals("X-Per-Page"))?.Value;
            var total = headers.FirstOrDefault(x => x.Name.Equals("X-Total"))?.Value;

            NextPage = nextPage != null && !string.IsNullOrEmpty(nextPage.ToString())
                ? int.Parse(nextPage.ToString())
                : 0;
            Page = page != null && !string.IsNullOrEmpty(page.ToString())
                ? int.Parse(page.ToString())
                : 0;
            PrevPage = prevPage != null && !string.IsNullOrEmpty(prevPage.ToString())
                ? int.Parse(prevPage.ToString())
                : 0;
            TotalPages = totalPages != null && !string.IsNullOrEmpty(totalPages.ToString())
                ? int.Parse(totalPages.ToString())
                : 0;
            PerPage = perPage != null && !string.IsNullOrEmpty(PerPage.ToString())
                ? int.Parse(perPage.ToString())
                : 0;
            Total = total != null && !string.IsNullOrEmpty(total.ToString())
                ? int.Parse(total.ToString())
                : 0;
        }
    }
