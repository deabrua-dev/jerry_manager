using System;

namespace jerry_manager.Core.Exceptions;

public class FileReadException : Exception
{
    public FileReadException(string message) : base(message) {  }
}