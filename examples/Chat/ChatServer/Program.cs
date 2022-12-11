﻿//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Akka.NET Project">
//     Copyright (C) 2009-2022 Lightbend Inc. <http://www.lightbend.com>
//     Copyright (C) 2013-2022 .NET Foundation <https://github.com/akkadotnet/akka.net>
// </copyright>
//-----------------------------------------------------------------------

using Akka.Actor;
using Akka.Configuration;
using ChatMessages;

var config = ConfigurationFactory.ParseString("""
akka {
    actor {
        provider = "Akka.OpenTelemetry.Remote.OpenTelemetryRemoteActorRefProvider, Akka.OpenTelemetry"
    }
    remote {
        dot-netty.tcp {
            port = 8081
            hostname = 0.0.0.0
            public-hostname = localhost
        }
    }
}
""");

using var system = ActorSystem.Create("MyServer", config);
system.ActorOf(Props.Create(() => new ChatServerActor()), "ChatServer");

Console.ReadLine();


internal class ChatServerActor : ReceiveActor, ILogReceive
{
    private readonly HashSet<IActorRef> _clients = new();

    public ChatServerActor()
    {
        Receive<SayRequest>(message =>
        {
            var response = new SayResponse
            {
                Username = message.Username,
                Text = message.Text
            };
            foreach (var client in _clients) client.Tell(response, Self);
        });

        Receive<ConnectRequest>(message =>
        {
            _clients.Add(Sender);
            Sender.Tell(new ConnectResponse
            {
                Message = "Hello and welcome to Akka.NET chat example"
            }, Self);
        });

        Receive<NickRequest>(message =>
        {
            var response = new NickResponse
            {
                OldUsername = message.OldUsername,
                NewUsername = message.NewUsername
            };

            foreach (var client in _clients) client.Tell(response, Self);
        });
    }
}