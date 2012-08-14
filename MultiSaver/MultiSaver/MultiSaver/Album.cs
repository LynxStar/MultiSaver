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
using System.IO;

namespace MultiSaver
{

    public class Album : Microsoft.Xna.Framework.Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        bool TransitionIn = true;

        BasicEffect BE;

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

        public String Location;

        public Rectangle Bounds = Rectangle.Empty;

        public bool IsLeft = false;

        public Vector2 CurrentSize = new Vector2();

        public IntPtr Handler;

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
            
            if (Handler.ToInt32() != 0)
            {

                
                User32.SetWindowPos((uint)this.Window.Handle, 0, 0, 0, 0, 0, 0);

                System.Drawing.Rectangle ParentRect;
                User32.GetClientRect(Handler, out ParentRect);

                User32.SetParent(this.Window.Handle, Handler);
                User32.SetWindowLong(this.Window.Handle, -16, new IntPtr(User32.GetWindowLong(this.Window.Handle, -16) | 0x40000000));

                PresentationParameters PP = GraphicsDevice.PresentationParameters;

                PP.DeviceWindowHandle = Handler;

                GraphicsDevice.Reset(PP);

                //graphics.PreferredBackBufferHeight = ParentRect.Height;
                //graphics.PreferredBackBufferWidth = ParentRect.Width;
                //graphics.ApplyChanges();

            }

            else
            {

                graphics.PreferredBackBufferHeight = Bounds.Height;
                graphics.PreferredBackBufferWidth = Bounds.Width;
                graphics.ApplyChanges();
                User32.SetWindowPos((uint)this.Window.Handle, 0, Bounds.X, Bounds.Y, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, 0);

            }
            
            
            

            spriteBatch = new SpriteBatch(GraphicsDevice);

            BE = new BasicEffect(GraphicsDevice);

            PanInEffect = Content.Load<Effect>("PanIn");
            PanOutEffect = Content.Load<Effect>("PanOut");

            FadeInEffect = Content.Load<Effect>("FadeIn");
            FadeOutEffect = Content.Load<Effect>("FadeOut");

            SpiralInEffect = Content.Load<Effect>("SpiralIn");
            SpiralOutEffect = Content.Load<Effect>("SpiralOut");

            String[] Files = File.Exists(Location) ? Directory.GetFiles(Location) : new string[0];

            foreach (String Picture in Files)
            {

                if (Picture.Contains(".png") || Picture.Contains(".PNG") || Picture.Contains(".jpg") || Picture.Contains(".JPG"))
                {
                    FileStream FS = new FileStream(Picture, FileMode.Open);
                    Images.Add(Texture2D.FromStream(GraphicsDevice, FS));
                    FS.Close();

                }

            }

            if (Images.Count == 0)
            {

                Images.Add(Content.Load<Texture2D>("Cell"));
                Images.Add(Content.Load<Texture2D>("Japan"));
                Images.Add(Content.Load<Texture2D>("Galaxy"));
                Images.Add(Content.Load<Texture2D>("AbstractBars"));

            }

            Mode = RandomMode();
            Mode = "Spiral";
            //GeneratePrimatives(Program.Rand.Next(10, 25), Images[0]);
            ImageIndex = new Random().Next(0, Images.Count);
            GeneratePrimatives(10, Images[ImageIndex]);

            MovableCamera = new FreeCamera(new Vector3(Bounds.Width / 2, Bounds.Height / 2, 10), 0, 0, GraphicsDevice, false);
            MovableCamera.Update();

        }

        public void GeneratePrimatives(int Tiling, Texture2D Image)
        {

            CurrentImage = Image;

            CurrentSize = new Vector2(Image.Width, Image.Height);

            float WPer = Image.Width * 100f / Bounds.Width;
            float HPer = Image.Height * 100f / Bounds.Height;

            if (WPer > 100 || HPer > 100)
            {

                if (WPer > HPer)
                    CurrentSize *= 100f / WPer;
                else
                    CurrentSize *= 100f / HPer;

            }

            else if (WPer < 65 || HPer < 65)
            {

                WPer = Image.Width * 100f / Bounds.Width * 100f / 65;
                HPer = Image.Height * 100f / Bounds.Height * 100f / 65;

                if (WPer > HPer)
                    CurrentSize *= 100f / WPer;
                else
                    CurrentSize *= 100f / HPer;

            }

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

                    int Length = (int)CurrentSize.X / Tiling;
                    int LengthX = (int)CurrentSize.X / Tiling;
                    int LengthY = (int)CurrentSize.Y / Tiling;
                    float UVLength = 1f / Tiling;

                    Vector2 Coords = new Vector2(((Bounds.Width - CurrentSize.X) / 2) + x * LengthX, Bounds.Height - ((Bounds.Height - CurrentSize.Y) / 2) - y * LengthY);
                    Vector2 UVCoords = new Vector2((float)x * UVLength, (float)y * UVLength);

                    #region Pan

                    if (Mode == "Pan")
                    {

                        float Offset = Rand.Next(Bounds.Width - Convert.ToInt32(Coords.X), Bounds.Width);
                        float Speed = Convert.ToSingle(3 + Rand.NextDouble() * 5);

                        Time = (int)(Time < (Coords.X + Offset) / (Speed) ? (Coords.X + Offset) / (Speed) : Time);

                        Vertices[i++] = new VertexPositionNormalTexture(new Vector3(Coords.X, Coords.Y, 0), new Vector3(Offset, Coords.X, Speed), new Vector2(UVCoords.X, UVCoords.Y));
                        Vertices[i++] = new VertexPositionNormalTexture(new Vector3(Coords.X + LengthX, Coords.Y, 0), new Vector3(Offset, Coords.X + LengthX, Speed), new Vector2(UVCoords.X + UVLength, UVCoords.Y));
                        Vertices[i++] = new VertexPositionNormalTexture(new Vector3(Coords.X, Coords.Y - LengthY, 0), new Vector3(Offset, Coords.X, Speed), new Vector2(UVCoords.X, UVCoords.Y + UVLength));
                        Vertices[i++] = new VertexPositionNormalTexture(new Vector3(Coords.X + LengthX, Coords.Y - LengthY, 0), new Vector3(Offset, Coords.X + LengthX, Speed), new Vector2(UVCoords.X + UVLength, UVCoords.Y + UVLength));

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
                        Vertices[i++] = new VertexPositionNormalTexture(new Vector3(Coords.X + LengthX, Coords.Y, 0), N1, new Vector2(UVCoords.X + UVLength, UVCoords.Y));
                        Vertices[i++] = new VertexPositionNormalTexture(new Vector3(Coords.X, Coords.Y - LengthY, 0), N1, new Vector2(UVCoords.X, UVCoords.Y + UVLength));
                        Vertices[i++] = new VertexPositionNormalTexture(new Vector3(Coords.X + LengthX, Coords.Y - LengthY, 0), N1, new Vector2(UVCoords.X + UVLength, UVCoords.Y + UVLength));

                    }

                    #endregion

                    #region Spiral

                    else if  (Mode == "Spiral")
                    {

                        int Delay = Rand.Next(0, 10) * 30;

                        Vector3 N1 = new Vector3(Delay, Coords.X - Bounds.Width / 2, Coords.Y - Bounds.Height / 2);
                        Vector3 N2 = new Vector3(Delay, Coords.X + LengthX - Bounds.Width / 2, Coords.Y - Bounds.Height / 2);
                        Vector3 N3 = new Vector3(Delay, Coords.X - Bounds.Width / 2, Coords.Y - LengthY - Bounds.Height / 2);
                        Vector3 N4 = new Vector3(Delay, Coords.X + LengthX - Bounds.Width / 2, Coords.Y - LengthY - Bounds.Height / 2);

                        //Time = (int)(Delay + 255 / Speed > Time ? Delay + 255 / Speed : Time);

                        Vertices[i++] = new VertexPositionNormalTexture(new Vector3(Coords.X, Coords.Y, 0), N1, new Vector2(UVCoords.X, UVCoords.Y));
                        Vertices[i++] = new VertexPositionNormalTexture(new Vector3(Coords.X + LengthX, Coords.Y, 0), N2, new Vector2(UVCoords.X + UVLength, UVCoords.Y));
                        Vertices[i++] = new VertexPositionNormalTexture(new Vector3(Coords.X, Coords.Y - LengthY, 0), N3, new Vector2(UVCoords.X, UVCoords.Y + UVLength));
                        Vertices[i++] = new VertexPositionNormalTexture(new Vector3(Coords.X + LengthX, Coords.Y - LengthY, 0), N4, new Vector2(UVCoords.X + UVLength, UVCoords.Y + UVLength));

                    }

                    #endregion

                    #region Test

                    else
                    {

                        Vertices[i++] = new VertexPositionNormalTexture(new Vector3(Coords.X, Coords.Y, 0), Vector3.Zero, new Vector2(UVCoords.X, UVCoords.Y));
                        Vertices[i++] = new VertexPositionNormalTexture(new Vector3(Coords.X + LengthX, Coords.Y, 0), Vector3.Zero, new Vector2(UVCoords.X + UVLength, UVCoords.Y));
                        Vertices[i++] = new VertexPositionNormalTexture(new Vector3(Coords.X, Coords.Y - LengthY, 0), Vector3.Zero, new Vector2(UVCoords.X, UVCoords.Y + UVLength));
                        Vertices[i++] = new VertexPositionNormalTexture(new Vector3(Coords.X + LengthX, Coords.Y - LengthY, 0), Vector3.Zero, new Vector2(UVCoords.X + UVLength, UVCoords.Y + UVLength));

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

                    ImageIndex = new Random().Next(0, Images.Count);

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
                else if (Mode == "Spiral")
                {

                    Used = SpiralInEffect;
                    Used.Parameters["Origin"].SetValue(new Vector2(Bounds.Width / 2, Bounds.Height / 2));

                }

            }

            else
            {
                if (Mode == "Pan")
                    Used = PanOutEffect;
                else if (Mode == "Fade")
                    Used = FadeOutEffect;
                else if (Mode == "Spiral")
                {

                    Used = SpiralOutEffect;
                    Used.Parameters["Origin"].SetValue(new Vector2(Bounds.Width / 2, Bounds.Height / 2));

                }
                
            }



            if (Mode != "Test")
            {

                Used.Parameters["View"].SetValue(MovableCamera.View);
                Used.Parameters["Projection"].SetValue(MovableCamera.Projection);
                Used.Parameters["PhotoTexture"].SetValue(CurrentImage);
                Used.Parameters["Time"].SetValue(Time);
                Used.Techniques[0].Passes[0].Apply();

            }

            else
            {

                BE.View = MovableCamera.View;
                BE.Projection = MovableCamera.Projection;
                BE.TextureEnabled = true;
                BE.Texture = CurrentImage;
                BE.Techniques[0].Passes[0].Apply();

            }

            

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