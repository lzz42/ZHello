using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace ZHello.Other
{
    /*
     管道式编程
     https://www.cnblogs.com/hippieZhou/p/11174644.html
     */

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

    public class DateTimeToString : IPipeLineStep<DateTime, string>
    {
        public string Process(DateTime input)
        {
            return input.ToString("yyyy-MM-dd hh:mm:ss:fff");
        }
    }

    public static class PipeLineStyleExtensions
    {
        /// <summary>
        /// 定义泛型扩展方法
        /// </summary>
        /// <typeparam name="Input"></typeparam>
        /// <typeparam name="Output"></typeparam>
        /// <param name="input"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static Output Step<Input, Output>(this Input input, IPipeLineStep<Input, Output> step)
        {
            System.Diagnostics.Trace.WriteLine("{0} Processing.", step.GetType().Name);
            return step.Process(input);
        }

        public static Output OptionStep<Input, Output>(this Input input, IPipeLineStep<Input, Output> step, Func<Input, bool> choice)
            where Input : Output
        {
            System.Diagnostics.Trace.WriteLine("{0} Processing.", step.GetType().Name);
            return choice(input) ? step.Process(input) : input;
        }
    }

    /*
    DI 依赖注入
    1.定义抽象类 封装管道
     */

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="Input"></typeparam>
    /// <typeparam name="Output"></typeparam>
    public abstract class Pipeline<Input, Output>
    {
        public Func<Input, Output> PipelineSteps { get; set; }

        public Output Process(Input input)
        {
            if (PipelineSteps != null)
                return PipelineSteps(input);
            else
                return default(Output);
        }
    }

    public class TPipeline : Pipeline<string, float>
    {
        public TPipeline()
        {
            PipelineSteps = new Func<string, float>((s) =>
             {
                 return s.Step(new StringToInt()).Step(new IntToFloat());
             });
        }
    }

    //条件式组装

    public interface IOptionStep<Input, Output> : IPipeLineStep<Input, Output>
        where Input : Output
    {
        bool Choice(Input input);
    }

    public class OptionalStep<Input, Output> : IOptionStep<Input, Output>
        where Input : Output
    {
        private readonly IPipeLineStep<Input, Output> Step;

        private readonly Func<Input, bool> IChoice;

        public OptionalStep(IPipeLineStep<Input, Output> step, Func<Input, bool> iChoice)
        {
            Step = step;
            IChoice = iChoice;
        }

        public bool Choice(Input input)
        {
            if (IChoice != null)
            {
                return IChoice(input);
            }
            return false;
        }

        public Output Process(Input input)
        {
            if (Choice(input))
            {
                if (Step != null)
                {
                    return Step.Process(input);
                }
            }
            return input;
        }
    }

    public class OptionStep<Input, Output> : IPipeLineStep<Input, Output>
        where Input : Output
    {
        private readonly IPipeLineStep<Input, Output> Step;
        private Func<Input, bool> Choice;

        public OptionStep(IPipeLineStep<Input, Output> step, Func<Input, bool> choice)
        {
            Step = step;
            Choice = choice;
        }

        public Output Process(Input input)
        {
            return Choice(input) ? Step.Process(input) : input;
        }
    }

    public class AbsInt : IPipeLineStep<int, int>
    {
        public int Process(int input)
        {
            return input >= 0 ? input : -input;
        }
    }

    public class PipelineStyleProgram
    {
        public void Main()
        {
            //pipe
            var outstr = "123".Step(new StringToInt()).Step(new IntToFloat());
            Trace.WriteLine(outstr);
            var dt = DateTime.Now.Step(new DateTimeToString());
            Trace.WriteLine(dt);

            //DI
            var services = new ServiceCollection();
            services.AddTransient<TPipeline>();
            var provider = services.BuildServiceProvider();
            var t = provider.GetService<TPipeline>();
            var res = t.Process("123");
            Trace.WriteLine(res);

            //optional pipe
            var s3 = -210.OptionStep(new AbsInt(), r => r > 0);
            var s4 = 210.OptionStep(new AbsInt(), r => r > 0);
        }
    }
}