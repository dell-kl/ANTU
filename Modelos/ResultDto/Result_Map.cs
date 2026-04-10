using System.Runtime.ExceptionServices;

namespace Modelos.ResultDto;

public static class Result_Map
{

    public static RequestResultDto<Um> Map<Tm, Um>(this RequestResultDto<Tm> r, Func<Tm, Um> mapper)
    {
        try
        {
            return r.Bind<Tm, Um>(x => mapper(x));
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }

    public static async Task<RequestResultDto<U>> Map<T, U>(this RequestResultDto<T> r, Func<T, Task<U>> mapper)
    {
        try
        {
            // return await r.Bind<T,U>(async x => (await mapper(x)).Success());
            throw new NotImplementedException();
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }

    public static async Task<RequestResultDto<U>> Map<T, U>(this Task<RequestResultDto<T>> result, Func<T, Task<U>> mapper)
    {
        try
        {
            var r = await result;
            return await r.Map(mapper);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }

    public static async Task<RequestResultDto<U>> Map<T, U>(this Task<RequestResultDto<T>> result, Func<T, U> mapper)
    {
        try
        {
            var r = await result;
            return r.Map(mapper);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }
}