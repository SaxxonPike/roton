using System;

namespace Lyon;

public sealed class LyonException(string message, Exception? innerException = null) 
    : Exception(message, innerException);