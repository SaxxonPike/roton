namespace Roton.Emulation.Core;

public interface IActorNotifier
{
    /// <remarks>
    /// RoZ: OopSend (label taken notification)
    /// </remarks>
    void NotifyLabelTaken(int index);
}