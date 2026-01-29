using System.Collections.Immutable;

namespace Modelos.ResultDto;

public class ErrorResultException : Exception
{
    public ImmutableArray<Error> Errors { get; }

    public ErrorResultException(ImmutableArray<Error> errors)
        : base(ValidateAndGetErrorMessage(errors))
    {
        Errors = errors;
    }

    public ErrorResultException(Error error)
        : this(new[] { error }.ToImmutableArray())
    {
    }

    private static string ValidateAndGetErrorMessage(ImmutableArray<Error> errors)
    {
        if (errors.Length == 0)
        {
            throw new Exception("You should include at least one Error");
        }

        if (errors.Length == 1)
        {
            return errors[0].Message;
        }

        var n = errors.Select(item => item.Message);

        return String.Join(", ", n);
    }
}