using System;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace MultiSaver
{
#if WINDOWS || XBOX
    public static class Program
    {
        
        public static Random Rand = new Random();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {

            Thread Monitor1 = new Thread(Run);
            Thread Monitor2 = new Thread(Run);


            Monitor1.Start(new Rectangle(0, 0, 1600, 900));

            Thread.Sleep(1);

            Monitor2.Start(new Rectangle(-1000, 0, 1680, 1050));
            
        }

        public static void Run(object Bounds)
        {

            using (Screensaver game = new Screensaver())
            {

                Control C = Form.FromHandle(game.Window.Handle);
                Form F = C.FindForm();

                F.FormBorderStyle = FormBorderStyle.None;
                game.Bounds = (Rectangle)Bounds;

                game.IsLeft = (Bounds as Rectangle?).Value.X < 0 ? true : false;

                game.Run();
            
            }

        }

    }
#endif
}

