using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Phishtank.Common.Exceptions
{
    public abstract class ApiException : Exception
    {
        public virtual HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public ApiException()
        {

        }

        public ApiException(string message) : base (message)
        {

        }
    }
}
