using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.Concurrent;

namespace MultiSaver
{
    
    public class Cell
    {

        public bool LeftWall;
        public bool RightWall;
        public bool TopWall;
        public bool BottomWall;

        public Vector2 Location;

        private bool VisitedBy0;
        private bool VisitedBy1;
        private bool VisitedBy2;
        private bool VisitedBy3;

        private Cell Previous0;
        private Cell Previous1;
        private Cell Previous2;
        private Cell Previous3;

        public Color LeftColor;
        public Color RightColor;
        public Color TopColor;
        public Color BottomColor;

        public bool IsEnd = false;

        public Cell(Vector2 Location)
        {

            this.Location = Location;

            LeftWall = true;
            RightWall = true;
            TopWall = true;
            BottomWall = true;

            VisitedBy0 = false;
            VisitedBy1 = false;
            VisitedBy2 = false;
            VisitedBy3 = false;

            Previous0 = null;
            Previous1 = null;
            Previous2 = null;
            Previous3 = null;

            LeftColor = new Color(Program.Rand.Next(50, 255), 0, 0);
            RightColor = new Color(0, 0, Program.Rand.Next(50, 255));
            TopColor = new Color(0, Program.Rand.Next(50, 255), 0);

            int Yellow = Program.Rand.Next(50, 255);

            BottomColor = new Color(Yellow, Yellow, 0);

        }

        public bool VisitedBy(int Index)
        {

            switch (Index)
            {

                case 0:
                    return VisitedBy0;
                case 1:
                    return VisitedBy1;
                case 2:
                    return VisitedBy2;
                case 3:
                    return VisitedBy3;
                default:
                    return VisitedBy0;

            }

        }

        public void SetVisit(int Index, bool State)
        {

            switch (Index)
            {

                case 0:
                    VisitedBy0 = State;
                    break;
                case 1:
                    VisitedBy1 = State;
                    break;
                case 2:
                    VisitedBy2 = State;
                    break;
                case 3:
                    VisitedBy3 = State;
                    break;
                default:
                    VisitedBy0 = State;
                    break;

            }

        }

        public Cell Previous(int Index)
        {

            switch (Index)
            {

                case 0:
                    return Previous0;
                case 1:
                    return Previous1;
                case 2:
                    return Previous2;
                case 3:
                    return Previous3;
                default:
                    return Previous0;

            }

        }

        public void SetPrevious(int Index, Cell Previous)
        {

            switch (Index)
            {

                case 0:
                    Previous0 = Previous; break;
                case 1:
                    Previous1 = Previous; break;
                case 2:
                    Previous2 = Previous; break;
                case 3:
                    Previous3 = Previous; break;
                default:
                    Previous0 = Previous; break;

            }

        }

        public bool AllWallsIntact()
        {

            return LeftWall && RightWall && TopWall && BottomWall;

        }

    }

}