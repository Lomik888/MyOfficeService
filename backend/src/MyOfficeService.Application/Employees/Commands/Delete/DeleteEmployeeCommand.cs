using MyOfficeService.Application.Abstractions.Handlers;

namespace MyOfficeService.Application.Employees.Commands.Delete;

public record DeleteEmployeeCommand(int Id) : ICommand;