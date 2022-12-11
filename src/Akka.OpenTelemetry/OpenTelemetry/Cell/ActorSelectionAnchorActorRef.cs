using Akka.Actor;
using Akka.Dispatch.SysMsg;
using Akka.Util;

namespace Akka.OpenTelemetry.Cell;

public class ActorSelectionAnchorActorRef : IInternalActorRef
{
    private readonly IInternalActorRef _inner;
    public ActorSelectionAnchorActorRef(IInternalActorRef inner) => _inner = inner;

    public void Tell(object message, IActorRef sender)
    {
        //if message is an actorselection message, unwrap it, extract current headers, and rebake a new one
        if (message is ActorSelectionMessage asl)
        {
            var m = asl.Message;
            var envelope = TraceTell.ExtractHeaders(m);
            var a = new ActorSelectionMessage(envelope, asl.Elements, asl.WildCardFanOut);
            _inner.Tell(a, sender);
            return;
        }

        _inner.Tell(message, sender);
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
    public void SendSystemMessage(ISystemMessage message, IActorRef sender) => _inner.SendSystemMessage(message, sender);
    public void SendSystemMessage(ISystemMessage message) => _inner.SendSystemMessage(message);
    public IInternalActorRef Parent => _inner.Parent;
    public IActorRefProvider Provider => _inner.Provider;
    public bool IsTerminated => _inner.IsTerminated;
}