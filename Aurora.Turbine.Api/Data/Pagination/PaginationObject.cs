namespace Aurora.Turbine.Api.Data.Pagination
{
    public class PaginationObject<TData>
    {
        public List<TData> Result { get; set; } = new();
        public int Page { get; set; }
        public int Size { get; set; }
        public int TotalPages { get; set; }
        public long Count { get; set; }
    }

    public class PaginationObjectBuilder<TData>
    {
        private readonly PaginationObject<TData> _paginationObject = new();

        public static PaginationObjectBuilder<TData> Create()
        {
            return new PaginationObjectBuilder<TData>();
        }

        public PaginationObjectBuilder<TData> Results(List<TData> results)
        {
            _paginationObject.Result = results;

            return this;
        }

        public PaginationObjectBuilder<TData> Size(int size)
        {
            _paginationObject.Size = size;
            return this;
        }

        public PaginationObjectBuilder<TData> Page(int page)
        {
            _paginationObject.Page = page;
            return this;
        }

        public PaginationObjectBuilder<TData> TotalPages(int totalPages)
        {
            _paginationObject.TotalPages = totalPages;
            return this;
        }

        public PaginationObjectBuilder<TData> Count(long count)
        {
            _paginationObject.Count = count;
            return this;
        }

        public PaginationObject<TData> Build()
        {
            if (_paginationObject.TotalPages == 0)
            {
                _paginationObject.TotalPages = (int)_paginationObject.Count / _paginationObject.Size;
            }

            return _paginationObject;
        }
    }
}
