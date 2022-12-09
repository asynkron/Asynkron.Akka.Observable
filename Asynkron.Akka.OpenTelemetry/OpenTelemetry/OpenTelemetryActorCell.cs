using System.Diagnostics;
using Akka.Actor;
using Akka.Actor.Internal;
using Akka.Dispatch;
using Asynkron.Akka.Decorators;

namespace Asynkron.Akka.OpenTelemetry;

public class OpenTelemetryActorCell : DecoratorActorCell
{
    public OpenTelemetryActorCell(ActorSystemImpl system, IInternalActorRef self, Props props,
        MessageDispatcher dispatcher, IInternalActorRef parent) : base(system, self, props, dispatcher, parent)
    {
    }

    protected override void ReceiveMessage(object message)
    {
        //non augmented message, just pass it on
        if (message is not MessageEnvelope envelope)
        {
            base.ReceiveMessage(message);
            return;
        }

        var propagationContext = envelope.Headers.ExtractPropagationContext();

        using var activity =
            OpenTelemetryHelpers.BuildStartedActivity(propagationContext.ActivityContext, "",
                nameof(ReceiveMessage),
                message, ReceiveActivitySetup);

        base.ReceiveMessage(envelope.Message);
    }

    void ReceiveActivitySetup(Activity activity, object message)
    {

    }
}