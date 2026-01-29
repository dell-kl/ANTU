using System.Collections.Immutable;
using System.Net;

namespace Modelos.ResultDto;

public static partial class Result
{
    public static RequestResultDto<T> Failure<T>(ImmutableArray<Error> errors) => new RequestResultDto<T>(errors, HttpStatusCode.BadRequest);
    public static RequestResultDto<T> Failure<T>(ImmutableArray<Error> errors, HttpStatusCode httpStatusCode) => new RequestResultDto<T>(errors, httpStatusCode);
    public static RequestResultDto<T> Failure<T>(Error error) => new RequestResultDto<T>(ImmutableArray.Create(error), HttpStatusCode.BadRequest);
    public static RequestResultDto<T> Failure<T>(string error) => new RequestResultDto<T>(ImmutableArray.Create(Error.Create(error)), HttpStatusCode.BadRequest);
    public static RequestResultDto<T> Failure<T>(Guid errorCode) => Failure<T>(Error.Create(errorCode));
    public static RequestResultDto<T> Failure<T>(this T result, HttpStatusCode httpStatusCode) => new RequestResultDto<T>(result, httpStatusCode);
}