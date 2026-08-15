using System.Linq;
using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data.Impl;

namespace Roton.Test.Roton.Integration.Oop;

public class EndToEndTests(Context context) : OopTestFixture(context)
{
    [Test]
    public void TestCase_TownOfZztHouseOfBlues()
    {
        var bluesCodes = new[]
        {
            new[]
            {
                "@blue",
                "#clear f2",
                "#char 14",
                "#end",
                ":touch",
                "#play qgi+c",
                "#if not f1 all:restart",
                "#char 13",
                "#clear f1",
                "#set f2"
            },
            new[]
            {
                "@blue",
                "#clear f4",
                "#char 14",
                "#end",
                ":touch",
                "#play qci-a#",
                "#if not f3 all:restart",
                "#char 13",
                "#clear f3",
                "#set f4"
            },
            new[]
            {
                "@blue",
                "#char 14",
                "#end",
                ":touch",
                "#play h.c",
                "#if not f4 all:restart",
                "#char 13",
                "#clear f4",
                "#zap blue:touch",
                "#send jazzman:do"
            },
            new[]
            {
                "@blue",
                "#clear f1",
                "#char 14",
                "#end",
                ":touch",
                "#play icd#cd#",
                "#set f1",
                "#char 13",
                "#others:restart",
                "#end"
            },
            new[]
            {
                "@blue",
                "#clear f3",
                "#char 14",
                "#end",
                ":touch",
                "#play if#fd#",
                "#if not f2 all:restart",
                "#char 13",
                "#clear f2",
                "#set f3"
            }
        };

        var x = 1;
        var actors = bluesCodes.Select(code =>
        {
            var actorIndex = SpawnTo(x++, 1, ElementList.ObjectId);
            var actor = Actors[actorIndex];
            actor.Cycle = 1;
            SetActorCode(actorIndex, code);
            return actor;
        }).ToList();

        MovePlayerTo(4, 2);
        Type(AnsiKey.Up);
        StepAllKeys();

        actors[0].P1.Should().Be(14);
        actors[1].P1.Should().Be(14);
        actors[2].P1.Should().Be(14);
        actors[3].P1.Should().Be(13);
        actors[4].P1.Should().Be(14);
    }
}