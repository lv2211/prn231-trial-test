using System.Net;

namespace PE.Core.Commons
{
    public record ApiResponseModel<T>
    {
        /// <summary>
        /// Status code of the response
        /// </summary>
        public required HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Response message
        /// </summary>
        public required string Message { get; set; }

        /// <summary>
        /// Response data
        /// </summary>
        public T? Response { get; set; }
    }
}
