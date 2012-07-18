using System;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using ConfigPanel;
using System.Collections.Generic;

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

            int Mode = 3;
            
            List<GroupSetting> Groups = XMLHandler.load("./config.xml");

            foreach (GroupSetting Group in Groups)
            {

                //int Mode = Group.ssType == "SlideShow" ? 1 : 3;
                
                switch (Mode)
                {

                    case 1:

                        Thread Monitor1 = new Thread(RunAlbum);
                        Thread Monitor2 = new Thread(RunAlbum);

                        Monitor1.Start(new Rectangle(0, 0, 1600, 900));
                        Thread.Sleep(1);
                        Monitor2.Start(new Rectangle(-1000, 0, 1920, 1080));

                        break;

                    case 2:

                        Thread MazeAICenterSolo = new Thread(RunMaze);

                        MazeAICenterSolo.Start(new object[] { new Rectangle(0, 0, 1600, 900), 0, 0 });

                        Thread Monitor3 = new Thread(RunAlbum);

                        Monitor3.Start(new Rectangle(-1000, 0, 1920, 1080));

                        break;

                    case 3:

                        Thread MazeAICenter = new Thread(RunMaze);
                        Thread MazeAILeft = new Thread(RunMaze);

                        MazeAICenter.Start(new object[] { new Rectangle(0, 0, 1600, 900), 0, 0 });
                        Thread.Sleep(1000);
                        MazeAILeft.Start(new object[] { new Rectangle(-1000, 0, 1920, 1080), 1, 10 });//Stagger to account for XNA and DirectX and Windows 
                        //and generally computers not being designed for MM

                        break;

                }

            }

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
                game.Stagger = (int)(Bounds as object[])[2];

                game.IsLeft = game.Bounds.X < 0 ? true : false;

                game.Run();

            }

        }

    }
#endif
}

