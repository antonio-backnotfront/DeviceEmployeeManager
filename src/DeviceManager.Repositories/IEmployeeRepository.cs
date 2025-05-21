using src.DeviceManager.Models;

namespace src.DeviceManager.Repositories;

public interface IEmployeeRepository
{
    public Task<List<Employee>> GetAllEmployees(CancellationToken cancellationToken);
    public Task<Employee?> GetEmployeeById(int id, CancellationToken cancellationToken);
}