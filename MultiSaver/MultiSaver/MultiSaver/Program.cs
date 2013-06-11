using System;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;

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
        [STAThread()]
        static void Main(string[] args)
        {

            int SuperMode = 0;

            if (args.Length > 0)
            {

                string firstArgument = args[0].ToLower().Trim();
                string secondArgument = null;

                
                if (firstArgument.Length > 2)
                {

                    secondArgument = firstArgument.Substring(3).Trim();
                    firstArgument = firstArgument.Substring(0, 2);

                }

                else if (args.Length > 1)
                    secondArgument = args[1];

                if (firstArgument == "/c")           // Configuration mode
                {

                    SuperMode = 1;

                }

                else if (firstArgument == "/p")      // Preview mode
                {
                    
                    SuperMode = 2;

                }

                else if (firstArgument == "/s")      // Full-screen mode
                {

                    SuperMode = 0;

                }

                else    // Undefined argument
                {
                    
                    MessageBox.Show("Sorry, but the command line argument \"" + firstArgument + "\" is not valid.", "ScreenSaver", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                
                }
            
            }
            else    // No arguments - treat like /c
            {

                SuperMode = 0;

            }

            #region Screensaver

            if (SuperMode == 0)
            {

                if (File.Exists("./MultiSaverConfiguration.xml"))
                {

                    ConfigData.Settings ConfigSettings = new ConfigData.Settings();

                    ConfigSettings.Load();

                    foreach (ConfigData.Monitor M in ConfigSettings.Slideshow)
                    {

                        Thread MonitorThread = new Thread(RunAlbum);
                        MonitorThread.Start(M);

                    }

                    bool Primary = true;

                    foreach (ConfigData.Monitor M in ConfigSettings.Maze)
                    {

                        Thread MazeAICenter = new Thread(RunMaze);
                        MazeAICenter.Start(new object[] { new Rectangle(M.Bounds.X, M.Bounds.Y, M.Bounds.Width, M.Bounds.Height), Primary ? 0 : 1, Primary ? 0 : 10 });
                        Primary = false;
                        Thread.Sleep(1000);

                    }

                }

                else
                {

                    foreach (Screen S in System.Windows.Forms.Screen.AllScreens)
                    {

                        Thread MonitorThread = new Thread(RunAlbum);
                        MonitorThread.Start(new ConfigData.Monitor { Bounds = new System.Drawing.Rectangle(S.Bounds.X, S.Bounds.Y, S.Bounds.Width, S.Bounds.Height) });

                    }

                }

            }

            #endregion

            if (SuperMode == 1)
            {

                //WPF_Practice.MainWindow ConfigPanel = new MainWindow();
                //ConfigPanel.ShowDialog();

                ConfigPanel.MainWindow ConfigPanel = new ConfigPanel.MainWindow();
                ConfigPanel.ShowDialog();

            }

            if (SuperMode == 2)
            {

                string firstArgument = args[0].ToLower().Trim();
                string secondArgument = null;

                if (firstArgument.Length > 2)
                {
                    secondArgument = firstArgument.Substring(3).Trim();
                    firstArgument = firstArgument.Substring(0, 2);
                }
                else if (args.Length > 1)
                    secondArgument = args[1];

                IntPtr previewWndHandle = new IntPtr(long.Parse(secondArgument));

                Thread MonitorThread = new Thread(RunDemo);
                MonitorThread.Start(new object[] { new Rectangle(0, 0, 800, 600), previewWndHandle });


            }

        }

        public static void RunAlbum(object Args)
        {

            using (Album game = new Album())
            {

                ConfigData.Monitor Mon = (ConfigData.Monitor)Args;

                Control C = Form.FromHandle(game.Window.Handle);
                Form F = C.FindForm();

                F.FormBorderStyle = FormBorderStyle.None;
                game.Bounds = new Rectangle(Mon.Bounds.X, Mon.Bounds.Y, Mon.Bounds.Width, Mon.Bounds.Height);
                
                game.Location = Mon.Source;
                game.TransitionMode = Mon.TransitionMode;
                game.Order = Mon.Order;

                game.TileType = Mon.TileType;
                game.FixedTiles = Mon.FixedTiles;
                game.MinTiles = Mon.MinTiles;
                game.MaxTiles = Mon.MaxTiles;

                game.TransitionTime = Mon.TransitionTime;
                game.FixedTime = Mon.FixedTime;
                game.MinTime = Mon.MinTime;
                game.MaxTime = Mon.MaxTime;

                game.Run();
            
            }

        }

        public static void RunDemo(object Bounds)
        {

            
            using (Album game = new Album())
            {

                Control C = Form.FromHandle(game.Window.Handle);
                Form F = C.FindForm();

                F.FormBorderStyle = FormBorderStyle.None;
                game.Bounds = (Rectangle)(Bounds as object[])[0];
                game.Handler = (IntPtr)(Bounds as object[])[1];

                //game.IsLeft = (Bounds as Rectangle?).Value.X < 0 ? true : false;

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

                game.IsLeft = game.ID == 0 ? false : true;

                game.Run();

            }

        }

    }
#endif
}

