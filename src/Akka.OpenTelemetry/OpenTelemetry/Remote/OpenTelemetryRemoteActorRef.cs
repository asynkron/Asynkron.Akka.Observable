using Akka.Actor;
using Akka.Remote;

namespace Akka.OpenTelemetry.Remote;

public class OpenTelemetryRemoteActorRef : RemoteActorRef
{
    protected override void TellInternal(object message, IActorRef sender)
    {
        var envelope = TraceTell.ExtractHeaders(message);
        base.TellInternal(envelope,sender);
    }

    public OpenTelemetryRemoteActorRef(RemoteTransport remote, Address localAddressToUse, ActorPath path, IInternalActorRef parent, Props props, Deploy deploy) : base(remote, localAddressToUse, path, parent, props, deploy)
    {
    }
}