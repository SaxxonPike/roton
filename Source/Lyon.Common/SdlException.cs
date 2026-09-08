namespace Lyon.Common;

public sealed class SdlException(string message = "An SDL error occurred")
    : Exception($"{message}: {SDL_GetError()}");