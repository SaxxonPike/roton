namespace Roton.Infrastructure;

public interface IContextEngineSelector
{
    bool TryGetForWorldFileName(string filename, out Context context);
}