using System.Runtime.ExceptionServices;

namespace Modelos.ResultDto;

public static class Result_Map
{
    public static void Map<T, U>(this RequestResultDto<T> r, Func<T, U> mapper)
    {
        try
        {
            
            
            // return r.Bind(x => mapper(x).Success());
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }
}