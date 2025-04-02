using FluentAssertions;
using GpioController.Parsers;

namespace GpioControllerTests.Parsers;

public class InforParserTests
{
    private static InfoParser GetSystemUnderTest()
    {
        return new InfoParser();
    }
    
    [Fact]
    public void Parse_ShouldParseExampleTerminalOutput_WithGoodData()
    {
        var sut = GetSystemUnderTest();

        var terminalOutput = """
            gpiochip0 - 11 lines:
            	line   0:    "UART TX"       unused   input  active-high 
            	line   1:    "UART RX"       unused   input  active-high 
            	line   2:   "Blue LED"       "blue"  output  active-high [used]
            	line   3: "SDCard Voltage Switch" "VCC_CARD" output active-high [used]
            	line   4: "7J1 Header Pin5" unused input active-high 
            	line   5: "7J1 Header Pin3" unused input active-high 
            	line   6: "7J1 Header Pin12" unused input active-high 
            	line   7:      "IR In"       unused   input  active-high 
            	line   8: "9J3 Switch HDMI CEC/7J1 Header " unused input active-high 
            	line   9: "7J1 Header Pin13" unused input active-high 
            	line  10: "7J1 Header Pin15" unused output active-high 
            gpiochip1 - 100 lines:
            	line  14: "Eth Link LED" unused input active-high 
            	line  15: "Eth Activity LED" unused input active-high 
            	line  16:   "HDMI HPD"       unused   input  active-high 
            	line  17:   "HDMI SDA"       unused   input  active-high 
            	line  18:   "HDMI SCL"       unused   input  active-high 
            	line  19: "HDMI_5V_EN" "regulator-hdmi-5v" output active-high [used]
            	line  20: "9J1 Header Pin2" unused input active-high 
            	line  21: "Analog Audio Mute" "enable" output active-high [used]
            	line  22: "2J3 Header Pin6" unused input active-high 
            	line  23: "2J3 Header Pin5" unused input active-high 
            	line  24: "2J3 Header Pin4" unused input active-high 
            	line  25: "2J3 Header Pin3" unused input active-high 
            	line  26:    "eMMC D0"       unused   input  active-high 
            	line  27:    "eMMC D1"       unused   input  active-high 
            	line  28:    "eMMC D2"       unused   input  active-high 
            	line  29:    "eMMC D3"       unused   input  active-high 
            """;
        
        var result = sut.Parse(terminalOutput).Result;
        result.Count().Should().Be(27);
        result.First(gpio => gpio.Id == 20).Chipset.Should().Be(1);
        result.First(gpio => gpio.Id == 20).Name.Should().Be("9J1 Header Pin2");
    }
}