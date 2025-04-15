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
    private IOptions<MappingSettings> mappingSettings;
    private ICommand<GpioInfoRequest, GpioInfoResult> command;

    public GpioService GetSystemUnderTest()
    {
        command = A.Fake<ICommand<GpioInfoRequest, GpioInfoResult>>();
        commandFactory = A.Fake<ICommandFactory>();
        filterSettings = A.Fake<IOptions<FilterSettings>>();
        mappingSettings = A.Fake<IOptions<MappingSettings>>();

        return new GpioService(commandFactory, filterSettings, mappingSettings);
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
    
    [Fact]
    public void MapGpioNames_WhenCustomMapsAreDefined_MapsNamesCorrectly()
    {
        var sut = GetSystemUnderTest();
        
        A.CallTo(() => mappingSettings.Value).Returns(new MappingSettings
        {
           GpioNames =
           [
               new Map
               {
                   Id = 91,
                   Name = "VCC"
               },
               new Map
               {
                   Id = 92,
                   Name = "Common"
               },
               new Map
               {
                   Id = 81,
                   Name = "Zone 1"
               }
           ]
        });

        var data = new List<Gpio>
        {
            new()
            {
                Chipset = 1,
                Id = 91,
                Name = "Gpio 91"
            },
            new()
            {
                Chipset = 1,
                Id = 92,
                Name = "Gpio 92"
            },
            new()
            {
                Chipset = 1,
                Id = 81,
                Name = "Gpio 81"
            }
        };
        
        var result = sut.MapGpioNames(data).ToArray();

        result[0].Name.Should().Be("VCC");
        result[1].Name.Should().Be("Common");
        result[2].Name.Should().Be("Zone 1");
    }
    
    [Fact]
    public void MapGpioNames_WhenNoMapsAreDefined_DoesNotError()
    {
        var sut = GetSystemUnderTest();

        A.CallTo(() => mappingSettings.Value).Returns(new MappingSettings());

        var data = new List<Gpio>
        {
            new()
            {
                Chipset = 1,
                Id = 91,
                Name = "Gpio 91"
            },
            new()
            {
                Chipset = 1,
                Id = 92,
                Name = "Gpio 92"
            },
            new()
            {
                Chipset = 1,
                Id = 81,
                Name = "Gpio 81"
            }
        };
        
        var result = sut.MapGpioNames(data).ToArray();

        result.Should().BeEquivalentTo(data);
    }
}