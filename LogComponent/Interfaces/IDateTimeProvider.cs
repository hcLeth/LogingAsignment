using System;

namespace LogComponent.Interfaces;

public interface IDateTimeProvider
{
    DateTime GetCurrentTime();
}