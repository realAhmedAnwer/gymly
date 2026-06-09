namespace Gymly.Application.Interfaces.Common;

public interface IQrCodeService
{
    string GeneratePngBase64(string payload, int pixelSize = 20);
}