using System;

public struct Log
{
    public DateTime TimeStamp { get; }
    public LogType @Type { get; }
    public string Tag { get; }
    public string Msg { get; }

    public Log(LogType type, string tag, string msg)
    {
        TimeStamp = DateTime.Now;
        Type = type;
        Tag = tag;
        Msg = msg;
    }
    public string GetMsgWithoutTag(bool datetime = true)
    {
        if (datetime)
        {
            return $"[{TimeStamp.ToString("hh:mm:ss")}] {Msg}";
        }
        else
        {
            return $"{Msg}";
        }
    }
    public string GetMsgWithTag(bool datetime = true)
    {
        if (datetime)
        {
            return $"[{TimeStamp:hh:mm:ss}] [{Tag}] {Msg}";
        }
        else
        {
            return $"[{Tag}] {Msg}";
        } 
    }
}
