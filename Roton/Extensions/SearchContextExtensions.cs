using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using Roton.Core;
using Roton.Emulation.Mapping;

namespace Roton.Extensions
{
    public static class SearchContextExtensions
    {
        public static IActorList GetActors(this ISearchContext context)
        {
            return context.Engine.Actors;
        }

        public static IBoard GetBoard(this ISearchContext context)
        {
            return context.Engine.Board;
        }

        public static int ReadByte(this IOopContext context)
        {
            return context.Engine.ReadActorCodeByte(context.Index, context);
        }

        public static IFlagList GetFlags(this ISearchContext context)
        {
            return context.Engine.World.Flags;
        }

        public static int ReadNumber(this IOopContext context)
        {
            return context.Engine.ReadActorCodeNumber(context.Index, context);
        }

        public static string ReadWord(this IOopContext context)
        {
            return context.Engine.ReadActorCodeWord(context.Index, context);
        }

        public static IWorld GetWorld(this ISearchContext context)
        {
            return context.Engine.World;
        }

        public static void SetByte(this ISearchContext context, int value)
        {
            context.Engine.State.OopByte = value;
        }

        public static void SetNumber(this ISearchContext context, int value)
        {
            context.Engine.State.OopNumber = value;
        }

        public static int GetByte(this ISearchContext context)
        {
            return context.Engine.State.OopByte;
        }

        public static int GetNumber(this ISearchContext context)
        {
            return context.Engine.State.OopNumber;
        }
    }
}
