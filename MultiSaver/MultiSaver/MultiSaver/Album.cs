using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MultiSaver
{

    public class Album : Microsoft.Xna.Framework.Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        bool TransitionIn = true;

        Effect PanInEffect;
        Effect PanOutEffect;

        Effect FadeInEffect;
        Effect FadeOutEffect;

        Effect SpiralInEffect;
        Effect SpiralOutEffect;

        String Mode = String.Empty;

        public VertexPositionNormalTexture[] Vertices;
        public VertexBuffer VerticesBuffer;
        public int[] Indices;
        public IndexBuffer IndicesBuffer;

        List<Texture2D> Images = new List<Texture2D>();
        int ImageIndex;

        Texture2D CurrentImage;

        TargetCamera FixedCamera;
        FreeCamera MovableCamera;
        
        int Time;
        int TransitionInTime;
        int TransitionOutTime;

        public Rectangle Bounds = Rectangle.Empty;

        public bool IsLeft = false;

        public Album()
        {
            
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
                        
            //graphics.SynchronizeWithVerticalRetrace = false;

        }

        public static string RandomMode()
        {
            
            int Num = Program.Rand.Next(0, 3);

            if (Num == 0)
                return "Pan";
            else if (Num == 1)
                return "Fade";
            else
                return "Spiral";

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

            PanInEffect = Content.Load<Effect>("PanIn");
            PanOutEffect = Content.Load<Effect>("PanOut");

            FadeInEffect = Content.Load<Effect>("FadeIn");
            FadeOutEffect = Content.Load<Effect>("FadeOut");

            SpiralInEffect = Content.Load<Effect>("SpiralIn");
            SpiralOutEffect = Content.Load<Effect>("SpiralOut");

            if (IsLeft)
            {

                Images.Add(Content.Load<Texture2D>("Alex1"));
                Images.Add(Content.Load<Texture2D>("F22"));
                Images.Add(Content.Load<Texture2D>("Tim1"));
                Images.Add(Content.Load<Texture2D>("CPU1"));

            }

            else
            {

                Images.Add(Content.Load<Texture2D>("Megaman1"));
                Images.Add(Content.Load<Texture2D>("Megaman2"));
                Images.Add(Content.Load<Texture2D>("Megaman3"));
                Images.Add(Content.Load<Texture2D>("Megaman4"));
                Images.Add(Content.Load<Texture2D>("Megaman5"));

            }

            Mode = RandomMode();
            GeneratePrimatives(Program.Rand.Next(10, 25), Images[0]);

            FixedCamera = new TargetCamera(new Vector3(100, 50, 300), new Vector3(100, 50, 0), GraphicsDevice);
            FixedCamera.Update();

            MovableCamera = new FreeCamera(new Vector3(250, 225, 750), 0, 0, GraphicsDevice);
            MovableCamera.Update();

        }

        public void GeneratePrimatives(int Tiling, Texture2D Image)
        {

            CurrentImage = Image;

            Vertices = new VertexPositionNormalTexture[Tiling * Tiling * 4];
            Indices = new int[Tiling * Tiling * 6];

            VerticesBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionNormalTexture), Vertices.Length, BufferUsage.WriteOnly);
            IndicesBuffer = new IndexBuffer(GraphicsDevice, IndexElementSize.ThirtyTwoBits, Indices.Length, BufferUsage.WriteOnly);

            int i = 0;
            int j = 0;

            Random Rand = new Random();

            int Time = 0;

            for (int y = 0; y < Tiling; y++)
            {

                for (int x = 0; x < Tiling; x++)
                {
                    
                    int Length = 500 / Tiling;
                    float UVLength = 1f / Tiling;

                    Vector2 Coords = new Vector2(x * Length, 500 - y * Length);
                    Vector2 UVCoords = new Vector2((float)x * UVLength, (float)y * UVLength);

                    #region Pan

                    if (Mode == "Pan")
                    {

                        int Offset = Rand.Next(500, 1000);
                        int Speed = Rand.Next(2, 7);

                        Time = (int)(Time < (250 + Coords.X + Offset) / (Speed) ? (250 + Coords.X + Offset) / (Speed) : Time);

                        Vertices[i++] = new VertexPositionNormalTexture(new Vector3(250 + Coords.X, Coords.Y, 0), new Vector3(Offset, 250 + Coords.X, Speed), new Vector2(UVCoords.X, UVCoords.Y));
                        Vertices[i++] = new VertexPositionNormalTexture(new Vector3(250 + Coords.X + Length, Coords.Y, 0), new Vector3(Offset, 250 + Coords.X + Length, Speed), new Vector2(UVCoords.X + UVLength, UVCoords.Y));
                        Vertices[i++] = new VertexPositionNormalTexture(new Vector3(250 + Coords.X, Coords.Y - Length, 0), new Vector3(Offset, 250 + Coords.X, Speed), new Vector2(UVCoords.X, UVCoords.Y + UVLength));
                        Vertices[i++] = new VertexPositionNormalTexture(new Vector3(250 + Coords.X + Length, Coords.Y - Length, 0), new Vector3(Offset, 250 + Coords.X + Length, Speed), new Vector2(UVCoords.X + UVLength, UVCoords.Y + UVLength));

                    }

                    #endregion

                    #region Fade

                    else if (Mode == "Fade")
                    {

                        int Delay = Rand.Next(0, 360);
                        int Speed = Rand.Next(100, 1000);

                        Vector3 N1 = new Vector3(Delay, Speed / 100f, 0);

                        Time = (int)(Delay + 255 / Speed > Time ? Delay + 255 / Speed : Time);

                        Vertices[i++] = new VertexPositionNormalTexture(new Vector3(Coords.X, Coords.Y, 0), N1, new Vector2(UVCoords.X, UVCoords.Y));
                        Vertices[i++] = new VertexPositionNormalTexture(new Vector3(Coords.X + Length, Coords.Y, 0), N1, new Vector2(UVCoords.X + UVLength, UVCoords.Y));
                        Vertices[i++] = new VertexPositionNormalTexture(new Vector3(Coords.X, Coords.Y - Length, 0), N1, new Vector2(UVCoords.X, UVCoords.Y + UVLength));
                        Vertices[i++] = new VertexPositionNormalTexture(new Vector3(Coords.X + Length, Coords.Y - Length, 0), N1, new Vector2(UVCoords.X + UVLength, UVCoords.Y + UVLength));

                    }

                    #endregion

                    #region Spiral

                    else
                    {

                        int Delay = Rand.Next(0, 10) * 30;

                        Vector3 N1 = new Vector3(Delay, Coords.X - 250, Coords.Y - 250);
                        Vector3 N2 = new Vector3(Delay, Coords.X + Length - 250, Coords.Y - 250);
                        Vector3 N3 = new Vector3(Delay, Coords.X - 250, Coords.Y - Length - 250);
                        Vector3 N4 = new Vector3(Delay, Coords.X + Length - 250, Coords.Y - Length - 250);

                        //Time = (int)(Delay + 255 / Speed > Time ? Delay + 255 / Speed : Time);

                        Vertices[i++] = new VertexPositionNormalTexture(new Vector3(Coords.X, Coords.Y, 0), N1, new Vector2(UVCoords.X, UVCoords.Y));
                        Vertices[i++] = new VertexPositionNormalTexture(new Vector3(Coords.X + Length, Coords.Y, 0), N2, new Vector2(UVCoords.X + UVLength, UVCoords.Y));
                        Vertices[i++] = new VertexPositionNormalTexture(new Vector3(Coords.X, Coords.Y - Length, 0), N3, new Vector2(UVCoords.X, UVCoords.Y + UVLength));
                        Vertices[i++] = new VertexPositionNormalTexture(new Vector3(Coords.X + Length, Coords.Y - Length, 0), N4, new Vector2(UVCoords.X + UVLength, UVCoords.Y + UVLength));

                    }

                    #endregion

                    Indices[j++] = i - 4;
                    Indices[j++] = i - 3;
                    Indices[j++] = i - 2;

                    Indices[j++] = i - 2;
                    Indices[j++] = i  - 3;
                    Indices[j++] = i - 1;


                }

            }

            VerticesBuffer.SetData<VertexPositionNormalTexture>(Vertices);
            IndicesBuffer.SetData<int>(Indices);

            TransitionInTime = Time + 180;

            if (Mode == "Fade")
                TransitionInTime += 180;

            else if (Mode == "Spiral")
                TransitionInTime += 480;

            TransitionOutTime = Time + 120;

            if (Mode == "Spiral")
                TransitionOutTime += 360;

        }

        protected override void UnloadContent()
        {



        }
        
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState keyState = Keyboard.GetState();
            Vector3 translation = Vector3.Zero;

            // Determine in which direction to move the camera
            if (keyState.IsKeyDown(Keys.W)) translation += Vector3.Up;
            if (keyState.IsKeyDown(Keys.S)) translation += Vector3.Down;
            if (keyState.IsKeyDown(Keys.A)) translation += Vector3.Left;
            if (keyState.IsKeyDown(Keys.D)) translation += Vector3.Right;
            if (keyState.IsKeyDown(Keys.F12)) this.Exit();

            // Move 4 units per millisecond, independent of frame rate
            translation *= (float)(0.5 * gameTime.ElapsedGameTime.TotalMilliseconds);

            // Move the camera
            MovableCamera.Move(translation);
            MovableCamera.Update();

            Time++;

            if (Time > TransitionInTime)
            {

                if (TransitionIn)
                {

                    TransitionIn = false;
                    Time = 0;

                    if (Mode == "Pan")
                        TransitionInTime -= 480;
                    else if (Mode == "Fade")
                        TransitionInTime = TransitionOutTime;
                    else
                        TransitionInTime = TransitionOutTime;

                }

                else
                {

                    ImageIndex++;

                    if (ImageIndex >= Images.Count)
                        ImageIndex = 0;

                    Mode = RandomMode();
                    GeneratePrimatives(Program.Rand.Next(10, 25), Images[ImageIndex]);
                    Time = 0;
                    TransitionIn = true;

                }
                
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Black);//Clear the screen and setup render state
            GraphicsDevice.BlendState = BlendState.NonPremultiplied;//Allow transparency
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            GraphicsDevice.SetVertexBuffer(VerticesBuffer);
            GraphicsDevice.Indices = IndicesBuffer;

            Effect Used = PanInEffect;

            if (TransitionIn)
            {

                if (Mode == "Pan")
                    Used = PanInEffect;
                else if (Mode == "Fade")
                    Used = FadeInEffect;
                else
                    Used = SpiralInEffect;

            }

            else
            {
                if (Mode == "Pan")
                    Used = PanOutEffect;
                else if (Mode == "Fade")
                    Used = FadeOutEffect;
                else
                    Used = SpiralOutEffect;
                
            }

            Used.Parameters["View"].SetValue(MovableCamera.View);
            Used.Parameters["Projection"].SetValue(MovableCamera.Projection);
            Used.Parameters["PhotoTexture"].SetValue(CurrentImage);
            Used.Parameters["Time"].SetValue(Time);

            Used.Techniques[0].Passes[0].Apply();

            GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, VerticesBuffer.VertexCount, 0, IndicesBuffer.IndexCount / 3);

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