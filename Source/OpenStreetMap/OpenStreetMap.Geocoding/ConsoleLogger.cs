using System;

namespace Proxoft.Maps.OpenStreetMap.Geocoding;

internal class ConsoleLogger(bool traceLog, bool logExceptions)
{
    private readonly bool _traceLog = traceLog;
    private readonly bool _logExceptions = logExceptions;

    public void LogMessage(string message)
    {
        if (!_traceLog)
        {
            return;
        }

        Console.WriteLine(message);
    }

    public void LogException(Exception exception, string message)
    {
        if (!_logExceptions)
        {
            return;
        }

        Console.WriteLine(message);
        Console.WriteLine(exception);
    }
}
