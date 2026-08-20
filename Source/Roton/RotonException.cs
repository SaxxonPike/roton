using System;

namespace Roton;

public class RotonException(string message, Exception? innerException = null) 
    : Exception(message, innerException);