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

    public static async Task<RequestResultDto<U>> Bind<T, U>(this RequestResultDto<T> r, Func<ObservableCollection<FileResultExtensible>,string,Task<RequestResultDto<U>>> method, ObservableCollection<FileResultExtensible> files)
    {
        try
        {
            return r.Success 
                ? await method(files, Guid.NewGuid().ToString())
                : Result.Failure<U>(r.Errors, r.HttpStatusCode);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}