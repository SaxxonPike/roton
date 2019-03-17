using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration
{
    public class OopIntegrationTestFixture : AllContextIntegrationTestFixture
    {
        public OopIntegrationTestFixture(Context context) : base(context)
        {
        }

        [SetUp]
        public void __Setup()
        {
            if (ElementList.ObjectId < 0)
                Assert.Inconclusive();
        }

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
        
        [Test]
        public void If_ShouldExecuteCurrentLine_WhenConditionIsMet()
        {
            var index = SpawnTo(1, 1, ElementList.ObjectId);
            var actor = Actors[index];
            actor.Cycle = 1;
            SetActorCode(index, 
                "#if blocked i set f1",
                "#set f2"
                );
            
            Step();

            World.Flags.Should().Contain("F1", "F2");
        }
        
        [Test]
        public void If_ShouldSkipToNextLine_WhenConditionIsNotMet()
        {
            var index = SpawnTo(1, 1, ElementList.ObjectId);
            var actor = Actors[index];
            actor.Cycle = 1;
            SetActorCode(index, 
                "#if not blocked i set f1",
                "#set f2"
            );
            
            Step();

            World.Flags.Should().Contain("F2");
        }

        [Test]
        public void ShortMovement_ShouldRunCommandsOnSameLine()
        {
            var index = SpawnTo(1, 1, ElementList.ObjectId);
            var actor = Actors[index];
            actor.Cycle = 1;
            SetActorCode(index, 
                "/i#set f2"
            );
            
            Step(2);

            World.Flags.Should().Contain("F2");
        }
        
        [Test]
        public void ShortMovement_ShouldRunCommandsOnSameLine_WhenPrecededByIf()
        {
            var index = SpawnTo(1, 1, ElementList.ObjectId);
            var actor = Actors[index];
            actor.Cycle = 1;
            SetActorCode(index,
                "#set f1",
                "#if f1 /i#set f2"
            );
            
            Step(2);

            World.Flags.Should().Contain("F2");
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
                    "#end"
                },
                new[]
                {
                    "@blue",
                    "'label",
                    "#end"
                },
                new[]
                {
                    "@green",
                    "#restore blue:label",
                    ":label",
                    "#end"
                },
                new[]
                {
                    "@green",
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
                "#end"
            ));
            actors[1].Code.ToStringValue().Should().Be(string.Join("\xD", 
                "@blue",
                ":label",
                "#end"
            ));
            actors[2].Code.ToStringValue().Should().Be(string.Join("\xD", 
                "@green",
                "#restore blue:label",
                "'label",
                "#end"
            ));
            actors[3].Code.ToStringValue().Should().Be(string.Join("\xD", 
                "@green",
                "'label",
                "#end"
            ));
        }
    }
}