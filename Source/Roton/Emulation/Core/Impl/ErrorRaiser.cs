using System;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class ErrorRaiser(
    IMessenger messenger,
    IFacts facts,
    IAlerts alerts,
    ISoundPlayer soundPlayer,
    ITracer tracer,
    IActorList actors,
    ISounds sounds)
    : IErrorRaiser
{
    public void RaiseError(ref OopContext context, ReadOnlySpan<char> error)
    {
        messenger.SetMessage(facts.LongMessageDuration, alerts.ErrorMessage(error));
        soundPlayer.PlaySound(5, sounds.Error);
        tracer.TraceError(ref context, error);
        actors[context.Index].Instruction = -1;
    }
}