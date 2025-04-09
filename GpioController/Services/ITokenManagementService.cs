using GpioController.Commands.Request;

namespace GpioController.Services;

public interface ITokenManagementService
{
    public void CancelAll();
    public CancellationToken CreateToken(GpioSetRequest request);
}