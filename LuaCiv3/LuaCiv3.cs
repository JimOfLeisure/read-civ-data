using System;
using MoonSharp.Interpreter;

namespace ReadCivData.LuaCiv3 {
    public class Test
    {
        public static string Foo()
        {
            return "Hi from LuaCiv3";
        }
        public static void DoLua(string script)
        {
            Script.RunString(script);
        }
    }
        
}