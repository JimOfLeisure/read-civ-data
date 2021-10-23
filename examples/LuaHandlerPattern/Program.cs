using System;

namespace LuaHandlerPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            // .NET 5 and later need Nuget package "System.Text.Encoding.CodePages" and registration for Windows-1252 (and other old locales) encoding
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }
    }
}
