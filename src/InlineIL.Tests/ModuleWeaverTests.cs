﻿using System;
using System.Linq;
using Fody;
using InlineIL.Fody;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Xunit;

#pragma warning disable 618

namespace InlineIL.Tests
{
    public class ModuleWeaverTests : IDisposable
    {
        private static readonly TestResult _testResult;
        private static readonly AssemblyDefinition _processedAsembly;

        static ModuleWeaverTests()
        {
            var weavingTask = new ModuleWeaver();
            _testResult = weavingTask.ExecuteTestRun("InlineIL.Tests.AssemblyToProcess.dll");
            _processedAsembly = AssemblyDefinition.ReadAssembly(_testResult.AssemblyPath);
        }

        public void Dispose()
        {
            _processedAsembly.Dispose();
        }

        [Fact]
        public void should_process_assembly()
        {
            var method = _processedAsembly.Modules.Single().GetType("BasicClass").Methods.Single(m => m.Name == "Nop");

            var instructionCount = method.Body.Instructions.Count;
            Assert.Equal(OpCodes.Ret, method.Body.Instructions.Last().OpCode);
            Assert.All(method.Body.Instructions.Select(i => i.OpCode).Take(instructionCount - 1), op => Assert.Equal(OpCodes.Nop, op));
        }

        [Fact]
        public void should_push_value()
        {
            var result = (int)_testResult.GetInstance("BasicClass").MultiplyBy3(42);

            Assert.Equal(42 * 3, result);
        }

        [Fact]
        public void should_push_value_by_ref()
        {
            var a = 42;
            _testResult.GetInstance("BasicClass").AddAssign(ref a, 8);
            Assert.Equal(50, a);
        }
    }
}
