using System;

namespace TimeGame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new TimeGame())
                game.Run();
        }
    }
}
