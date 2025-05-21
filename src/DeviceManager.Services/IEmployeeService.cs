using src.DTO;

namespace src.DeviceManager.Repositories;

public interface IEmployeeService
{
    public Task<List<GetEmployeesDto>> GetAllEmployees(CancellationToken cancellationToken);
    public Task<GetEmployeeDto?> GetEmployeeById(int id, CancellationToken cancellationToken);
}