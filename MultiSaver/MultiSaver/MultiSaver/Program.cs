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
        public static Maze MasterMaze;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {

            //Thread Monitor1 = new Thread(RunAlbum);
            //Thread Monitor2 = new Thread(RunAlbum);

            //Monitor1.Start(new Rectangle(0, 0, 1600, 900));
            //Thread.Sleep(1);
            //Monitor2.Start(new Rectangle(-1000, 0, 1680, 1050));

            Thread MazeAICenter = new Thread(RunMaze);
            //Thread MazeAILeft = new Thread(RunMaze);

            MazeAICenter.Start(new object[] {new Rectangle(0, 0, 1680, 1050), 0 });

        }

        public static void RunAlbum(object Bounds)
        {

            using (Album game = new Album())
            {

                Control C = Form.FromHandle(game.Window.Handle);
                Form F = C.FindForm();

                F.FormBorderStyle = FormBorderStyle.None;
                game.Bounds = (Rectangle)Bounds;

                game.IsLeft = (Bounds as Rectangle?).Value.X < 0 ? true : false;

                game.Run();
            
            }

        }

        public static void RunMaze(object Bounds)
        {

            using (MazeAI game = new MazeAI())
            {

                Control C = Form.FromHandle(game.Window.Handle);
                Form F = C.FindForm();

                F.FormBorderStyle = FormBorderStyle.None;
                game.Bounds = (Rectangle)(Bounds as object[])[0];
                game.ID = (int)(Bounds as object[])[1];

                game.IsLeft = game.Bounds.X < 0 ? true : false;

                game.Run();

            }

        }

    }
#endif
}

