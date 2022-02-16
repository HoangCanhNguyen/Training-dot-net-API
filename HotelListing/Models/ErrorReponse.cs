using System.Collections.Generic;

namespace HotelListing.Models
{
    public class ErrorReponse
    {
        public List<Error> Errors { get; set; } = new List<Error>();
    }
}
