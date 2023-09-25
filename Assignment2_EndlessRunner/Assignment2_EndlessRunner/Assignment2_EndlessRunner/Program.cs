using System;

namespace Assignment2_EndlessRunner
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (EndlessRunner game = new EndlessRunner())
            {
                game.Run();
            }
        }
    }
#endif
}

