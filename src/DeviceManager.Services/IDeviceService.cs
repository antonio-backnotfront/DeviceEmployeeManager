﻿using System.Text.Json.Nodes;
using src.DeviceManager.Models;
using src.DTO;

namespace src.DeviceManager.Services;

public interface IDeviceService
{
    public Task<List<GetDevicesDto>> GetAllDevices(CancellationToken cancellationToken);
    
    public Task<GetDeviceDto?> GetDeviceById(int id, CancellationToken cancellationToken);
    
    public Task<bool> CreateDevice(CreateDeviceDto createDeviceDto, CancellationToken cancellationToken);
    
    public Task<bool> UpdateDevice(int id, UpdateDeviceDto updateDeviceDto, CancellationToken cancellationToken);
    
    public Task<bool> DeleteDevice(int id, CancellationToken cancellationToken);
}