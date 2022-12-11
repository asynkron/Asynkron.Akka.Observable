using Akka.Actor;
using Akka.Dispatch.SysMsg;
using Akka.Remote;
using Akka.Util;

namespace Akka.OpenTelemetry.Remote;

public class OpenTelemetryRemoteActorRef : IInternalActorRef, IRemoteRef
{
    private readonly IInternalActorRef _inner;
    public OpenTelemetryRemoteActorRef(IInternalActorRef inner) => _inner = inner;

    public void Tell(object message, IActorRef sender)
    {
        var envelope = TraceTell.ExtractHeaders(message);
        _inner.Tell(envelope, sender);
    }

    public bool Equals(IActorRef? other) => _inner.Equals(other);
    public int CompareTo(IActorRef? other) => _inner.CompareTo(other);
    public ISurrogate ToSurrogate(ActorSystem system) => _inner.ToSurrogate(system);
    public int CompareTo(object? obj) => _inner.CompareTo(obj);
    public ActorPath Path => _inner.Path;
    public bool IsLocal  => _inner.IsLocal;
    public IActorRef GetChild(IReadOnlyList<string> name) => _inner.GetChild(name);
    public void Resume(Exception causedByFailure = null) => _inner.Resume(causedByFailure);
    public void Start() => _inner.Start();
    public void Stop() => _inner.Stop();
    public void Restart(Exception cause) => _inner.Restart(cause);
    public void Suspend() => _inner.Suspend();
    public void SendSystemMessage(ISystemMessage message) => _inner.SendSystemMessage(message);
    public IInternalActorRef Parent => _inner.Parent;
    public IActorRefProvider Provider => _inner.Provider;
#pragma warning disable CS0618
    public void SendSystemMessage(ISystemMessage message, IActorRef sender) => _inner.SendSystemMessage(message, sender);
    public bool IsTerminated => _inner.IsTerminated;
#pragma warning restore CS0618
}