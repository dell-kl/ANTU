using System.Net;

namespace Modelos.ResultDto;

public static partial class Result
{
    public static RequestResultDto<T> Failure<T>(List<Error> errors) => new RequestResultDto<T>(errors, HttpStatusCode.BadRequest);
    public static RequestResultDto<T> Failure<T>(List<Error> errors, HttpStatusCode httpStatusCode) => new RequestResultDto<T>(errors, httpStatusCode);
    public static RequestResultDto<T> Failure<T>(this T result, HttpStatusCode httpStatusCode) => new RequestResultDto<T>(result, httpStatusCode);
    public static RequestResultDto<T> Failure<T>(string error, HttpStatusCode httpStatusCode) => new RequestResultDto<T>(error, httpStatusCode);
}