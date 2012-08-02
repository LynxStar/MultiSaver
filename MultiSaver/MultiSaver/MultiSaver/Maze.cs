using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MultiSaver
{
    
    public class Maze
    {

        public Vector2 Dimensions = new Vector2(25, 25);

        public Cell[,] Cells;

        public string State = "None";

        public VertexPositionNormalTexture[] Vertices;
        public VertexBuffer VerticesBuffer;
        public int[] Indices;
        public IndexBuffer IndicesBuffer;

        public VertexPositionColorTexture[] WallVertices;
        public VertexBuffer WallVerticesBuffer;
        public int[] WallIndices;
        public IndexBuffer WallIndicesBuffer;

        public Stack<Cell> CellStack = new Stack<Cell>();

        public Random Random = new Random(1);//526
        public Cell Location;
        public Cell End;

        GraphicsDevice Graphics;

        public void Generate()
        {

            Cells = new Cell[(int)Dimensions.X, (int)Dimensions.Y];

            for (int i = 0; i < Dimensions.X; i++)
            {

                for (int j = 0; j < Dimensions.Y; j++)
                {

                    Cells[i, j] = new Cell(new Vector2(i, j));

                }

            }

            State = "Generate";

            //Location = Cells[Random.Next((int)Dimensions.X), Random.Next((int)Dimensions.Y)];
            Location = Cells[0, 0];
            CellStack.Push(Location);

        }
        
        public void GenerateNext(Boolean ReRender = false)
        {

            if (CellStack.Count > 0)
            {

                List<Vector2> Neighbours = GetNeighbours(Location);

                if (Neighbours.Count > 0)
                {

                    Vector2 Temp = Neighbours[Random.Next(Neighbours.Count)];

                    Knockwall(Temp);

                    CellStack.Push(Location);
                    Location = Cells[(int)Temp.X, (int)Temp.Y];

                }

                else
                    Location = CellStack.Pop();

            }

            else
            {

                Cells[(int)Dimensions.X - 1, Random.Next((int)Dimensions.Y)].IsEnd = true;

                End = Cells[(int)Dimensions.X - 1, Random.Next((int)Dimensions.Y)];

                State = "Ready";

            }

            if (ReRender)
                GenerateWallPrimatives();

        }

        public void Knockwall(Vector2 Temp)
        {

            // The next is down
            if ((int)Location.Location.X == Temp.X && (int)Location.Location.Y > Temp.Y)
            {
                Cells[(int)Location.Location.X, (int)Location.Location.Y].TopWall = false;
                Cells[(int)Temp.X, (int)Temp.Y].BottomWall = false;
            }
            // the Next is up
            else if ((int)Location.Location.X == Temp.X)
            {
                Cells[(int)Location.Location.X, (int)Location.Location.Y].BottomWall = false;
                Cells[(int)Temp.X, (int)Temp.Y].TopWall = false;
            }
            // the Next is right
            else if ((int)Location.Location.X > Temp.X)
            {

                Cells[(int)Location.Location.X, (int)Location.Location.Y].LeftWall = false;
                Cells[(int)Temp.X, (int)Temp.Y].RightWall = false;
            }
            // the Next is left
            else
            {
                Cells[(int)Location.Location.X, (int)Location.Location.Y].RightWall = false;
                Cells[(int)Temp.X, (int)Temp.Y].LeftWall = false;
            }

        }

        public List<Vector2> GetNeighbours(Cell C)
        {

            Vector2 Temp = C.Location;

            List<Vector2> Available = new List<Vector2>();

            //Left
            Temp.X = C.Location.X - 1;
            if (Temp.X >= 0 && Cells[(int)Temp.X, (int)Temp.Y].AllWallsIntact())
                Available.Add(Temp);

            //Right
            Temp.X = C.Location.X + 1;
            if (Temp.X < Dimensions.X && Cells[(int)Temp.X, (int)Temp.Y].AllWallsIntact())
                Available.Add(Temp);

            //Top
            Temp.X = C.Location.X;
            Temp.Y = C.Location.Y - 1;
            if (Temp.Y >= 0 && Cells[(int)Temp.X, (int)Temp.Y].AllWallsIntact())
                Available.Add(Temp);

            Temp.Y = C.Location.Y + 1;
            if (Temp.Y < Dimensions.Y && Cells[(int)Temp.X, (int)Temp.Y].AllWallsIntact())
                Available.Add(Temp);

            return Available;

        }

        public void GenerateGroundPrimatives(GraphicsDevice G)
        {

            Graphics = G;

            Vertices = new VertexPositionNormalTexture[(int)(Dimensions.X * Dimensions.Y) * 4];
            Indices = new int[(int)(Dimensions.X * Dimensions.Y) * 6];

            VerticesBuffer = new VertexBuffer(G, typeof(VertexPositionNormalTexture), Vertices.Length, BufferUsage.WriteOnly);
            IndicesBuffer = new IndexBuffer(G, IndexElementSize.ThirtyTwoBits, Indices.Length, BufferUsage.WriteOnly);

            int i = 0;
            int j = 0;

            for (int y = 0; y < Dimensions.Y; y++)
            {

                for (int x = 0; x < Dimensions.X; x++)
                {

                    Vector3 Location = new Vector3(x * 110, 0, y * 110);

                    Vertices[i++] = new VertexPositionNormalTexture(Location, Vector3.Zero, new Vector2(Location.X / 128, Location.Z / 128));
                    Vertices[i++] = new VertexPositionNormalTexture(Location + new Vector3(110, 0, 0), Vector3.Zero, new Vector2((Location.X + 110) / 128, Location.Z / 128));
                    Vertices[i++] = new VertexPositionNormalTexture(Location + new Vector3(0, 0, 110), Vector3.Zero, new Vector2(Location.X / 128, (Location.Z + 110) / 128));
                    Vertices[i++] = new VertexPositionNormalTexture(Location + new Vector3(110, 0, 110), Vector3.Zero, new Vector2((Location.X + 110) / 128, (Location.Z + 110) / 128));

                    Indices[j++] = i - 4;
                    Indices[j++] = i - 3;
                    Indices[j++] = i - 2;

                    Indices[j++] = i - 2;
                    Indices[j++] = i - 3;
                    Indices[j++] = i - 1;

                }


            }

            VerticesBuffer.SetData<VertexPositionNormalTexture>(Vertices);
            IndicesBuffer.SetData<int>(Indices);

        }

        public void GenerateWallPrimatives()
        {

            WallVertices = new VertexPositionColorTexture[(int)(Dimensions.X * Dimensions.Y) * 4 * 8];
            WallIndices = new int[(int)(Dimensions.X * Dimensions.Y) * 4 * 30];

            WallVerticesBuffer = new VertexBuffer(Graphics, typeof(VertexPositionColorTexture), WallVertices.Length, BufferUsage.WriteOnly);
            WallIndicesBuffer = new IndexBuffer(Graphics, IndexElementSize.ThirtyTwoBits, WallIndices.Length, BufferUsage.WriteOnly);

            int i = 0;
            int j = 0;

            List<VertexPositionColorTexture> Vertices = new List<VertexPositionColorTexture>();
            List<int> Indices = new List<int>();

            for (int y = 0; y < Dimensions.Y; y++)
            {

                for (int x = 0; x < Dimensions.X; x++)
                {
                    
                    #region Left
                    int[] Inds = new int[30];
                    VertexPositionColorTexture[] Verts = new VertexPositionColorTexture[8];

                    int Y = Cells[x, y].LeftWall ? 110 : -100;

                    BoundingBox Temp = new BoundingBox(new Vector3(x * 110, Y - 110, y * 110), new Vector3(x * 110 + 5, Y, y * 110 + 110));

                    GenerateBoxVertices(Temp, Inds, Verts, new Vector2(.5f, 0), i);

                    i += 8;
                    Indices.AddRange(Inds);
                    Vertices.AddRange(Verts);
                    
                    #endregion
                    
                    #region Right
                    Inds = new int[30];
                    Verts = new VertexPositionColorTexture[8];

                    Y = Cells[x, y].RightWall ? 110 : -100;

                    Temp = new BoundingBox(new Vector3(x * 110 + 105, Y - 110, y * 110), new Vector3(x * 110 + 110, Y, y * 110 + 110));

                    GenerateBoxVertices(Temp, Inds, Verts, new Vector2(.5f, .5f), i);

                    i += 8;
                    Indices.AddRange(Inds);
                    Vertices.AddRange(Verts);

                    #endregion

                    #region Top
                    Inds = new int[30];
                    Verts = new VertexPositionColorTexture[8];

                    Y = Cells[x, y].TopWall ? 110 : -100;

                    Temp = new BoundingBox(new Vector3(x * 110, Y - 110, y * 110), new Vector3(x * 110 + 110, Y, y * 110 + 5));

                    GenerateBoxVertices(Temp, Inds, Verts, new Vector2(0, 0), i);

                    i += 8;
                    Indices.AddRange(Inds);
                    Vertices.AddRange(Verts);

                    #endregion

                    #region Bottom
                    Inds = new int[30];
                    Verts = new VertexPositionColorTexture[8];

                    Y = Cells[x, y].BottomWall ? 110 : -100;

                    Temp = new BoundingBox(new Vector3(x * 110, Y -110, y * 110 + 105), new Vector3(x * 110 + 110, Y, y * 110 + 110));

                    GenerateBoxVertices(Temp, Inds, Verts, new Vector2(0, .5f), i);

                    i += 8;
                    Indices.AddRange(Inds);
                    Vertices.AddRange(Verts);

                    #endregion

                    #region Floor
                    //Inds = new int[30];
                    //Verts = new VertexPositionColor[8];

                    //Color C = Cells[x, y].AllWallsIntact() ? Color.Transparent : (Cells[x,y].IsEnd ? Color.White : Color.Gray);

                    //Temp = new BoundingBox(new Vector3(x * 110, -1, y * 110), new Vector3(x * 110 + 110, -1, y * 110 + 110));

                    //GenerateBoxVertices(Temp, Inds, Verts, C, i);

                    //i += 8;
                    //Indices.AddRange(Inds);
                    //Vertices.AddRange(Verts);

                    #endregion

                }


            }

            WallIndices = Indices.ToArray();
            WallVertices = Vertices.ToArray();

            WallVerticesBuffer.SetData<VertexPositionColorTexture>(WallVertices);
            WallIndicesBuffer.SetData<int>(WallIndices);

        }

        public void GenerateBoxVertices(BoundingBox Box, int[] Indices, VertexPositionColorTexture[] Vertices, Vector2 UVOffset, int Offset)
        {

            Vector3[] corners = Box.GetCorners();
            for (int i = 0; i < 8; i++)
            {

                Vertices[i].Position = corners[i];
                Vertices[i].Color = Color.White;

            }

            if (UVOffset.X == 0)
            {

                Vertices[0].TextureCoordinate = new Vector2(0, 0) + UVOffset;
                Vertices[3].TextureCoordinate = new Vector2(0, .5f) + UVOffset;
                Vertices[1].TextureCoordinate = new Vector2(.5f, 0) + UVOffset;
                Vertices[2].TextureCoordinate = new Vector2(.5f, .5f) + UVOffset;

                Vertices[4].TextureCoordinate = new Vector2(0, 0) + UVOffset;
                Vertices[7].TextureCoordinate = new Vector2(0, .5f) + UVOffset;
                Vertices[5].TextureCoordinate = new Vector2(.5f, 0) + UVOffset;
                Vertices[6].TextureCoordinate = new Vector2(.5f, .5f) + UVOffset;

            }

            else
            {

                Vertices[0].TextureCoordinate = new Vector2(0, 0) + UVOffset;
                Vertices[3].TextureCoordinate = new Vector2(0, .5f) + UVOffset;
                Vertices[4].TextureCoordinate = new Vector2(.5f, 0) + UVOffset;
                Vertices[7].TextureCoordinate = new Vector2(.5f, .5f) + UVOffset;

                Vertices[1].TextureCoordinate = new Vector2(0, 0) + UVOffset;
                Vertices[2].TextureCoordinate = new Vector2(0, .5f) + UVOffset;
                Vertices[5].TextureCoordinate = new Vector2(.5f, 0) + UVOffset;
                Vertices[6].TextureCoordinate = new Vector2(.5f, .5f) + UVOffset;

            }

            //Left Side
            Indices[0] = 7 + Offset;
            Indices[1] = 5 + Offset;
            Indices[2] = 6 + Offset;

            Indices[3] = 7 + Offset;
            Indices[4] = 4 + Offset;
            Indices[5] = 5 + Offset;

            //Right Side
            Indices[6] = 3 + Offset;
            Indices[7] = 2 + Offset;
            Indices[8] = 1 + Offset;

            Indices[9] = 0 + Offset;
            Indices[10] = 1 + Offset;
            Indices[11] = 3 + Offset;

            //Front Side
            Indices[12] = 7 + Offset;
            Indices[13] = 0 + Offset;
            Indices[14] = 3 + Offset;

            Indices[15] = 4 + Offset;
            Indices[16] = 0 + Offset;
            Indices[17] = 7 + Offset;

            //Back Side
            Indices[18] = 6 + Offset;
            Indices[19] = 1 + Offset;
            Indices[20] = 2 + Offset;

            Indices[21] = 5 + Offset;
            Indices[22] = 1 + Offset;
            Indices[23] = 6 + Offset;

            //Top Side
            Indices[24] = 4 + Offset;
            Indices[25] = 5 + Offset;
            Indices[26] = 1 + Offset;

            Indices[27] = 4 + Offset;
            Indices[28] = 1 + Offset;
            Indices[29] = 0 + Offset;

        }

        public Cell Guess(Stack<Cell> PathStack, int ID, Cell Next)
        {

            if (PathStack.Count > 0)
            {

                Cell Temp = PathStack.Pop();

                if (Temp.IsFake)
                {

                    return PathStack.Peek();

                }

                //Check if at the end
                if (Temp.Location == End.Location)
                {

                    State = "Win|" + ID;

                    End.SetVisit(ID, true);

                }

                Temp.SetVisit(ID, true);

                Boolean Neighbors = false;

                #region Add Neighbours

                //Left
                if (Temp.Location.X - 1 >= 0
                    && !Cells[(int)Temp.Location.X - 1, (int)Temp.Location.Y].RightWall
                    && !Cells[(int)Temp.Location.X - 1, (int)Temp.Location.Y].VisitedBy(ID))
                {

                    Next = Cells[(int)Temp.Location.X - 1, (int)Temp.Location.Y];
                    Next.SetPrevious(ID, Temp);
                    PathStack.Push(Next);
                    Neighbors = true;

                }

                //Right
                if (Temp.Location.X + 1 < Dimensions.X
                    && !Cells[(int)Temp.Location.X + 1, (int)Temp.Location.Y].LeftWall
                    && !Cells[(int)Temp.Location.X + 1, (int)Temp.Location.Y].VisitedBy(ID))
                {

                    Next = Cells[(int)Temp.Location.X + 1, (int)Temp.Location.Y];
                    Next.SetPrevious(ID, Temp);
                    PathStack.Push(Next);
                    Neighbors = true;

                }

                //Top
                if (Temp.Location.Y -1 >= 0
                    && !Cells[(int)Temp.Location.X, (int)Temp.Location.Y - 1].BottomWall
                    && !Cells[(int)Temp.Location.X, (int)Temp.Location.Y - 1].VisitedBy(ID))
                {

                    Next = Cells[(int)Temp.Location.X, (int)Temp.Location.Y - 1];
                    Next.SetPrevious(ID, Temp);
                    PathStack.Push(Next);
                    Neighbors = true;

                }

                //Bottom
                if (Temp.Location.Y + 1 < Dimensions.Y
                    && !Cells[(int)Temp.Location.X, (int)Temp.Location.Y + 1].TopWall
                    && !Cells[(int)Temp.Location.X, (int)Temp.Location.Y + 1].VisitedBy(ID))
                {

                    Next = Cells[(int)Temp.Location.X, (int)Temp.Location.Y + 1];
                    Next.SetPrevious(ID, Temp);
                    PathStack.Push(Next);
                    Neighbors = true;

                }

                #endregion

                if (!Neighbors)
                {
                    
                    //Adds fake steps in so that the AI will animate the retracing of its steps
                    List<Cell> Fakes = GetPathDFS(Temp, PathStack.Peek());
                    Fakes.RemoveAt(0);
                    Fakes.RemoveAt(Fakes.Count - 1);
                    
                    foreach (Cell C in Fakes)
                    {

                        PathStack.Push(C);

                    }

                    Next = PathStack.Peek();

                }

                return Next;

            }

            else
            {

                //HERPITY FUCKING DERP
                return null;

            }

        }

        public List<Cell> GetPathDFS(Cell Start, Cell End)
        {

            DFS Solver = new DFS(Cells, (int)Dimensions.X, (int)Dimensions.Y);

            if (Solver.DFSSolve(Solver.Maze[(int)Start.Location.X, (int)Start.Location.Y], Solver.Maze[(int)End.Location.X, (int)End.Location.Y]))
                return Solver.Path;

            else
                return null;

        }

    }

    public class DFS
    {

        public List<Cell> Path;
        public Cell[,] Maze;

        int Height;
        int Width;

        public DFS(Cell[,] MazeData, int Height, int Width)
        {

            this.Height = Height;
            this.Width = Width;

            Maze = GenericCopier<Cell[,]>.DeepCopy(MazeData);
            
            foreach (Cell C in Maze)
            {

                C.SetVisit(1, false);
                C.IsFake = true;

            }

            Path = new List<Cell>();

        }

        public bool DFSSolve(Cell Start, Cell End)
        {
            // base condition
            if (Start.Location == End.Location)
            {
                // make it visited in order to be drawed with green
                Maze[(int)Start.Location.X, (int)Start.Location.Y].SetVisit(1, true);
                // add end point to the foundPath
                Path.Add(Start);
                return true;
            }


            // has been visited alread, return
            if (Maze[(int)Start.Location.X, (int)Start.Location.Y].VisitedBy(1))
                return false;


            // mark as visited
            Maze[(int)Start.Location.X, (int)Start.Location.Y].SetVisit(1, true);

            // Check every neighbor cell
            // If it exists (not outside the maze bounds)
            // and if there is no wall between start and it
            // recursive call this method with it
            // if it returns true, add the current start to foundPath and return true too
            // else complete

            // Left
            if (Start.Location.X - 1 >= 0 && !Maze[(int)Start.Location.X - 1, (int)Start.Location.Y].RightWall)
            {
                //Maze[(int)Start.Location.X, (int)Start.Location.Y].Path = Cell.Paths.Left;
                Cell Next = Maze[(int)Start.Location.X - 1, (int)Start.Location.Y];
                {
                    if (DFSSolve(Next, End))
                    {
                        Path.Add(Start);
                        return true;
                    }
                }
            }
            // Right
            if (Start.Location.X + 1 < Width && !Maze[(int)Start.Location.X + 1, (int)Start.Location.Y].LeftWall)
            {
                //Maze[(int)Start.Location.X, (int)Start.Location.Y].Path = Cell.Paths.Right;
                Cell Next = Maze[(int)Start.Location.X + 1, (int)Start.Location.Y];
                {
                    if (DFSSolve(Next, End))
                    {
                        Path.Add(Start);
                        return true;
                    }
                }
            }

            // Up
            if (Start.Location.Y - 1 >= 0 && !Maze[(int)Start.Location.X, (int)Start.Location.Y - 1].BottomWall)
            {
                //Maze[(int)Start.Location.X, (int)Start.Location.Y].Path = Cell.Paths.Up;
                Cell Next = Maze[(int)Start.Location.X, (int)Start.Location.Y - 1];
                {
                    if (DFSSolve(Next, End))
                    {
                        Path.Add(Start);
                        return true;
                    }
                }
            }


            // Down
            if (Start.Location.Y + 1 < Height && !Maze[(int)Start.Location.X, (int)Start.Location.Y + 1].TopWall)
            {
                //Maze[(int)Start.Location.X, (int)Start.Location.Y].Path = Cell.Paths.Down;
                Cell Next = Maze[(int)Start.Location.X, (int)Start.Location.Y + 1];
                {
                    if (DFSSolve(Next, End))
                    {
                        Path.Add(Start);
                        return true;
                    }
                }
            }
            //Maze[(int)Start.Location.X, (int)Start.Location.Y].Path = Cell.Paths.None;
            return false;
        }

    }

    public static class GenericCopier<T>//deep copy a list
    {
        public static T DeepCopy(object objectToCopy)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, objectToCopy);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return (T)binaryFormatter.Deserialize(memoryStream);
            }
        }
    }

}