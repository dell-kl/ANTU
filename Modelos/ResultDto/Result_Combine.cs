using System.Runtime.ExceptionServices;

namespace Modelos.ResultDto;

public static class Result_Combine
{
    public static RequestResultDto<(T1, T2)> Combine<T1, T2>(this RequestResultDto<T1> r, Func<T1, RequestResultDto<T2>> action)
    {
        try
        {
            return r.Bind(action)
                    .Map(x => (r.Value, x));
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }
    
    public static async Task<RequestResultDto<(T1, T2)>> Combine<T1, T2>(this RequestResultDto<T1> r, Func<T1, Task<RequestResultDto<T2>>> action)
    {
        try
        {
            return await r.Bind(action)
                          .Map(x => (r.Value, x));
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }
    
    public static async Task<RequestResultDto<(T1, T2)>> Combine<T1, T2>(this Task<RequestResultDto<T1>> result, Func<T1, Task<RequestResultDto<T2>>> action)
    {
        try
        {
            RequestResultDto<T1> r = await result;
            return await r.Combine(action);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }

    public static async Task<RequestResultDto<(T1, T2)>> Combine<T1, T2>(this Task<RequestResultDto<T1>> result, Func<T1, RequestResultDto<T2>> action)
    {
        try
        {
            RequestResultDto<T1> r = await result;
            return r.Combine(action);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }
}