using System.Collections;

namespace MyOfficeService.Domain.Errors;

/// <summary>
/// ErrorList - коллекция Error с дополнительной логикой
/// </summary>
public class ErrorList : IEnumerable<Error>
{
    private readonly List<Error> _errors;

    public IReadOnlyList<Error> Values => _errors;

    public ErrorList(List<Error> errors)
    {
        _errors = errors;
    }

    public static ErrorList Create(List<Error> errorsList)
    {
        var errors = new ErrorList(errorsList);
        return errors;
    }

    public void Add(ErrorList errors)
    {
        this._errors.AddRange(errors.Values);
    }

    public IEnumerator<Error> GetEnumerator() => _errors.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}