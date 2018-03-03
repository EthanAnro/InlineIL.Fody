﻿using System.Diagnostics.CodeAnalysis;
using System.Reflection.Emit;
using InlineIL;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class BasicClass
{
    public static void Nop()
    {
        IL.Op(OpCodes.Nop);
    }

    public int MultiplyBy3(int value)
    {
        IL.Push(value);
        IL.Op(OpCodes.Dup);
        IL.Op(OpCodes.Dup);
        IL.Op(OpCodes.Add);
        IL.Op(OpCodes.Add);
        IL.Op(OpCodes.Ret);
        throw IL.Unreachable();
    }

    public void AddAssign(ref int a, int b)
    {
        IL.Push(ref a);
        IL.Push(a);
        IL.Push(b);
        IL.Op(OpCodes.Add);
        IL.Op(OpCodes.Stind_I4);
    }
}
