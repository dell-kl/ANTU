namespace Modelos.ResultDto;

public class Error
{
    public readonly string Message;

    public readonly Guid? ErrorCode;

    public readonly string[] TranslationVariables;

    private Error(string message, Guid? errorCode, string[] translationVariables)
    {
        Message = message;
        ErrorCode = errorCode;
        TranslationVariables = translationVariables;
    }
    
    public static Error Create(string message, Guid? errorCode = null, string[] translationVariables = null)
    {
        return new Error(message, errorCode, translationVariables);
    }
    
    public static Error Create(Guid errorCode, string[] translationVariables = null)
    {
        return Error.Create(string.Empty, errorCode, translationVariables);
    }

    public static IEnumerable<Error> Exception(Exception e)
    {
        if (e is ErrorResultException errs)
        {
            return errs.Errors;
        }

        return new[]
        {
            Create(e.ToString())
        };
    }
}