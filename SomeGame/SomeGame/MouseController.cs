﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SomeGame
{
    class MouseController
    {
        public Vector2 mousePositionVector = new Vector2(0, 0);

        private MouseState mouseState;





        public void Update()
        {
            
            mouseState = Mouse.GetState();

            mousePositionVector.X = mouseState.X;
            mousePositionVector.Y = mouseState.Y;

            // --------------------------- Обработка зоны движения мыши-------
            if (mousePositionVector.X <= 0)
                mousePositionVector.X = 0;
            if (mousePositionVector.Y <= 0)
                mousePositionVector.Y = 0;
            if (mousePositionVector.X >= Game1.graphics.GraphicsDevice.Viewport.Width)
                mousePositionVector.X = Game1.graphics.GraphicsDevice.Viewport.Width;
            if (mousePositionVector.Y >= Game1.graphics.GraphicsDevice.Viewport.Height)
                mousePositionVector.Y = Game1.graphics.GraphicsDevice.Viewport.Height;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.cursorSprite, mousePositionVector, Color.White);
        }
    }
}