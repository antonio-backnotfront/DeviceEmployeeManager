using Microsoft.EntityFrameworkCore;
using src.DeviceManager.Models;
using src.DeviceManager.Repositories;

namespace src.DeviceManager.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private DeviceEmployeeContext _context;
    public EmployeeRepository(DeviceEmployeeContext context)
    {
        _context = context;
    }
    public async Task<List<Employee>> GetAllEmployees(CancellationToken cancellationToken)
    {
        try
        {
            return await _context.Employees
                .Include(p => p.Person)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error while getting all employees", ex);
        }
    }
    public Task<Employee?> GetEmployeeById(int id, CancellationToken cancellationToken)
    {
        try
        {
            return _context.Employees
                .Include(p => p.Person)
                .Include(pos => pos.Position)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error while getting employee by id", ex);
        }
    }
}