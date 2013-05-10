using System;

namespace SomeGame
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (PudgeWarsGame game = new PudgeWarsGame())
            {
                game.Run();
            }
        }
    }
#endif
}

