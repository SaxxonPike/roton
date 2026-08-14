using System.Linq;
using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;

namespace Roton.Test.Roton.Integration.Oop;

public class LabelTests : OopTestFixture
{
    public LabelTests(Context context) : base(context)
    {
    }

    [Test]
    public void ZappingOwnLabels_ShouldProduceExpectedCode()
    {
        var programs = new[]
        {
            new[]
            {
                "@yellow",
                "#zap label",
                ":label",
                ":label",
                "#end"
            }
        };

        var x = 1;
        var actors = programs.Select(code =>
        {
            var actorIndex = SpawnTo(x++, 1, ElementList.ObjectId);
            var actor = Actors[actorIndex];
            actor.Cycle = 1;
            SetActorCode(actorIndex, code);
            return actor;
        }).ToList();

        Step(2);

        actors[0].Code.ToStringValue().Should().Be(string.Join("\xD",
            "@yellow",
            "#zap label",
            "'label",
            ":label",
            "#end"
        ));
    }

    [Test]
    public void RestoringOwnLabels_ShouldProduceExpectedCode()
    {
        var programs = new[]
        {
            new[]
            {
                "@yellow",
                "#restore label",
                "'label",
                "'label",
                "#end"
            }
        };

        var x = 1;
        var actors = programs.Select(code =>
        {
            var actorIndex = SpawnTo(x++, 1, ElementList.ObjectId);
            var actor = Actors[actorIndex];
            actor.Cycle = 1;
            SetActorCode(actorIndex, code);
            return actor;
        }).ToList();

        Step(2);

        actors[0].Code.ToStringValue().Should().Be(string.Join("\xD",
            "@yellow",
            "#restore label",
            ":label",
            ":label",
            "#end"
        ));
    }

    [Test]
    public void ZappingRemoteLabels_ShouldProduceExpectedCode()
    {
        var programs = new[]
        {
            new[]
            {
                "@blue",
                "#zap green:label",
                "'label",
                "'label",
                "#end"
            },
            new[]
            {
                "@blue",
                "'label",
                "'label",
                "#end"
            },
            new[]
            {
                "@green",
                "#restore blue:label",
                ":label",
                ":label",
                "#end"
            },
            new[]
            {
                "@green",
                ":label",
                ":label",
                "#end"
            }
        };

        var x = 1;
        var actors = programs.Select(code =>
        {
            var actorIndex = SpawnTo(x++, 1, ElementList.ObjectId);
            var actor = Actors[actorIndex];
            actor.Cycle = 1;
            SetActorCode(actorIndex, code);
            return actor;
        }).ToList();

        Step(2);

        actors[0].Code.ToStringValue().Should().Be(string.Join("\xD",
            "@blue",
            "#zap green:label",
            ":label",
            "'label",
            "#end"
        ));
        actors[1].Code.ToStringValue().Should().Be(string.Join("\xD",
            "@blue",
            "'label",
            "'label",
            "#end"
        ));
        actors[2].Code.ToStringValue().Should().Be(string.Join("\xD",
            "@green",
            "#restore blue:label",
            "'label",
            ":label",
            "#end"
        ));
        actors[3].Code.ToStringValue().Should().Be(string.Join("\xD",
            "@green",
            ":label",
            ":label",
            "#end"
        ));
    }
}