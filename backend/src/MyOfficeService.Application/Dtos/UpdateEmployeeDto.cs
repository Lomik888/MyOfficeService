using CSharpFunctionalExtensions;
using MyOfficeService.Domain.Errors;
using MyOfficeService.Domain.Extations;
using MyOfficeService.Domain.ValueObjects;

namespace MyOfficeService.Application.Dtos;

public class UpdateEmployeeDto
{
    public int Id { get; private set; }

    public string? Name { get; private set; }

    public string? Phone { get; private set; }

    public int? CompanyId { get; private set; }

    public int? DepartmentId { get; private set; }

    public Passport? Passport { get; private set; }

    private UpdateEmployeeDto(
        int id,
        string? name,
        string? phone,
        int? companyId,
        int? departmentId,
        Passport? passport)
    {
        Id = id;
        Name = name;
        Phone = phone;
        CompanyId = companyId;
        DepartmentId = departmentId;
        Passport = passport;
    }

    public static Result<UpdateEmployeeDto, ErrorList> Create(
        int id,
        string? name,
        string? phone,
        int? companyId,
        int? departmentId,
        Passport? passport)
    {
        if ((name != null ||
             phone != null ||
             companyId != null ||
             departmentId != null ||
             passport != null) == false)
        {
            var error = GeneralErrors.Validation.InvalidField("Нет данных для обновления сотрудника");
            return error.ToErrorList();
        }

        return new UpdateEmployeeDto(id, name, phone, companyId, departmentId, passport);
    }
}