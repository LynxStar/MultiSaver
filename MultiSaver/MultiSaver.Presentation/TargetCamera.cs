﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiSaver.Presentation
{
    public class TargetCamera : Camera
    {
        public Vector3 Position { get; set; }
        public Vector3 Target { get; set; }

        public TargetCamera(Vector3 Position, Vector3 Target,
            GraphicsDevice graphicsDevice)
            : base(graphicsDevice, true)
        {
            this.Position = Position;
            this.Target = Target;
        }

        public override void Update()
        {
            this.View = Matrix.CreateLookAt(Position, Target, Vector3.Up);
        }
    }
}
