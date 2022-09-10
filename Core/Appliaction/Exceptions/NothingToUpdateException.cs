using System.Net;

namespace Application.Exceptions
{
    public class NothingToUpdateException : CustomException
    {
        public NothingToUpdateException()
        : base("There are no new changes to update.", null, HttpStatusCode.NotAcceptable)
        {
        }
    }
}