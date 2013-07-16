using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SomeGame.Actors
{
    class MainHero : DrawableGameComponent
    {

        public bool isHited = false;
        public Rectangle boxingRectangle = new Rectangle(); 
        //----Координаты героя
        public Vector2 heroPositionVector = new Vector2(0,0);
        //----Положение мыщи
        private MouseState mouseState = Mouse.GetState();
        private KeyboardState keyboardState = Keyboard.GetState();
        //----Координаты точки, куда нужно переместится
        private Vector2 targetCoordinats = new Vector2(0,0);
        //----Вектор скорости героя
        public Vector2 heroSpeedVector = new Vector2();
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
        //----- Текстура героя
        Texture2D heroSprite;
        private Hook heroHook;

        SpriteBatch spriteBatch;

        public MainHero(Game game,Texture2D heroSprite):base(game) {
            heroPositionVector = Vector2.Add(heroPositionVector,indentVector);
            this.heroSprite = heroSprite;

            heroHook = new Hook(game,this);
            game.Components.Add(heroHook);
        }

    
   

        public override void Update(GameTime gameTime) 
        {   
            mouseState = Mouse.GetState();
            //Расчет прямоугольника, олицетворяющего границы героя
            boxingRectangle = new Rectangle((int)(heroPositionVector.X - indentVector.X), (int)(heroPositionVector.Y - indentVector.Y), 
                                                    heroSprite.Width/6, heroSprite.Height/4);

            //------Обработка нажатия ПКМ
            if (mouseState.RightButton.Equals(ButtonState.Pressed))
            {
                targetCoordinats.X = mouseState.X;
                targetCoordinats.Y = mouseState.Y;
                isMoving = true;
                
                //Определение направления движения
                if (targetCoordinats.Y - heroPositionVector.Y >= targetCoordinats.X - heroPositionVector.X 
                    && targetCoordinats.Y - heroPositionVector.Y >= -(targetCoordinats.X - heroPositionVector.X))
                {
                    directionFlag = 1;
                }
                if (targetCoordinats.Y - heroPositionVector.Y <= targetCoordinats.X - heroPositionVector.X
                    && targetCoordinats.Y - heroPositionVector.Y >= -(targetCoordinats.X - heroPositionVector.X))
                {
                    directionFlag = 4;
                }
                if (targetCoordinats.Y - heroPositionVector.Y < targetCoordinats.X - heroPositionVector.X
                   && targetCoordinats.Y - heroPositionVector.Y < -(targetCoordinats.X - heroPositionVector.X))
                {
                    directionFlag = 3;
                }

                if (targetCoordinats.Y - heroPositionVector.Y > targetCoordinats.X - heroPositionVector.X
                   && targetCoordinats.Y - heroPositionVector.Y < -(targetCoordinats.X - heroPositionVector.X))
                {
                    directionFlag = 2;
                }
                                
               

                //Определения вектора скорости
                heroSpeedVector.X = heroPositionVector.X - targetCoordinats.X;
                heroSpeedVector.Y = heroPositionVector.Y - targetCoordinats.Y;

                heroSpeedVector.Normalize();
                heroSpeedVector = Vector2.Multiply(heroSpeedVector,3);
                

                



            }
            //Обработка клавиши для броска крюка
            keyboardState = Keyboard.GetState();

            if (heroHook.currentHookState == Hook.HOOK_STATE.NOT_READY_FOR_FROW)
            {
                if (keyboardState.IsKeyDown(Keys.Q))
                {
                    heroHook.currentHookState = Hook.HOOK_STATE.READY_FOR_FROW;
                }
            }
            if (mouseState.LeftButton.Equals(ButtonState.Pressed) && heroHook.currentHookState == Hook.HOOK_STATE.READY_FOR_FROW)
            {
                heroHook.Frow(Mouse.GetState());

            }

            //-------------Обработка движения героя--------
            if(isMoving)
            {
                distanceToTargetLocation = Vector2.Distance(heroPositionVector,targetCoordinats);
                heroPositionVector = Vector2.Subtract(heroPositionVector,heroSpeedVector);
                if (distanceToTargetLocation <= 3)
                    isMoving = false;

            }
        
        }

        public override void Draw(GameTime gameTime) 
        {
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch)); 
            totalTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (totalTime > timeForFrame)
            {
                frame++;

                frame = frame % 5;

                totalTime -= timeForFrame;
            }

            if (!isMoving)
            {
                spriteBatch.Draw(heroSprite, Vector2.Subtract(heroPositionVector, indentVector), new Rectangle(0, 0, 30, 62), Color.White);
            }
            else {
                spriteBatch.Draw(heroSprite, Vector2.Subtract(heroPositionVector, indentVector), new Rectangle(frame * 32, (directionFlag - 1) * 64, 31, 63), Color.White);
            }

            

            

           
        }
    }
}
