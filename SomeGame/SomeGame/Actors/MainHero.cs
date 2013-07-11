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
        //----- Переменная,определяющая направление движения
        private int directionFlag;
        //----- Переменные для расчета времени показа одного фрейма
        private float totalTime, timeForFrame = 0.5f;
        //----- индикатор фрейма
        private int frame;

        

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
                
                //Определение направления движения
                if(targetCoordinats.X >= heroPositionVector.X && targetCoordinats.Y >= heroPositionVector.Y ){
                    directionFlag = 1;
                }
                if (targetCoordinats.X <= heroPositionVector.X && targetCoordinats.Y >= heroPositionVector.Y)
                {
                    directionFlag = 2;
                }
                if (targetCoordinats.X < heroPositionVector.X && targetCoordinats.Y < heroPositionVector.Y)
                {
                    directionFlag = 3;
                }
                if (targetCoordinats.X >= heroPositionVector.X && targetCoordinats.Y <= heroPositionVector.Y)
                {
                    directionFlag = 4;
                }



                //Определения вектора скорости
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
                if (distanceToTargetLocation <= 3)
                    isMoving = false;

            }
        
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime) 
        {
            
            totalTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (totalTime > timeForFrame)
            {
                frame++;

                frame = frame % 5;

                totalTime -= timeForFrame;
            }

            if (!isMoving)
            {
                spriteBatch.Draw(PudgeWarsGame.heroSprite, Vector2.Subtract(heroPositionVector, indentVector), new Rectangle(0, 0, 30, 62), Color.White);
            }
            else {
                spriteBatch.Draw(PudgeWarsGame.heroSprite, Vector2.Subtract(heroPositionVector, indentVector), new Rectangle(frame*32,(directionFlag-1)*64, 31, 63), Color.White);
            }
        }
    }
}
