using src.DeviceManager.Repositories;
using src.DeviceManager.Services;
using src.DTO;

namespace src.DeviceManager.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository) =>
        _employeeRepository = employeeRepository;

    public async Task<List<GetEmployeesDto>> GetAllEmployees(CancellationToken cancellationToken)
    {
        var employees = await _employeeRepository.GetAllEmployees(cancellationToken);

        return employees.Select(e => new GetEmployeesDto
        {
            Id = e.Id,
            FullName = $"{e.Person.FirstName} {e.Person.MiddleName} {e.Person.LastName}"
        }).ToList();
    }

    public async Task<GetEmployeeDto?> GetEmployeeById(int id, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetEmployeeById(id, cancellationToken);
        if (employee is null) return null;

        return new GetEmployeeDto
        {
            PersonDto = new PersonDto
            {
                Id = employee.Id,
                FirstName = employee.Person.FirstName,
                MiddleName = employee.Person.MiddleName,
                LastName = employee.Person.LastName,
                Email = employee.Person.Email,
                PassportNumber = employee.Person.PassportNumber,
                PhoneNumber = employee.Person.PhoneNumber
            },
            PositionDto = new PositionDto
            {
                Id = employee.Position.Id,
                PositionName = employee.Position.Name
            },
            HireDate = employee.HireDate,
            Salary = employee.Salary
        };
    }
}