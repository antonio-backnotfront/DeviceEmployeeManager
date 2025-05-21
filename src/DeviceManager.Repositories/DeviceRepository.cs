using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using src.DeviceManager.Models;
using src.DeviceManager.Repositories;

namespace src.DeviceManager.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly DeviceEmployeeContext _context;

        public DeviceRepository(DeviceEmployeeContext context)
        {
            _context = context;
        }

        public async Task<List<Device>> GetAllDevices(CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Devices.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to retrieve devices." + ex.Message);
            }
        }

        public async Task<Device?> GetDeviceById(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Devices
                    .Include(d => d.DeviceType)
                    .Include(d => d.DeviceEmployees)
                        .ThenInclude(de => de.Employee)
                            .ThenInclude(e => e.Person)
                    .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Unable to retrieve device with ID {id}.", ex);
            }
        }

        public async Task<bool> CreateDevice(Device device, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.Devices.AddAsync(device);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to create device.", ex);
            }
        }

        public async Task<DeviceType?> GetDeviceTypeByName(string name, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.DeviceTypes
                    .FirstOrDefaultAsync(dt => dt.Name == name, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to retrieve device type '{name}'.", ex);
            }
        }

        public async Task<bool> UpdateDevice(int id, Device updatedDevice, CancellationToken cancellationToken)
        {
            try
            {
                var existingDevice = await GetDeviceById(id,cancellationToken);

                if (existingDevice == null)
                    throw new KeyNotFoundException($"Device with ID {id} not found.");

                existingDevice.Name = updatedDevice.Name;
                existingDevice.IsEnabled = updatedDevice.IsEnabled;
                existingDevice.DeviceType = updatedDevice.DeviceType;
                existingDevice.AdditionalProperties = updatedDevice.AdditionalProperties;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to update device with ID {id}.", ex);
            }
        }

        public async Task<bool> DeleteDevice(int id, CancellationToken cancellationToken)
        {
            try
            {
                var device = await _context.Devices.FirstOrDefaultAsync(d => d.Id == id,cancellationToken);

                if (device == null)
                    throw new KeyNotFoundException($"Device with ID {id} not found.");

                _context.Devices.Remove(device);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to delete device with ID {id}.", ex);
            }
        }
    }
}
