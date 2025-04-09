using FakeItEasy;
using GpioController.Commands;
using GpioController.Commands.Request;
using GpioController.Commands.Results;
using GpioController.Factories;
using GpioController.Models;
using GpioController.Services;

namespace GpioControllerTests.Services;

public class StateServiceTests
{
    private ICommandFactory commandFactory;
    private IGpioService gpioService;
    private ITokenManagementService tokenManagementService;
    
    private StateService GetSystemUnderTest()
    {
        commandFactory = A.Fake<ICommandFactory>();
        gpioService = A.Fake<IGpioService>();
        tokenManagementService = A.Fake<ITokenManagementService>();
        
        return new StateService(commandFactory, gpioService, tokenManagementService);
    }

    [Fact]
    public void UpdateMultipleStates_WhenItHasMultipleRequests_CancellingOneTaskShouldCancelAll()
    {
        var sut = GetSystemUnderTest();

        var requests = new List<GpioSetRequest>
        {
            new GpioSetRequest
            {
                Chipset = 1,
                Gpios = [91, 92, 81],
                State = "0",
                Options = new OptionalSettings
                {
                    Milliseconds = 1000,
                    RepeatTimes = 1
                }
            },
            new GpioSetRequest
            {
                Chipset = 1,
                Gpios = [91, 92, 95],
                State = "0",
                Options = new OptionalSettings
                {
                    Milliseconds = 1000,
                    RepeatTimes = 1
                }
            },
            new GpioSetRequest
            {
                Chipset = 1,
                Gpios = [91, 92, 80],
                State = "0",
                Options = new OptionalSettings
                {
                    Milliseconds = 1000,
                    RepeatTimes = 1
                }
            }
        };

        var cancellationSource = new CancellationTokenSource();
        var cancellationToken = cancellationSource.Token;

        A.CallTo(() => tokenManagementService.CreateToken(requests[0])).Returns(cancellationToken);
        A.CallTo(() => gpioService.GetGpios()).Returns(new List<Gpio>
        {
            new()
            {
                Chipset = 1,
                Name = "GPIO 1",
                Id = 91
            },
            new()
            {
                Chipset = 1,
                Name = "GPIO 2",
                Id = 92
            },
            new()
            {
                Chipset = 1,
                Name = "GPIO 3",
                Id = 95
            },
            new()
            {
                Chipset = 1,
                Name = "GPIO 4",
                Id = 81
            },
            new()
            {
                Chipset = 1,
                Name = "GPIO 5",
                Id = 80
            }
        });

        var command = A.Fake<ICommand<GpioSetRequest, GpioSetResult>>();

        A.CallTo(() => commandFactory.GetCommand<GpioSetRequest, GpioSetResult>()).Returns(command);
        A.CallTo(() => command.Execute(A<GpioSetRequest>._))
            .Invokes((GpioSetRequest request, GpioSetResult result) =>
            {
                Thread.Sleep(request.Options?.Milliseconds ?? 0);
            });
        
        sut.UpdateMultipleStates(requests);
        
        Thread.Sleep(50);
        
        cancellationSource.Cancel();
        
        Thread.Sleep(3500);

        A.CallTo(() => command.Execute(A<GpioSetRequest>._)).MustHaveHappenedOnceExactly();
    }
}