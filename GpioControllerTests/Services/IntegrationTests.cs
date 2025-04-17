using System.Text.Json;
using FakeItEasy;
using FluentAssertions;
using GpioController.Commands;
using GpioController.Commands.Request;
using GpioController.Commands.Results;
using GpioController.Factories;
using GpioController.Parsers;
using GpioController.Services;
using Microsoft.Extensions.Logging;

namespace GpioControllerTests.Services;

public class IntegrationTests
{
    [Fact]
    public void UpdateMultipleStateionTestShouldShowCorrectPinsTurnOff()
    {

        var commandFactory = A.Fake<ICommandFactory>();
        var gpioService = A.Fake<IGpioService>();
        var logger = A.Fake<ILogger<TokenManagementService>>();
        var tokenManagementService = new TokenManagementService(commandFactory, logger);
        var stateService = new StateService(commandFactory, gpioService, tokenManagementService);
        var parser = A.Fake<IParser<GpioSetResult>>();
        var terminalService = A.Fake<ITerminalService>();

        var json = """
            [
              {"Gpios":[91,92,81],"Chipset":1,"State":"Low","Options":{"Milliseconds":1000,"RepeatTimes":1}},
              {"Gpios":[91,92,95],"Chipset":1,"State":"Low","Options":{"Milliseconds":1000,"RepeatTimes":1}},
              {"Gpios":[91,92,80],"Chipset":1,"State":"Low","Options":{"Milliseconds":1000,"RepeatTimes":1}}
            ]
            """;
        
        var requests = JsonSerializer.Deserialize<List<GpioSetRequest>>(json);

        stateService.ValidateUpdateMultipleStates = (requests) => { };

        var command = new GpioSetCommand(parser, terminalService);
        A.CallTo(() => commandFactory.GetCommand<GpioSetRequest, GpioSetResult>()).ReturnsLazily(() => command);
            
        stateService.UpdateMultipleStates(requests);
        
        Thread.Sleep(2500);
        
        tokenManagementService.CancelAll();
        
        var allTerminalCalls = Fake.GetCalls(terminalService)
            .Where(call => call.Method.Name == nameof(terminalService.RunCommand))
            .Select(call => call.Arguments[0] as string)
            .ToList();

        allTerminalCalls[0].Should().Be("gpioset 1 91=0;gpioset 1 92=0;gpioset 1 81=0");
        allTerminalCalls[1].Should().Be("gpioset 1 91=1;gpioset 1 92=1;gpioset 1 81=1");
        allTerminalCalls[2].Should().Be("gpioset 1 91=0;gpioset 1 92=0;gpioset 1 95=0");
        allTerminalCalls[3].Should().Be("gpioset 1 91=1;gpioset 1 92=1;gpioset 1 95=1");
        allTerminalCalls[4].Should().Be("gpioset 1 91=0;gpioset 1 92=0;gpioset 1 80=0");
        allTerminalCalls[5].Should().Be("gpioset 1 91=1;gpioset 1 92=1;gpioset 1 80=1");
    }

}