using System;
using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Data;
using Roton.Infrastructure;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Emulation;

public class ParserTests(Context context) : AllContextTestFixture(context)
{
    [Test]
    public void ReadWord_ShouldParseSimpleWord_AndSetState()
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];
        Span<char> oopWordBuffer = stackalloc char[byte.MaxValue];
        var instruction = new Word(0);
        ReadOnlySpan<char> code = "hello world";

        var result = Parser.ReadWord(code, ref instruction, buffer);

        result.ToString().Should().Be("HELLO");
        ((int)instruction).Should().Be(5);
        ((char)State.OopByte).Should().Be(' ');
        State.GetOopWord(oopWordBuffer).ToString().Should().Be("HELLO");
    }

    [Test]
    public void ReadWord_ShouldSkipLeadingSpaces_AndAllowColonsAndUnderscores()
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];
        Span<char> oopWordBuffer = stackalloc char[byte.MaxValue];
        var instruction = new Word(0);
        ReadOnlySpan<char> code = "   foo_bar:123 remainder";

        var result = Parser.ReadWord(code, ref instruction, buffer);

        result.ToString().Should().Be("FOO_BAR:123");
        ((int)instruction).Should().Be(14);
        ((char)State.OopByte).Should().Be(' ');
        State.GetOopWord(oopWordBuffer).ToString().Should().Be("FOO_BAR:123");
    }

    [Test]
    public void ReadWord_ShouldFailOnLeadingDigit_AndSetOopByte()
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];
        Span<char> oopWordBuffer = stackalloc char[byte.MaxValue];
        var instruction = new Word(0);
        ReadOnlySpan<char> code = "  99bottles";

        var result = Parser.ReadWord(code, ref instruction, buffer);

        result.IsEmpty.Should().BeTrue();
        ((int)instruction).Should().Be(2);
        ((char)State.OopByte).Should().Be('9');
        State.GetOopWord(oopWordBuffer).ToString().Should().Be(string.Empty);
    }

    [Test]
    public void ReadWord_ShouldHandleOnlySpaces_AndEndOfCode()
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];
        var instruction = new Word(0);
        ReadOnlySpan<char> code = "   ";

        var result = Parser.ReadWord(code, ref instruction, buffer);

        result.IsEmpty.Should().BeTrue();
        ((int)instruction).Should().Be(2);
        ((char)State.OopByte).Should().Be('\0');
    }

    [Test]
    public void ReadWord_ShouldHandleEmptyCode()
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];
        var instruction = new Word(0);
        ReadOnlySpan<char> code = "";

        var result = Parser.ReadWord(code, ref instruction, buffer);

        result.IsEmpty.Should().BeTrue();
        ((int)instruction).Should().Be(0);
        ((char)State.OopByte).Should().Be('\0');
    }

    [Test]
    public void ReadWord_ShouldHandleInstructionPastEnd()
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];
        var instruction = new Word(10);
        ReadOnlySpan<char> code = "abc";

        var result = Parser.ReadWord(code, ref instruction, buffer);

        result.IsEmpty.Should().BeTrue();
        ((int)instruction).Should().Be(9);
        ((char)State.OopByte).Should().Be('\0');
    }
}
