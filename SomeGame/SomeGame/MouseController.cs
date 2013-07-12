using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SomeGame
{
    class MouseController : DrawableGameComponent
    {
        public Vector2 mousePositionVector = new Vector2(0, 0);

        private MouseState mouseState;
        private Texture2D cursorSprite;
        private SpriteBatch spriteBatch;

        public  MouseController(Game game,Texture2D cursorSprite):base(game) {
            this.cursorSprite = cursorSprite;
        }

       


        public  override void Update(GameTime gameTime)
        {
            
            mouseState = Mouse.GetState();

            mousePositionVector.X = mouseState.X;
            mousePositionVector.Y = mouseState.Y;

            // --------------------------- Обработка зоны движения мыши-------
            if (mousePositionVector.X <= 0)
                mousePositionVector.X = 0;
            if (mousePositionVector.Y <= 0)
                mousePositionVector.Y = 0;
            if (mousePositionVector.X >= PudgeWarsGame.graphics.GraphicsDevice.Viewport.Width)
                mousePositionVector.X = PudgeWarsGame.graphics.GraphicsDevice.Viewport.Width;
            if (mousePositionVector.Y >= PudgeWarsGame.graphics.GraphicsDevice.Viewport.Height)
                mousePositionVector.Y = PudgeWarsGame.graphics.GraphicsDevice.Viewport.Height;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            spriteBatch.Draw(cursorSprite, mousePositionVector, Color.White);
        }
    }
}
