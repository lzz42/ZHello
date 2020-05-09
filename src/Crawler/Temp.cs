using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace absCardReader
{

    public interface InA
    {
        int F1(string str);
    }
    public interface InB
    {
        int F1(string str, int a);
    }
    public interface InC
    {
        int F1(string str, int b, int c);
    }

    public interface IA:InA
    {
    }

    public interface IB:InB
    {
    }

    public interface IC:InC
    {
    }

    public abstract class Base : InA, InB, InC
    {
        public virtual int F1(string str)
        {
            var res = FindCard();
            return 0;
        }

        public virtual int F1(string str, int a)
        {
            return 0;
        }

        public virtual int F1(string str, int b, int c)
        {
            return 0;
        }

        protected abstract int FindCard();
    }

    public class A : Base, IA
    {
        protected override int FindCard()
        {
            return 0;
        }
    }

    public class B : Base, IB, IC
    {
        protected override int FindCard()
        {
            return 0;
        }
    }

    public class UsedClass
    {
        public void Main()
        {
            Base b1 = new A();
            b1.F1("5455");
        }
    }

}
