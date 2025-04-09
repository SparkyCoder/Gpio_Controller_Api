using FakeItEasy;
using FluentAssertions;
using GpioController.Commands;
using GpioController.Commands.Request;
using GpioController.Commands.Results;
using GpioController.Factories;
using GpioController.Models;
using GpioController.Services;
using Microsoft.Extensions.Options;

namespace GpioControllerTests.Services;

public class GpioServiceTests
{
    private ICommandFactory commandFactory;
    private IOptions<FilterSettings> filterSettings;
    private ICommand<GpioInfoRequest, GpioInfoResult> command;

    public GpioService GetSystemUnderTest()
    {
        command = A.Fake<ICommand<GpioInfoRequest, GpioInfoResult>>();
        commandFactory = A.Fake<ICommandFactory>();
        filterSettings = A.Fake<IOptions<FilterSettings>>();

        return new GpioService(commandFactory, filterSettings);
    }

    [Fact]
    public void GetGpios_ShouldReturnResult_WhenFactoryAdCommandFound()
    {
        var sut = GetSystemUnderTest();
        
        A.CallTo(() => filterSettings.Value).Returns(new FilterSettings
        {
            AllowOnlyTheseChipsets = [],
            AllowOnlyTheseGpios = []
        });

        var expectedResult = new GpioInfoResult
        {
            Result = new List<Gpio>
            {
                new()
                {
                    Chipset = 1,
                    Id = 95,
                    Name = "Gpio 95"
                },
                new()
                {
                    Chipset = 1,
                    Id = 80,
                    Name = "Gpio 80"
                },
                new()
                {
                    Chipset = 1,
                    Id = 81,
                    Name = "Gpio 81"
                }
            }
        };

        A.CallTo(() => commandFactory.GetCommand<GpioInfoRequest, GpioInfoResult>()).Returns(command);

        A.CallTo(() => command.Execute(A<GpioInfoRequest>._)).Returns(expectedResult);

        var results = sut.GetGpios();
        
        results.Should().BeEquivalentTo(expectedResult.Result);


    }

    [Fact]
    public void OrderResultsByFilter_WhenFilterIsPresent_ShouldSortResults()
    {
        var sut = GetSystemUnderTest();
        
        A.CallTo(() => filterSettings.Value).Returns(new FilterSettings
        {
            AllowOnlyTheseChipsets = [1],
            AllowOnlyTheseGpios = [81,95,80,79,94,93]
        });

        var data = new List<Gpio>
        {
            new()
            {
                Chipset = 1,
                Id = 95,
                Name = "Gpio 95"
            },
            new()
            {
                Chipset = 1,
                Id = 80,
                Name = "Gpio 80"
            },
            new()
            {
                Chipset = 1,
                Id = 81,
                Name = "Gpio 81"
            }
        };
        
        var result = sut.OrderResultsByFilter(data).ToArray();

        result[0].Id.Should().Be(81);
        result[1].Id.Should().Be(95);
        result[2].Id.Should().Be(80);
    }
    
    [Fact]
    public void OrderResultsByFilter_WhenNoFilterIsPresent_ShouldStillReturnOriginalResults()
    {
        var sut = GetSystemUnderTest();
        
        A.CallTo(() => filterSettings.Value).Returns(new FilterSettings
        {
            AllowOnlyTheseChipsets = [],
            AllowOnlyTheseGpios = []
        });

        var data = new List<Gpio>
        {
            new()
            {
                Chipset = 1,
                Id = 95,
                Name = "Gpio 95"
            },
            new()
            {
                Chipset = 1,
                Id = 80,
                Name = "Gpio 80"
            },
            new()
            {
                Chipset = 1,
                Id = 81,
                Name = "Gpio 81"
            }
        };
        
        var result = sut.OrderResultsByFilter(data).ToArray();

        result[0].Id.Should().Be(95);
        result[1].Id.Should().Be(80);
        result[2].Id.Should().Be(81);
    }
}