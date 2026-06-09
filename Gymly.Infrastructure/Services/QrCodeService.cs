using Gymly.Application.Interfaces.Common;
using QRCoder;

namespace Gymly.Infrastructure.Services;

public class QrCodeService : IQrCodeService
{
    public string GeneratePngBase64(string payload, int pixelSize = 20)
    {
        using var qrGenerator = new QRCodeGenerator();
        var qrData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.M);
        using var qrCode = new PngByteQRCode(qrData);
        var qrBytes = qrCode.GetGraphic(pixelSize);
        return Convert.ToBase64String(qrBytes);
    }
}