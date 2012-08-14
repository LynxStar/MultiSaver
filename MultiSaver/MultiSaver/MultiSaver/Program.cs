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
        static void Main(string[] args)
        {

            int SuperMode = 0;

            if (args.Length > 0)
            {
                string firstArgument = args[0].ToLower().Trim();
                string secondArgument = null;

                // Handle cases where arguments are separated by colon.
                // Examples: /c:1234567 or /P:1234567
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
                    MessageBox.Show("Sorry, but the command line argument \"" + firstArgument +
                        "\" is not valid.", "ScreenSaver",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                                        MonitorThread.Start(new Rectangle(S.Bounds.X, S.Bounds.Y, S.Bounds.Width, S.Bounds.Height), Group.albumLocation);
                                        Thread.Sleep(1);

                                        break;

                                    }

                                }

                            }

                            break;

                        case 2:

                            Thread MazeAICenter = new Thread(RunMaze);
                            Thread MazeAILeft = new Thread(RunMaze);

                            MazeAICenter.Start(new object[] { new Rectangle(0, 0, 1600, 900), 0, 0 });
                            Thread.Sleep(1000);
                            MazeAILeft.Start(new object[] { new Rectangle(-1000, 0, 1680, 1050), 1, 10 });//Stagger to account for XNA and DirectX and Windows 
                            //and generally computers not being designed for MM

                            break;

                    }

                }

            }

            #endregion

        }

        public static void RunAlbum(object Bounds)
        {

            using (Album game = new Album())
            {

                Control C = Form.FromHandle(game.Window.Handle);
                Form F = C.FindForm();

                F.FormBorderStyle = FormBorderStyle.None;
                game.Bounds = (Rectangle)Bounds;

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

                game.IsLeft = game.Bounds.X < 0 ? true : false;

                game.Run();

            }

        }

    }
#endif
}

