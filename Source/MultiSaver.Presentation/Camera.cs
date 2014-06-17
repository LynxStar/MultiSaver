using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiSaver.Presentation
{
    public abstract class Camera
    {
        Matrix view;
        Matrix projection;

        public Matrix Projection
        {
            get { return projection; }
            protected set
            {
                projection = value;
                generateFrustum();
            }
        }

        public Matrix View
        {
            get { return view; }
            protected set
            {
                view = value;
                generateFrustum();
            }
        }

        public BoundingFrustum Frustum { get; private set; }

        protected GraphicsDevice GraphicsDevice { get; set; }

        public Camera(GraphicsDevice graphicsDevice, bool Perspective)
        {
            this.GraphicsDevice = graphicsDevice;

            if (Perspective)
                generatePerspectiveProjectionMatrix(MathHelper.PiOver4);
            else
                generateOrthogonalProjectionMatrix(MathHelper.PiOver4);
        }

        private void generatePerspectiveProjectionMatrix(float FieldOfView)
        {
            PresentationParameters pp = GraphicsDevice.PresentationParameters;

            float aspectRatio = (float)pp.BackBufferWidth /
                (float)pp.BackBufferHeight;

            this.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), aspectRatio, 1.0f, 10000.0f);
            //this.Projection = Matrix.CreateOrthographic(pp.BackBufferWidth, pp.BackBufferHeight, 1.0f, 10000.0f);
        }

        private void generateOrthogonalProjectionMatrix(float FieldOfView)
        {
            PresentationParameters pp = GraphicsDevice.PresentationParameters;

            float aspectRatio = (float)pp.BackBufferWidth /
                (float)pp.BackBufferHeight;

            //this.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), aspectRatio, 1.0f, 10000.0f);
            this.Projection = Matrix.CreateOrthographic(pp.BackBufferWidth, pp.BackBufferHeight, 1.0f, 10000.0f);
        }

        public virtual void Update()
        {
        }

        private void generateFrustum()
        {
            Matrix viewProjection = View * Projection;
            Frustum = new BoundingFrustum(viewProjection);
        }

        public bool BoundingVolumeIsInView(BoundingSphere sphere)
        {
            return (Frustum.Contains(sphere) != ContainmentType.Disjoint);
        }

        public bool BoundingVolumeIsInView(BoundingBox box)
        {
            return (Frustum.Contains(box) != ContainmentType.Disjoint);
        }
    }
}
