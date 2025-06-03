using System.Text.Json;
using src.DeviceManager.Exceptions;
// using src.DeviceManager.API;

using src.DeviceManager.Models;
using src.DeviceManager.Services;
using src.DTO;

namespace src.DeviceManager.Services;

public class DeviceService : IDeviceService
{
    private readonly IDeviceRepository _deviceRepository;

    public DeviceService(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    public async Task<List<GetDevicesDto>> GetAllDevices(CancellationToken cancellationToken)
    {
        var devices = await _deviceRepository.GetAllDevices(cancellationToken);

        return devices.Select(device => new GetDevicesDto
        {
            Id = device.Id,
            Name = device.Name
        }).ToList();
    }

    public async Task<GetDeviceDto?> GetDeviceById(int id, CancellationToken cancellationToken)
    {
        var device = await _deviceRepository.GetDeviceById(id, cancellationToken);
        if (device is null) return null;

        var dto = new GetDeviceDto
        {
            Name = device.Name,
            DeviceType = device.DeviceType.Name,
            AdditionalProperties = JsonDocument.Parse(device.AdditionalProperties ?? "").RootElement
        };

        var activeAssignment = device.DeviceEmployees.FirstOrDefault(e => e.ReturnDate == null);
        if (activeAssignment != null)
        {
            var person = activeAssignment.Employee.Person;
            dto.CurrentEmployee = new GetEmployeesDto(
                activeAssignment.Id,
                $"{person.FirstName} {person.MiddleName} {person.LastName}"
            );
        }

        return dto;
    }

    public async Task<bool> CreateDevice(CreateDeviceDto dto, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(dto.DeviceType))
            throw new ArgumentException("Invalid device type");

        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Invalid device name");

        var deviceType = await _deviceRepository.GetDeviceTypeByName(dto.DeviceType, cancellationToken)
                         ?? throw new InvalidDeviceTypeException();

        var device = new Device
        {
            Name = dto.Name,
            DeviceType = deviceType,
            IsEnabled = dto.IsEnabled,
            AdditionalProperties = dto.AdditionalProperties.GetRawText()
        };

        await _deviceRepository.CreateDevice(device, cancellationToken);
        dto.Id = device.Id;
        return true;
    }

    public async Task<bool> UpdateDevice(int id, UpdateDeviceDto dto, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(dto.DeviceType))
            throw new ArgumentException("Invalid device type");

        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Invalid device name");

        var existingDevice = await _deviceRepository.GetDeviceById(id, cancellationToken)
                              ?? throw new KeyNotFoundException("Device not found");

        var deviceType = await _deviceRepository.GetDeviceTypeByName(dto.DeviceType, cancellationToken)
                         ?? throw new ArgumentException("Invalid device type");

        var updatedDevice = new Device
        {
            Name = dto.Name,
            IsEnabled = dto.IsEnabled,
            DeviceType = deviceType,
            AdditionalProperties = dto.AdditionalProperties ?? ""
        };

        return await _deviceRepository.UpdateDevice(id, updatedDevice, cancellationToken);
    }

    public async Task<bool> DeleteDevice(int id, CancellationToken cancellationToken)
    {
        var device = await _deviceRepository.GetDeviceById(id, cancellationToken)
                     ?? throw new KeyNotFoundException("Device not found");

        return await _deviceRepository.DeleteDevice(id, cancellationToken);
    }
}
