using System.Collections.ObjectModel;
using System.Runtime.ExceptionServices;
using Modelos.Dto;

namespace Modelos.ResultDto;

public static class Result_Bind
{
    public static RequestResultDto<U> Bind<T, U>(this RequestResultDto<T> r, Func<T, RequestResultDto<U>> method)
    {
        try
        {
            return r.Success
                ? method(r.Value)
                : Result.Failure<U>(r.Errors, r.HttpStatusCode);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }
    
    public static async Task<RequestResultDto<U>> Bind<T, U>(this RequestResultDto<T> r, Func<T, Task<RequestResultDto<U>>> method)
    {
        try
        {
            return r.Success
                ? await method(r.Value)
                : Result.Failure<U>(r.Errors, r.HttpStatusCode);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }

    public static async Task<RequestResultDto<U>> Bind<T, U>(this Task<RequestResultDto<T>> result, Func<T, Task<RequestResultDto<U>>> method)
    {
        try
        {
            var r = await result;
            return await r.Bind(method);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }

    public static async Task<RequestResultDto<U>> Bind<T, U>(this Task<RequestResultDto<T>> result, Func<T, RequestResultDto<U>> method)
    {
        try
        {
            var r = await result;
            return r.Bind(method);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }
    
    //este fue para algo de imagenes, necesitamos hacer una modificacion ... no me gusta esta parte.
    public static async Task<RequestResultDto<U>> Bind<T, U>(this RequestResultDto<T> r, Func<ObservableCollection<FileResultExtensible>,string,Task<RequestResultDto<U>>> method, ObservableCollection<FileResultExtensible> files, string identificador)
    {
        try
        {
            if (r.Success)
            {
               RequestResultDto<U> resultado = await method(files, identificador);

               if (resultado.Success || r.Success)
                   resultado.Successful.AddRange(r.Successful);
               
               return resultado;
            }
            
            return Result.Failure<U>(r.Errors, r.HttpStatusCode);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }
}