namespace src.DeviceManager.Exceptions;

public class InvalidDeviceTypeException : Exception
{
    public InvalidDeviceTypeException() : base("Invalid device type") { }
}