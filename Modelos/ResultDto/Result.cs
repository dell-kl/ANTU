using System.Net;

namespace Modelos.ResultDto;

public static partial class Result
{
    public static readonly Unit Unit = Unit.Value;

    public static RequestResultDto<T> Success<T>(this T value) => new RequestResultDto<T>(value, HttpStatusCode.OK);   
    public static RequestResultDto<T> Success<T>(this T value, HttpStatusCode httpStatusCode) => new RequestResultDto<T>(value, httpStatusCode);
    public static RequestResultDto<Unit> Success() => new RequestResultDto<Unit>(Unit, HttpStatusCode.OK);
}