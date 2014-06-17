﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiSaver.Presentation
{
    public class MazeAI : Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Effect MazeEffect;

        String MasterMode = String.Empty;
        String SelfMode = String.Empty;

        MouseState lastMouseState;

        Texture2D MazeTexture;

        FreeCamera MovableCamera;

        int SmallTimer;
        int Delay;

        public String CameraState = "Waiting";

        public Rectangle Bounds = Rectangle.Empty;

        public bool IsLeft = false;

        public int ID;

        BasicEffect BEffect;

        Stack<Cell> PathStack;

        bool DoOnce = true;

        Cell Current;
        Cell Next;

        String CurrentDirection = "";

        int RotateBy = 0;

        int ReRender = 0;
        public int Stagger = 0;

        public VertexBuffer WallVerticesBuffer;
        public IndexBuffer WallIndicesBuffer;

        public VertexPositionNormalTexture[] Vertices;
        public VertexBuffer VerticesBuffer;
        public int[] Indices;
        public IndexBuffer IndicesBuffer;

        Vector2 HasMoved;
        Vector2 ShouldMove;
        Vector3 Start;

        public Texture2D WallTiling;

        public MazeAI()
        {

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.SynchronizeWithVerticalRetrace = false;

        }

        protected override void Initialize()
        {

            this.InactiveSleepTime = TimeSpan.Zero;
            base.Initialize();

        }

        protected override void LoadContent()
        {

            Delay = Program.Rand.Next(300, 3000);

            graphics.PreferredBackBufferHeight = Bounds.Height;
            graphics.PreferredBackBufferWidth = Bounds.Width;
            graphics.ApplyChanges();
            User32.SetWindowPos((uint)this.Window.Handle, 0, Bounds.X, Bounds.Y, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, 0);

            spriteBatch = new SpriteBatch(GraphicsDevice);

            if (ID == 0)
            {

                Program.MasterMaze = new Maze();
                Program.MasterMaze.Generate();

                MasterMode = "Generate";

                Program.MasterMaze.GenerateGroundPrimatives(GraphicsDevice);

            }

            MovableCamera = new FreeCamera(new Vector3(1375, 3750, 1375), 0, MathHelper.ToRadians(-90), GraphicsDevice);
            MovableCamera.Update();

            MazeEffect = Content.Load<Effect>("MazeEffect");
            MazeTexture = Content.Load<Texture2D>("Floor");
            WallTiling = Content.Load<Texture2D>("Walls");

            BEffect = new BasicEffect(GraphicsDevice);
            BEffect.VertexColorEnabled = true;
            BEffect.LightingEnabled = false;

        }

        protected override void UnloadContent()
        {



        }

        protected override void Update(GameTime gameTime)
        {

            if (MasterMode != Program.MasterMaze.State)
            {

                MasterMode = Program.MasterMaze.State;

            }

            if (Program.MasterMaze.State.Contains("Win"))
            {

                if (ID == 0)
                {

                    Program.MasterMaze.State = "Generate";

                }

                else
                {

                    Thread.Sleep(1000);

                }

            }

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            #region Free Camera

            KeyboardState keyState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            if (keyState.IsKeyDown(Keys.Space))
            {

                // Determine how much the camera should turn
                float deltaX = (float)lastMouseState.X - (float)mouseState.X;
                float deltaY = (float)lastMouseState.Y - (float)mouseState.Y;

                // Rotate the camera
                MovableCamera.Rotate(deltaX * .005f, deltaY * .005f);

            }

            Vector3 translation = Vector3.Zero;

            // Determine in which direction to move the camera
            if (keyState.IsKeyDown(Keys.W)) translation += Vector3.Forward;
            if (keyState.IsKeyDown(Keys.S)) translation += Vector3.Backward;
            if (keyState.IsKeyDown(Keys.A)) translation += Vector3.Left;
            if (keyState.IsKeyDown(Keys.D)) translation += Vector3.Right;
            if (keyState.IsKeyDown(Keys.F12)) this.Exit();

            // Move 4 units per millisecond, independent of frame rate
            translation *= (float)(0.5 * gameTime.ElapsedGameTime.TotalMilliseconds) * (Keyboard.GetState().IsKeyDown(Keys.LeftControl) ? 2.5f : 1f) * (Keyboard.GetState().IsKeyDown(Keys.LeftShift) ? 10f : 1f);

            // Move the camera
            MovableCamera.Move(translation);
            MovableCamera.Update();

            lastMouseState = mouseState;

            #endregion

            #region Maze Stuff
            switch (MasterMode)
            {

                case "Generate":

                    ReRender--;

                    if (ID == 0)
                    {

                        Program.MasterMaze.GenerateNext(ReRender < 0 ? true : false);

                    }

                    else
                    {

                        WallVerticesBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColorTexture), Program.MasterMaze.WallVertices.Length, BufferUsage.WriteOnly);
                        WallIndicesBuffer = new IndexBuffer(GraphicsDevice, IndexElementSize.ThirtyTwoBits, Program.MasterMaze.WallIndices.Length, BufferUsage.WriteOnly);

                        WallVerticesBuffer.SetData<VertexPositionColorTexture>(Program.MasterMaze.WallVertices);
                        WallIndicesBuffer.SetData<int>(Program.MasterMaze.WallIndices);

                        if (DoOnce)
                        {

                            DoOnce = false;

                            Vertices = new VertexPositionNormalTexture[Program.MasterMaze.Vertices.Length];
                            Indices = new int[Program.MasterMaze.Indices.Length];

                            Vertices = (VertexPositionNormalTexture[])Program.MasterMaze.Vertices.Clone();
                            Indices = (int[])Program.MasterMaze.Indices.Clone();

                            VerticesBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionNormalTexture), Vertices.Length, BufferUsage.WriteOnly);
                            IndicesBuffer = new IndexBuffer(GraphicsDevice, IndexElementSize.ThirtyTwoBits, Indices.Length, BufferUsage.WriteOnly);

                            VerticesBuffer.SetData<VertexPositionNormalTexture>(Vertices);
                            IndicesBuffer.SetData<int>(Indices);

                        }

                    }

                    if (ReRender < 0)
                        ReRender += Stagger;

                    break;

                case "Ready":

                    #region Maze Stuff

                    if (SelfMode == "Ready")
                    {



                    }

                    #region Transition
                    else if (SelfMode == "Transition")
                    {

                        if (SmallTimer > 0)
                        {

                            if (ID == 0)
                            {

                                MovableCamera = new FreeCamera(new Vector3(1375 - (180 - SmallTimer) * 1320 / 180f,
                                    3750 - (180 - SmallTimer) * 3695 / 180f,
                                    1375 - (180 - SmallTimer) * 1320 / 180f),
                                    MathHelper.ToRadians(-(180 - SmallTimer)),
                                    MathHelper.ToRadians(-90 + (180 - SmallTimer) * .5f), GraphicsDevice);


                            }

                            else
                            {

                                MovableCamera = new FreeCamera(new Vector3(1375 - (180 - SmallTimer) * 1320 / 180f,
                                    3750 - (180 - SmallTimer) * 3695 / 180f,
                                    1375 + (180 - SmallTimer) * 1320 / 180f),
                                    MathHelper.ToRadians(0),
                                    MathHelper.ToRadians(-90 + (180 - SmallTimer) * .5f), GraphicsDevice);

                            }

                            MovableCamera.Update();

                            SmallTimer--;

                        }

                        else
                        {

                            SelfMode = "Guess";

                            PathStack = new Stack<Cell>();

                            if (ID == 0)
                                PathStack.Push(Program.MasterMaze.Cells[0, 0]);
                            else
                                PathStack.Push(Program.MasterMaze.Cells[0, (int)Program.MasterMaze.Dimensions.Y - 1]);

                            Current = PathStack.Peek();

                            CurrentDirection = ID == 0 ? "Down" : "Up";
                            MovableCamera = new FreeCamera(new Vector3(Current.Location.X * 110 + 55, 55, Current.Location.Y * 110 + 55), MovableCamera.Yaw, 0, GraphicsDevice);

                        }

                    }
                    #endregion

                    #region Guess
                    else if (SelfMode == "Guess")
                    {

                        Next = Program.MasterMaze.Guess(PathStack, ID, Next);

                        Vector3 Difference = new Vector3(Current.Location.X * 110 + 55, 55, Current.Location.Y * 110 + 55) - MovableCamera.Position;

                        ShouldMove = new Vector2(Difference.X + 110, Difference.Z + 110);
                        HasMoved = new Vector2(110) - ShouldMove;
                        Start = MovableCamera.Position;
                        Start.Y = 55;

                        RotateBy = NeedsRotate();

                        if (RotateBy == 0)
                        {

                            //No rotation needed go to moving immediately
                            SelfMode = "Move";
                            SmallTimer = 0;

                        }

                        else
                        {

                            SelfMode = "Rotate";
                            SmallTimer = Math.Abs(RotateBy / 90) * 30;

                            if (RotateBy > 90)
                            {



                            }

                        }

                    }
                    #endregion

                    #region Move and Rotate
                    else if (SelfMode == "Move")
                    {

                        if (HasMoved.X < 110 && HasMoved.Y < 110)
                        {

                            MovableCamera.Position += (new Vector3(Next.Location.X * 110 + 55, 55, Next.Location.Y * 110 + 55) - Start) / 30f;
                            HasMoved += new Vector2(Math.Abs(Vector3.Forward.X * 110 / 30f), Math.Abs(Vector3.Forward.Z * 110 / 30f));

                        }

                        else
                        {

                            SelfMode = "Guess";
                            Current = Next;

                        }

                    }

                    else if (SelfMode == "Rotate")
                    {

                        if (SmallTimer > 0)
                        {

                            MovableCamera.Rotate(MathHelper.ToRadians(RotateBy) / (Math.Abs(RotateBy / 90) * 30), 0);
                            SmallTimer--;

                        }

                        else
                        {

                            SelfMode = "Move";
                            SmallTimer = 0;
                            SetDirection();

                        }

                    }
                    #endregion

                    else
                    {

                        SmallTimer = 180;
                        SelfMode = "Transition";

                    }

                    #endregion

                    #region Overview

                    if (Delay <= 0)
                    {

                        if (CameraState == "Waiting")
                        {

                            CameraState = "Up";
                            Delay = 60;

                        }

                        else if (CameraState == "Up")
                        {

                            CameraState = "Observe";
                            Delay = 600;

                        }

                        else if (CameraState == "Observe")
                        {

                            CameraState = "Down";
                            Delay = 60;

                        }

                        else if (CameraState == "Down")
                        {

                            CameraState = "Waiting";
                            Delay = Program.Rand.Next(1500, 3000);

                        }

                    }

                    else
                    {

                        if (CameraState == "Up")
                        {

                            MovableCamera.Rotate(0, -MathHelper.ToRadians(90) / 60);
                            MovableCamera.Position += new Vector3(0, 500 / 60, 0);

                        }

                        else if (CameraState == "Down")
                        {

                            MovableCamera.Rotate(0, MathHelper.ToRadians(90) / 60);
                            MovableCamera.Position -= new Vector3(0, 500 / 60, 0);

                        }

                        Delay--;

                    }

                    #endregion

                    break;

            }

            #endregion

            base.Update(gameTime);

        }

        public int NeedsRotate()
        {

            if (CurrentDirection == "Down" && Next.Location.Y > Current.Location.Y)
                return 0;
            else if (CurrentDirection == "Up" && Next.Location.Y < Current.Location.Y)
                return 0;
            else if (CurrentDirection == "Right" && Next.Location.X > Current.Location.X)
                return 0;
            else if (CurrentDirection == "Left" && Next.Location.X < Current.Location.X)
                return 0;

            if (CurrentDirection == "Down")
            {

                if (Next.Location.Y < Current.Location.Y)
                    return 180;
                else if (Next.Location.X > Current.Location.X)
                    return 90;
                else
                    return -90;

            }

            else if (CurrentDirection == "Up")
            {

                if (Next.Location.Y > Current.Location.Y)
                    return 180;
                else if (Next.Location.X > Current.Location.X)
                    return -90;
                else
                    return 90;

            }

            else if (CurrentDirection == "Right")
            {

                if (Next.Location.X < Current.Location.X)
                    return 180;
                else if (Next.Location.Y > Current.Location.Y)
                    return -90;
                else
                    return 90;

            }

            else if (CurrentDirection == "Left")
            {

                if (Next.Location.X > Current.Location.X)
                    return 180;
                else if (Next.Location.Y > Current.Location.Y)
                    return 90;
                else
                    return -90;

            }

            return 1;

        }

        public void SetDirection()
        {

            if (Next.Location.Y > Current.Location.Y)
                CurrentDirection = "Down";
            else if (Next.Location.Y < Current.Location.Y)
                CurrentDirection = "Up";
            else if (Next.Location.X > Current.Location.X)
                CurrentDirection = "Right";
            else
                CurrentDirection = "Left";
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Black);//Clear the screen and setup render state

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rs;

            GraphicsDevice.BlendState = BlendState.NonPremultiplied;//Allow transparency
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            if (Program.MasterMaze.State != "None")
            {

                if (ID == 0)
                {

                    GraphicsDevice.SetVertexBuffer(Program.MasterMaze.VerticesBuffer);
                    GraphicsDevice.Indices = Program.MasterMaze.IndicesBuffer;

                }

                else if (!DoOnce)
                {

                    GraphicsDevice.SetVertexBuffer(VerticesBuffer);
                    GraphicsDevice.Indices = IndicesBuffer;


                }

                MazeEffect.Parameters["View"].SetValue(MovableCamera.View);
                MazeEffect.Parameters["Projection"].SetValue(MovableCamera.Projection);
                MazeEffect.Parameters["MazeTexture"].SetValue(MazeTexture);

                MazeEffect.Techniques[0].Passes[0].Apply();

                GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, Program.MasterMaze.VerticesBuffer.VertexCount, 0, Program.MasterMaze.IndicesBuffer.IndexCount / 3);


                if (ID == 0)
                {

                    GraphicsDevice.SetVertexBuffer(Program.MasterMaze.WallVerticesBuffer);
                    GraphicsDevice.Indices = Program.MasterMaze.WallIndicesBuffer;

                }

                else
                {

                    GraphicsDevice.SetVertexBuffer(WallVerticesBuffer);
                    GraphicsDevice.Indices = WallIndicesBuffer;

                }

                BEffect.View = MovableCamera.View;
                BEffect.Projection = MovableCamera.Projection;
                BEffect.Texture = WallTiling;
                BEffect.TextureEnabled = true;

                BEffect.Techniques[0].Passes[0].Apply();

                GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, Program.MasterMaze.WallVerticesBuffer.VertexCount, 0, Program.MasterMaze.WallIndicesBuffer.IndexCount / 3); ;

            }

            base.Draw(gameTime);
        }

        public static Vector3 Rotate(float radiansX, float radiansY, float radiansZ, Vector3 vector)
        {
            Matrix rotationX = Matrix.CreateRotationX(radiansX);
            Matrix rotationY = Matrix.CreateRotationY(radiansY);
            Matrix rotationZ = Matrix.CreateRotationZ(radiansZ);
            return Vector3.Transform(Vector3.Transform(Vector3.Transform(vector, rotationX), rotationY), rotationZ);
        }

    }
}
