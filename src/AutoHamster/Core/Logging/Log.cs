using System;

public struct Log
{
    public DateTime TimeStamp;
    public LogType Type;
    public string Tag;
    public string Msg;

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
            return $"[{TimeStamp.ToString("hh:mm:ss")}] [{Tag}] {Msg}";
        }
        else
        {
            return $"[{Tag}] {Msg}";
        } 
    }
}
