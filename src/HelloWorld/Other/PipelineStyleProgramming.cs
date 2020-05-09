using System;

namespace ZHello.Other
{
    public interface IPipeLineStep<Input, Output>
    {
        Output Process(Input input);
    }

    public class StringToInt : IPipeLineStep<string, int>
    {
        public int Process(string input)
        {
            return int.Parse(input);
        }
    }

    public class IntToFloat : IPipeLineStep<int, float>
    {
        public float Process(int input)
        {
            return input;
        }
    }

    public class FloatToString : IPipeLineStep<float, string>
    {
        public string Process(float input)
        {
            return input.ToString("0.00");
        }
    }

    public static class PipeLineStyleExtensions
    {
        public static Output Step<Input, Output>(this Input input, IPipeLineStep<Input, Output> step)
        {
            return step.Process(input);
        }
    }

    public static class PipelineStyleProgramming
    {
        public static void Main()
        {
            var str = "123";
            var outstr = str.Step(new StringToInt()).Step(new IntToFloat());
        }
    }

    //DI
    public abstract class Pipeline<Input, Output>
    {
        public Func<Input, Output> PipelineSteps { get; set; }

        public Output Process(Input input)
        {
            return PipelineSteps(input);
        }
    }

    public class TPipeline : Pipeline<string, float>
    {
        public TPipeline()
        {
            PipelineSteps = input => input.Step(new StringToInt()).Step(new IntToFloat());
        }
    }

    public class DIMain
    {
        public void Main()
        {
            var str = "123";
        }
    }
}