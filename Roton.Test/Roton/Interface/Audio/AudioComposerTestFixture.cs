using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Moq;
using NUnit.Framework;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Original;
using Roton.Interface.Audio;

namespace Roton.Test.Roton.Interface.Audio
{
    [TestFixture]
    public class AudioComposerTestFixture
    {
        private Mock<IDrumBank> _drumBankMock;

        private IDrumBank CreateStandardDrumBank()
        {
            var memory = new Memory();
            return new OriginalDrumBank(memory);
        }

        [SetUp]
        public void SetUpTest()
        {
            _drumBankMock = new Mock<IDrumBank>();
        }

        [Test]
        [Explicit]
        public void RenderDrums()
        {
            // Arrange.
            var subject = new AudioComposer(
                CreateStandardDrumBank(),
                44100,
                80);

            // Act.
            var output = new List<byte>();
            for (var j = 0; j < 10; j++)
            {
                subject.PlayDrum(j);
                var samples =
                    subject.ComposeAudio()
                        .Take(11025)
                        .Select(i => i << 12)
                        .SelectMany(i => new[] {(byte) (i & 0xFF), (byte) ((i >> 8) & 0xFF)});
                output.AddRange(samples);
            }

            File.WriteAllBytes(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.bin"), output.ToArray());
        }
    }
}