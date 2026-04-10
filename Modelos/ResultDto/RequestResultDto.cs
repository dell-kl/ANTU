using System.Net;
namespace Modelos.ResultDto;

public struct RequestResultDto<T>
{
    public readonly T Value;
    public List<Error> Errors = new(); // vamos recolectando cada error de cada proceso. 
    public List<object?> Successful = new(); // vamos recolectando cada successful de cada proceso.
    public readonly HttpStatusCode HttpStatusCode;
    
    public static implicit operator RequestResultDto<T>(T value) => new RequestResultDto<T>(value, HttpStatusCode.OK);
    
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
        if(value is not Unit && value is not RequestDataImage)
            Successful.Add(value?.ToString());
        else if (value is RequestDataImage resultado)
            Successful.Add(resultado.mensaje);
    }
    
    public RequestResultDto(List<Error> errors, HttpStatusCode statusCode)
    {
        if (errors.Count == 0) {
            throw new InvalidOperationException("Deberias especificar al menos un error");
        }
        HttpStatusCode = statusCode;
        Value = default(T);
        Errors = errors;
    }
}