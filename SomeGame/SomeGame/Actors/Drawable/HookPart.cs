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
        public RotatedRectangle boxingRectangle;
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
        private Vector2 partFirstSpeedVector = new Vector2(0,0);
        
        public PART_STATE currentPartState;

        public enum PART_STATE { 
            STAND,FLYING_FORVARD, FLYING_BACK
        }



        public HookPart(Game game,Texture2D hookPartSprite, Vector2 partPositionVector,Hook currentHook):base(game) {
            this.currentHook = currentHook;
            partSprite = hookPartSprite;
            partCenterCoordsVector = new Vector2(partSprite.Width/2, partSprite.Height/2);
            currentPartState = PART_STATE.STAND;
            colorData = new Color[partSprite.Width * partSprite.Height];
            partSprite.GetData(colorData);
            boxingRectangle = new RotatedRectangle(partPositionVector + partCenterCoordsVector,partSprite.Width,partSprite.Height,0);
        }

        public override void Update(GameTime gameTime) 
        {
          
            
            switch (currentPartState)
            {
                
                case PART_STATE.FLYING_FORVARD:
                    if (currentHook.currentHero.IsMoving())
                    {
                        Vector2 tempVect = currentHook.currentHero.heroPositionVector - boxingRectangle.positionVector;
                        tempVect.Normalize();
                        partSpeedVector = partFirstSpeedVector + tempVect;
                    }
                    else
                    {
                        partSpeedVector = partFirstSpeedVector;
                    }
                    boxingRectangle.ChangePosition(partSpeedVector);

                    break;
                case PART_STATE.FLYING_BACK:
                    partSpeedVector= currentHook.currentHero.heroPositionVector - boxingRectangle.positionVector;
                    partSpeedVector.Normalize();
                    partSpeedVector = Vector2.Multiply(partSpeedVector,currentHook.hookSpeedCoef);

                    boxingRectangle.ChangePosition(partSpeedVector);
                    
                    if (boxingRectangle.Intersects(currentHook.currentHero.boxingRectangle))
                    {
                        isRendered = false;
                        currentPartState = PART_STATE.STAND;
                        rotationAngle = 0;
                        boxingRectangle.positionVector = currentHook.currentHero.heroPositionVector + partCenterCoordsVector;
                        
                    }
                    break;
                case PART_STATE.STAND:
                    boxingRectangle.ChangePosition(currentHook.currentHero.heroSpeedVector);
                   

                    break;
            }

        }

        public override void Draw(GameTime gameTime) 
        {
            spriteBatch = (SpriteBatch) Game.Services.GetService(typeof(SpriteBatch));
            if (isRendered)
            {
                spriteBatch.Draw(partSprite, new Vector2(boxingRectangle.X + (boxingRectangle.Width / 2), boxingRectangle.Y + (boxingRectangle.Height / 2)), null, Color.White, boxingRectangle.Rotation, partCenterCoordsVector, 1.0f, SpriteEffects.None, 0);
            }
        }

        public void SetRorationAngle(double angle){
            this.rotationAngle = angle;
            this.boxingRectangle.Rotation=(float)angle;
        }
        
        public void SetRendered(bool isRendered) {
            this.isRendered = isRendered;
        }
        public void SetSpeedVector(Vector2 speedVector) {
            this.partFirstSpeedVector = speedVector;
            
        }
        public bool IsRendered() { 
            return isRendered;
        }
       
    }

}
