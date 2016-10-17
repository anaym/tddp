using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class B
    {
        
    }

    class D : B
    {
        
    }

    class Program
    {
        static string Foo<T>(T arg)
        {
            return arg.GetType().Name;
        }
        static void Main(string[] args)
        {
            //TODO: save to file from input
            Console.WriteLine(Foo((object)1));
            Console.WriteLine(Foo(new D()));
        }
    }
}
