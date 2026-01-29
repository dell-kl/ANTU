using System.Collections.Immutable;
using System.Net;

namespace Modelos.ResultDto;

public class RequestResultDto<T>
{
    public readonly T Value;

    public static implicit operator RequestResultDto<T>(T value) => new RequestResultDto<T>(value, HttpStatusCode.OK);

    public static implicit operator RequestResultDto<T>(ImmutableArray<Error> errors) => new RequestResultDto<T>(errors, System.Net.HttpStatusCode.BadRequest);

    public readonly ImmutableArray<Error> Errors;

    public readonly HttpStatusCode HttpStatusCode;
    
    public bool Success => Errors.Length == 0;

    public RequestResultDto(T value, HttpStatusCode statusCode)
    {
        Value = value;
        Errors = ImmutableArray<Error>.Empty;
        HttpStatusCode = statusCode;
    }
    
    public RequestResultDto(ImmutableArray<Error> errors, HttpStatusCode statusCode)
    {
        if (errors.Length == 0)
        {
            throw new InvalidOperationException("Deberias especificar al menos un error");
        }

        HttpStatusCode = statusCode;
        Value = default(T);
        Errors = errors;
    }
}