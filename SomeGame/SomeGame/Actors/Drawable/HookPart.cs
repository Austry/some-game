using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace SomeGame.Actors
{
    class HookPart: DrawableGameComponent
    {
        //Бокс для определения столкновений
        public Rectangle boxingRectangle;
        //Вектор координат
        private Vector2 partPositionVector;
        // Вектор координат центра спрайта относительно левого верхнего угла
        private Vector2 partCenterCoordsVector;
        // Вектор скорости спрайта
        private Vector2 partSpeedVector = new Vector2(0,0);
        // Угол поворота спрайта, вектора и бокса относительно оси У
        private double rotationAngle;
        private Texture2D partSprite;
        public Color[] colorData;
        private SpriteBatch spriteBatch;
        // Ссылка на крюк, к которому принадлежит часть.
        private Hook currentHook;
        private bool isRendered = false;
        
        public PART_STATE currentPartState;

        public enum PART_STATE { 
            STAND,FLYING_FORVARD, FLYING_BACK
        }



        public HookPart(Game game,Texture2D hookPartSprite, Vector2 partPositionVector,Hook currentHook):base(game) {
            this.currentHook = currentHook;
            partSprite = hookPartSprite;
            this.partPositionVector = partPositionVector;
            partCenterCoordsVector = new Vector2(partSprite.Width/2, partSprite.Height/2);
            currentPartState = PART_STATE.STAND;
            colorData = new Color[partSprite.Width * partSprite.Height];
            partSprite.GetData(colorData);
            
        }

        public override void Update(GameTime gameTime) 
        {
          
            
            switch (currentPartState)
            {
                
                case PART_STATE.FLYING_FORVARD:
                    
                    partPositionVector += partSpeedVector;

                    break;
                case PART_STATE.FLYING_BACK:
                    if (boxingRectangle.Intersects(currentHook.currentHero.boxingRectangle))
                    {
                        isRendered = false;
                        currentPartState = PART_STATE.STAND;
                        partPositionVector += currentHook.currentHero.heroPositionVector;
                        rotationAngle = 0;
                    }
                    partPositionVector -= partSpeedVector;

                    break;
                case PART_STATE.STAND:
                    partPositionVector = currentHook.currentHero.heroPositionVector;
                    break;
            }


            // Трансформация прямоугольника, симвализируюшего первый элемент крюка
            Matrix partTransform =
                Matrix.CreateTranslation(new Vector3(new Vector2(partSprite.Width/2,partSprite.Height/2), 0.0f)) *
                Matrix.CreateRotationZ((float)rotationAngle) *
                Matrix.CreateTranslation(new Vector3(partPositionVector, 0.0f));
            
            // Расчет параметров нового прямоугольника
             boxingRectangle = CalculateBoundingRectangle(
                     new Rectangle(0, 0, partSprite.Width, partSprite.Height),
                     partTransform);
            
        }

        public override void Draw(GameTime gameTime) 
        {
            spriteBatch = (SpriteBatch) Game.Services.GetService(typeof(SpriteBatch));
            if (isRendered)
            {
               spriteBatch.Draw(partSprite, partPositionVector, null, Color.White, (float)rotationAngle,
                                            partCenterCoordsVector, 1.0f, SpriteEffects.None, 0.0f);
               //spriteBatch.Draw(partSprite, boxingRectangle, null, Color.White,0, partCenterCoordsVector, SpriteEffects.None, 0.0f);
            }
        }

        public void SetRorationAngle(double angle){
            this.rotationAngle = angle;
        }
        public void SetRendered(bool isRendered) {
            this.isRendered = isRendered;
        }
        public void SetSpeedVector(Vector2 speedVector) {
            this.partSpeedVector = speedVector;
            
        }
        private  Rectangle CalculateBoundingRectangle(Rectangle rectangle,
                                                           Matrix transform)
        {
            // Get all four corners in local space
            Vector2 leftTop = new Vector2(rectangle.Left, rectangle.Top);
            Vector2 rightTop = new Vector2(rectangle.Right, rectangle.Top);
            Vector2 leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
            Vector2 rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

            // Transform all four corners into work space
            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            // Find the minimum and maximum extents of the rectangle in world space
            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                                      Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                      Vector2.Max(leftBottom, rightBottom));

            // Return that as a rectangle
            return new Rectangle((int)partPositionVector.X, (int)partPositionVector.Y,
                                 (int)(max.X - min.X), (int)(max.Y - min.Y));
        }
    }

}
