using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Phishtank.Common.Exceptions
{
    public class InvalidShortenUrlRequestException : ApiException
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public InvalidShortenUrlRequestException()
        {

        }

        public InvalidShortenUrlRequestException(string message) : base(message)
        {

        }
    }
}
