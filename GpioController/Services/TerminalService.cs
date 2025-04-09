using System.Diagnostics;

namespace GpioController.Services;

public class TerminalService(ILogger<TerminalService> logger) : ITerminalService
{
    public string RunCommand(string command)
    {
        var process = CreateProcess(command);
        process.Start();
        logger.LogWarning($"Command Run: {command}");
        return ReadOutput(process);
    }

    private static string ReadOutput(Process process)
    {
        return process.StandardOutput.ReadToEnd();

    }

    private static Process CreateProcess(string command)
    {
        return new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"{command}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };
    }
}
