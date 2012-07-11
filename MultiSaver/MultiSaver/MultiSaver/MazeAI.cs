using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MultiSaver
{
    
    public class MazeAI : Microsoft.Xna.Framework.Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        Effect MazeEffect;

        String MasterMode = String.Empty;

        Texture2D MazeTexture;

        FreeCamera MovableCamera;

        int SmallTimer;

        public Rectangle Bounds = Rectangle.Empty;

        public bool IsLeft = false;

        public int ID;

        BasicEffect BEffect;

        public MazeAI()
        {
            
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";


        }
        
        protected override void Initialize()
        {

            this.InactiveSleepTime = TimeSpan.Zero;
            base.Initialize();

        }

        protected override void LoadContent()
        {

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

            }

            Program.MasterMaze.GenerateGroundPrimatives(GraphicsDevice);

            MovableCamera = new FreeCamera(new Vector3(1375, 3750, 1375), 0, MathHelper.ToRadians(-90), GraphicsDevice);
            MovableCamera.Update();

            MazeEffect = Content.Load<Effect>("MazeEffect");
            MazeTexture = Content.Load<Texture2D>("Zelda2");

            BEffect = new BasicEffect(GraphicsDevice);
            BEffect.VertexColorEnabled = true;
            BEffect.LightingEnabled = false;

        }

        protected override void UnloadContent()
        {



        }

        MouseState lastMouseState;

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

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

            switch (MasterMode)
            {

                case "Generate":

                    Program.MasterMaze.GenerateNext();

                    break;

            }

            base.Update(gameTime);

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

                //GraphicsDevice.SetVertexBuffer(Program.MasterMaze.VerticesBuffer);
                //GraphicsDevice.Indices = Program.MasterMaze.IndicesBuffer;

                //MazeEffect.Parameters["View"].SetValue(MovableCamera.View);
                //MazeEffect.Parameters["Projection"].SetValue(MovableCamera.Projection);
                //MazeEffect.Parameters["MazeTexture"].SetValue(MazeTexture);

                //MazeEffect.Techniques[0].Passes[0].Apply();

                //GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, Program.MasterMaze.VerticesBuffer.VertexCount, 0, Program.MasterMaze.IndicesBuffer.IndexCount / 3);


                GraphicsDevice.SetVertexBuffer(Program.MasterMaze.WallVerticesBuffer);
                GraphicsDevice.Indices = Program.MasterMaze.WallIndicesBuffer;

                BEffect.View = MovableCamera.View;
                BEffect.Projection = MovableCamera.Projection;

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