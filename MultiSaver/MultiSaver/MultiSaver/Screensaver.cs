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

    public class Screensaver : Microsoft.Xna.Framework.Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Effect SlideShowEffect;

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
        int TransitionTime;

        public Screensaver()
        {

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = 900;
            graphics.PreferredBackBufferWidth = 1440;

        }

        protected override void Initialize()
        {

            base.Initialize();

        }

        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);

            SlideShowEffect = Content.Load<Effect>("SlideShow");

            Images.Add(Content.Load<Texture2D>("Image1"));
            Images.Add(Content.Load<Texture2D>("Image2"));

            GeneratePrimatives(10, Images[0]);

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

                    //0-500 for x and y

                    int Length = 500 / Tiling;
                    float UVLength = 1f / Tiling;

                    Vector2 Coords = new Vector2(x * Length, 500 - y * Length);
                    Vector2 UVCoords = new Vector2((float)x * UVLength, (float)y * UVLength);

                    int Offset = Rand.Next(0, 500);
                    int Speed = Rand.Next(2, 5);

                    Time = (int)(Time < Offset / Speed ? (250 + Coords.X - Offset) / (Speed) : Time);

                    Vertices[i++] = new VertexPositionNormalTexture(new Vector3(250 + Coords.X, Coords.Y, 0), new Vector3(Offset, 250 + Coords.X, Speed), new Vector2(UVCoords.X, UVCoords.Y));
                    Vertices[i++] = new VertexPositionNormalTexture(new Vector3(250 + Coords.X + Length, Coords.Y, 0), new Vector3(Offset, 250 + Coords.X + Length, Speed), new Vector2(UVCoords.X + UVLength, UVCoords.Y));
                    Vertices[i++] = new VertexPositionNormalTexture(new Vector3(250 + Coords.X, Coords.Y - Length, 0), new Vector3(Offset, 250 + Coords.X, Speed), new Vector2(UVCoords.X, UVCoords.Y + UVLength));
                    Vertices[i++] = new VertexPositionNormalTexture(new Vector3(250 + Coords.X + Length, Coords.Y - Length, 0), new Vector3(Offset, 250 + Coords.X + Length, Speed), new Vector2(UVCoords.X + UVLength, UVCoords.Y + UVLength));

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

            TransitionTime = Time + 300;

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

            // Move 4 units per millisecond, independent of frame rate
            translation *= (float)(0.5 * gameTime.ElapsedGameTime.TotalMilliseconds);

            // Move the camera
            MovableCamera.Move(translation);
            MovableCamera.Update();

            Time++;

            if (Time > TransitionTime)
            {

                ImageIndex++;

                if (ImageIndex >= Images.Count)
                    ImageIndex = 0;

                GeneratePrimatives(10, Images[ImageIndex]);
                Time = 0;
                
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            GraphicsDevice.SetVertexBuffer(VerticesBuffer);
            GraphicsDevice.Indices = IndicesBuffer;

            SlideShowEffect.Parameters["View"].SetValue(MovableCamera.View);
            SlideShowEffect.Parameters["Projection"].SetValue(MovableCamera.Projection);
            SlideShowEffect.Parameters["PhotoTexture"].SetValue(CurrentImage);
            SlideShowEffect.Parameters["Time"].SetValue(Time);

            SlideShowEffect.Techniques[0].Passes[0].Apply();

            GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, VerticesBuffer.VertexCount, 0, IndicesBuffer.IndexCount / 3);

            base.Draw(gameTime);
        }

    }

}