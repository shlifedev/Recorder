using System;
using System.Collections.Generic;

public static class Logger
{
 
    public static Action<Log> onLogged;
    public static List<Log> logs = new List<Log>();
    public static void Log(string tag, string msg)
    {
        Log log = new Log(LogType.Debug, tag, msg);
        logs.Add(log);
        Console.WriteLine(log.GetMsgWithTag());
        onLogged?.Invoke(log);
    }
    public static void Warning(string tag, string msg)
    {
        Log log = new Log(LogType.Warning, tag, msg);
        logs.Add(log);
        Console.WriteLine(log.GetMsgWithTag());
        onLogged?.Invoke(log);
    }
    public static void Error(string tag, string msg)
    {
        Log log = new Log(LogType.Error, tag, msg);
        logs.Add(log);
        Console.WriteLine(log.GetMsgWithTag());
        onLogged?.Invoke(log);
    }


    public static void Log(string msg)
    {
        Log log = new Log(LogType.Debug, "Log", msg);
        logs.Add(log);
        Console.WriteLine(log.GetMsgWithTag());
        onLogged?.Invoke(log);
    }
    public static void Warning(string msg)
    {
        Log log = new Log(LogType.Warning, "Warning", msg);
        logs.Add(log);
        Console.WriteLine(log.GetMsgWithTag());
        onLogged?.Invoke(log);
    }
    public static void Error(string msg)
    {
        Log log = new Log(LogType.Error, "Error", msg);
        logs.Add(log);
        Console.WriteLine(log.GetMsgWithTag());
        onLogged?.Invoke(log);
    } 

    public static void Log(this object tag, string msg)
    {
        Log log = new Log(LogType.Debug, tag.GetType().Name, msg);
        logs.Add(log);
        Console.WriteLine(log.GetMsgWithTag());
        onLogged?.Invoke(log);
    }
    public static void Warning(this object tag, string msg)
    {
        Log log = new Log(LogType.Warning, tag.GetType().Name, msg);
        logs.Add(log);
        Console.WriteLine(log.GetMsgWithTag());
        onLogged?.Invoke(log);
    }
    public static void Error(this object tag, string msg)
    {
        Log log = new Log(LogType.Error, tag.GetType().Name, msg);
        logs.Add(log);
        Console.WriteLine(log.GetMsgWithTag());
        onLogged?.Invoke(log);
    }
}
