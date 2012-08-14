using System;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using WPF_Practice;
using System.Collections.Generic;
using WPF_Practice.MonitorControls;

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

                List<GroupSetting> Groups = XMLHandler.load("./MultiSaverConfig.xml");

                foreach (GroupSetting Group in Groups)
                {

                    int Mode = Group.ssType == "SlideShow" ? 1 : 2;

                    switch (Mode)
                    {

                        case 1:

                            foreach (MonitorSetting MS in Group.monitors)
                            {

                                foreach (Screen S in System.Windows.Forms.Screen.AllScreens)
                                {

                                    if (S.DeviceName == MS.monitorId)
                                    {

                                        Thread MonitorThread = new Thread(RunAlbum);
                                        MonitorThread.Start(new object[] {new Rectangle(S.Bounds.X, S.Bounds.Y, S.Bounds.Width, S.Bounds.Height), Group.albumLocation});
                                        Thread.Sleep(10000);

                                        break;

                                    }

                                }

                            }

                            break;

                        case 2:

                            foreach (MonitorSetting MS in Group.monitors)
                            {

                                foreach (Screen S in System.Windows.Forms.Screen.AllScreens)
                                {

                                    if (S.DeviceName == MS.monitorId)
                                    {

                                        Thread MazeAICenter = new Thread(RunMaze);
                                        MazeAICenter.Start(new object[] { new Rectangle(S.Bounds.X, S.Bounds.Y, S.Bounds.Width, S.Bounds.Height), S.Primary ? 0 : 1, S.Primary ? 0 : 10 });
                                        Thread.Sleep(1000);
                                        break;

                                    }


                                }


                            }

                            break;

                    }

                }

            }

            #endregion

            if (SuperMode == 1)
            {

                WPF_Practice.MainWindow ConfigPanel = new MainWindow();
                ConfigPanel.ShowDialog();

            }

        }

        public static void RunAlbum(object Bounds)
        {

            using (Album game = new Album())
            {

                Control C = Form.FromHandle(game.Window.Handle);
                Form F = C.FindForm();

                F.FormBorderStyle = FormBorderStyle.None;
                game.Bounds = (Rectangle)(Bounds as object[])[0];
                game.Location = (String)(Bounds as object[])[1];

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

