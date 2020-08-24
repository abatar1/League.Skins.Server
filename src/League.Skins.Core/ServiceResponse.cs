using System;

namespace League.Skins.Core
{
    public enum ErrorServiceCodes
    {
        Unknown,
        WrongEnumFormat,
        ChestDropInvalidModel,
        EntityAlreadyExists,
        WrongLoginOrPassword,
        EntityNotFound,
        UserNotRelated,
        SelfRelation
    }

    public class ServiceErrorResponse
    {
        public ErrorServiceCodes Code { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }

    public class ServiceResponse<T> : ServiceResponse
        where T : class
    {
        public T SuccessResponse { get; set; }

        public static ServiceResponse<T> Ok(T result) => new ServiceResponse<T> {SuccessResponse = result};
    }

    public class ServiceResponse
    {
        public ServiceErrorResponse ErrorResponse { get; private set; }

        public bool HasError => ErrorResponse != null;

        public static ServiceResponse Ok() => new ServiceResponse();

        public static ServiceResponse Error(ErrorServiceCodes code, string message) => new ServiceResponse
            {ErrorResponse = new ServiceErrorResponse {Code = code, Message = message}};

        public static ServiceResponse Error(ErrorServiceCodes code, Exception e) => new ServiceResponse
            {ErrorResponse = new ServiceErrorResponse {Code = code, Message = e.Message, StackTrace = e.StackTrace}};
    }
}
