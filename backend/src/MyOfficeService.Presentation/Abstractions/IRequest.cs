namespace MyOfficeService.Presentation.Abstractions;

/// <summary>
/// IRequest - это интерфейс маркер, который нужен просто для имплементации метода для удобного мапинга 
/// </summary>
/// <typeparam name="TReturn">Возвращает нужную command или query</typeparam>
public interface IRequest<TReturn>
{
    public TReturn ToCommand();
}

/// <summary>
/// IRequest - это интерфейс маркер, который нужен просто для имплементации метода для удобного мапинга
/// </summary>
/// <typeparam name="TParam">Какой-то параметры, который нужен для паминга в command или query.
/// Это может быть Id, который принимается из Route.</typeparam>
/// <typeparam name="TReturn">Возвращает нужную command или query</typeparam>
public interface IRequest<TParam, TReturn>
{
    public TReturn ToCommand(TParam param);
}