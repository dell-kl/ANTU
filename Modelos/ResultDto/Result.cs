using System;
using System.Collections.Immutable;
using System.Net;

namespace Modelos.ResultDto;

public static partial class Result
{
    public static RequestResultDto<T> Success<T>(this T value) => new RequestResultDto<T>(value, HttpStatusCode.OK);   
    public static RequestResultDto<T> Success<T>(this T value, HttpStatusCode httpStatusCode) => new RequestResultDto<T>(value, httpStatusCode);
}