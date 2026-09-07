using System;
using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IErrorRaiser
{
    /// <remarks>
    /// RoZ: OopError
    /// </remarks>
    void RaiseError(ref OopContext context, ReadOnlySpan<char> error);
}