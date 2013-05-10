using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SomeGame.Actors
{
    class MainHero
    {
        

        //----Координаты героя
        public Vector2 heroPositionVector = new Vector2(0,0);
        //----Положение мыщи
        private MouseState mouseState = Mouse.GetState();
        //----Координаты точки, куда нужно переместится
        private Vector2 targetCoordinats = new Vector2(0,0);
        //----Вектор скорости героя
        private Vector2 heroSpeedVector = new Vector2();
        //----- Триггер движения
        private bool isMoving = false;
        //----- Дистанция до указанной точки перемещения от героя
        private float distanceToTargetLocation;
        //---- Отступ координат героя из-за параметров спрайта
        private Vector2 indentVector= new Vector2(15,15);

        public MainHero() {
            heroPositionVector = Vector2.Add(heroPositionVector,indentVector);
            
        }


        public void Update() 
        {   
            mouseState = Mouse.GetState();
            
            //------Обработка нажатия ПКМ
            if (mouseState.RightButton.Equals(ButtonState.Pressed))
            {
                targetCoordinats.X = mouseState.X;
                targetCoordinats.Y = mouseState.Y;
                isMoving = true;
                
                heroSpeedVector.X = heroPositionVector.X - targetCoordinats.X;
                heroSpeedVector.Y = heroPositionVector.Y - targetCoordinats.Y;

                heroSpeedVector.Normalize();
                heroSpeedVector = Vector2.Multiply(heroSpeedVector,3);
                
            }

            //-------------Обработка движения--------
            if(isMoving)
            {
                distanceToTargetLocation = Vector2.Distance(heroPositionVector,targetCoordinats);
                heroPositionVector = Vector2.Subtract(heroPositionVector,heroSpeedVector);
                if (distanceToTargetLocation <= 2)
                    isMoving = false;

            }
        
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            spriteBatch.Draw(PudgeWarsGame.heroSprite,Vector2.Subtract(heroPositionVector,indentVector),Color.White);
        }
    }
}
