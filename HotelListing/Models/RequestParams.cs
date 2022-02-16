namespace HotelListing.Models
{
    public class RequestParams
    {
        const int maxPageSize = 10;
        public int PageNumber { get; set; }
        private int _pageSizie = 10;

        public int PageSize {
            get { return _pageSizie; }
            set {  _pageSizie = (value > maxPageSize) ? maxPageSize : value; }
        }

    }
}
