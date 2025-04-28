using System;
using UnityEngine;

public class CheatBase
{
    protected string _commandID;
    protected string _commandDesc;
    protected string _commandFormat;

    public string commandID => _commandID;
    public string commandDesc => _commandDesc;
    public string commandFormat => _commandFormat;

    public CheatBase(string id, string desc, string format)
    {
        _commandID = id;
        _commandDesc = desc;
        _commandFormat = format;
    }
}

public class CheatCommand : CheatBase // Default
{
    private Action command;

    public CheatCommand(string id, string desc, string format, Action command) : base(id, desc, format)
    {
        this.command = command;
    }

    public void Invoke()
    {
        command?.Invoke();
    }
}

public class CheatCommand<T1> : CheatBase // Takes Type
{
    private Action<T1> command;

    public CheatCommand(string id, string desc, string format, Action<T1> command) : base(id, desc, format)
    {
        this.command = command;
    }

    public void Invoke(T1 value)
    {
        command?.Invoke(value);
    }
}