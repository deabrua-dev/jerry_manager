using System;

namespace jerry_manager.Core.Exceptions;

public class ArchiveReadException : Exception
{
    public ArchiveReadException(string message) : base(message) {  }
}