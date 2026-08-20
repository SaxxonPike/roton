using System.Linq;
using AwesomeAssertions;
using NUnit.Framework;

namespace Roton.Test.Roton.Integration.Oop;

public class LabelTests(Context context) : OopTestFixture(context)
{
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

        actors[0].Code.ToString().Should().Be(string.Join("\r",
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

        actors[0].Code.ToString().Should().Be(string.Join("\r",
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

        actors[0].Code.ToString().Should().Be(string.Join("\r",
            "@blue",
            "#zap green:label",
            ":label",
            "'label",
            "#end"
        ));
        actors[1].Code.ToString().Should().Be(string.Join("\r",
            "@blue",
            "'label",
            "'label",
            "#end"
        ));
        actors[2].Code.ToString().Should().Be(string.Join("\r",
            "@green",
            "#restore blue:label",
            "'label",
            ":label",
            "#end"
        ));
        actors[3].Code.ToString().Should().Be(string.Join("\r",
            "@green",
            ":label",
            ":label",
            "#end"
        ));
    }
}