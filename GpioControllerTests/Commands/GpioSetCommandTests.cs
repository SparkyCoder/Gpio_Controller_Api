using FakeItEasy;
using GpioController.Commands;
using GpioController.Commands.Request;
using GpioController.Commands.Results;
using GpioController.Parsers;
using GpioController.Services;

namespace GpioControllerTests.Commands;

public class GpioSetCommandTests
{
    private IParser<GpioSetResult>? parser;
    private ITerminalService? terminalService;
    
    private GpioSetCommand GetSystemUnderTest()
    {
        parser = A.Fake<IParser<GpioSetResult>>();
        terminalService = A.Fake<ITerminalService>();
        
        return new GpioSetCommand(parser, terminalService);
    }

    [Fact]
    public void Execute_ShouldRunTerminalCommandCorrectly_ForEachRepeatedNumber()
    {
        var sut = GetSystemUnderTest();

        var request = new GpioSetRequest
        {
            Chipset = 1,
            Gpio = 81,
            State = "Low",
            Options = new OptionalSettings
            {
                Milliseconds = 50,
                RepeatTimes = 2
            }
        };
        
        sut.Execute(request);

        A.CallTo(() => terminalService.RunCommand(A<string>.That.Matches(command => command.Contains("81=0"))))
            .MustHaveHappenedANumberOfTimesMatching(times => times == 2);
        
        A.CallTo(() => terminalService.RunCommand(A<string>.That.Matches(command => command.Contains("81=1"))))
            .MustHaveHappenedANumberOfTimesMatching(times => times == 2);
    }
    
    [Fact]
    public void Execute_ShouldRunTerminalCommandCorrectly_WhenRunningAnOddNumberOfTimes()
    {
        var sut = GetSystemUnderTest();

        var request = new GpioSetRequest
        {
            Chipset = 1,
            Gpio = 81,
            State = "Low",
            Options = new OptionalSettings
            {
                Milliseconds = 50,
                RepeatTimes = 3
            }
        };
        
        sut.Execute(request);

        A.CallTo(() => terminalService.RunCommand(A<string>.That.Matches(command => command.Contains("81=0"))))
            .MustHaveHappenedANumberOfTimesMatching(times => times == 3);
        
        A.CallTo(() => terminalService.RunCommand(A<string>.That.Matches(command => command.Contains("81=1"))))
            .MustHaveHappenedANumberOfTimesMatching(times => times == 3);
    }
    
    [Fact]
    public void Execute_ShouldRunTerminalCommandCorrectly_WithRequestWithoutAnyOptions()
    {
        var sut = GetSystemUnderTest();

        var request = new GpioSetRequest
        {
            Chipset = 1,
            Gpio = 81,
            State = "Low"
        };
        
        sut.Execute(request);

        A.CallTo(() => terminalService.RunCommand(A<string>._))
            .MustHaveHappenedANumberOfTimesMatching(times => times == 1);
    }
}