﻿using Akka.Actor;
using Asynkron.Akka;

var system = ActorSystem.Create("MySystem");
var obs = Asynkron.Akka.OpenTelemetry.OpenTelemetry.For(system);

var r = system.ActorOf<MyActor>();
r.Tell("Hello");

Console.ReadLine();


namespace Asynkron.Akka
{
    public class MyActor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            var x = Context;
        }
    }
}