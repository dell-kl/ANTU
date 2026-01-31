using System.Collections.Immutable;
using System.Net;

namespace Modelos.ResultDto;

public struct RequestResultDto<T>
{
    public readonly T Value;
    public static implicit operator RequestResultDto<T>(T value) => new RequestResultDto<T>(value, HttpStatusCode.OK);
    public static implicit operator RequestResultDto<T>(List<Error> errors) => new RequestResultDto<T>(errors, System.Net.HttpStatusCode.BadRequest);
    public List<Error> Errors = new();
    public readonly HttpStatusCode HttpStatusCode;
    
    public bool Success => Errors.Count == 0;

    public RequestResultDto(string error, HttpStatusCode statusCode)
    {
        HttpStatusCode = statusCode;
        Errors.Add(new Error(error));
    }

    public RequestResultDto(T value, HttpStatusCode statusCode)
    {
        Value = value;
        Errors = [];
        HttpStatusCode = statusCode;
    }
    
    public RequestResultDto(List<Error> errors, HttpStatusCode statusCode)
    {
        if (errors.Count == 0)
        {
            throw new InvalidOperationException("Deberias especificar al menos un error");
        }

        HttpStatusCode = statusCode;
        Value = default(T);
        Errors = errors;
    }
}