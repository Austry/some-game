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
        public RotatedRectangle boxingRectangle; 
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
            
            this.heroSprite = heroSprite;

            heroHook = new Hook(game,this);
            game.Components.Add(heroHook);
            boxingRectangle = new RotatedRectangle(heroPositionVector,heroSprite.Width / 6, heroSprite.Height / 4,0);
        }

    
   

        public override void Update(GameTime gameTime) 
        {   
            mouseState = Mouse.GetState();
            //Расчет прямоугольника, олицетворяющего границы героя

            
            
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
                heroSpeedVector.X =  targetCoordinats.X - heroPositionVector.X;
                heroSpeedVector.Y = targetCoordinats.Y - heroPositionVector.Y;

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
                heroPositionVector += heroSpeedVector;
                boxingRectangle.ChangePosition(heroSpeedVector);
                
                if (distanceToTargetLocation <= 3)
                {
                    heroSpeedVector.X = 0;
                    heroSpeedVector.Y = 0;
                    isMoving = false;
                }
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
                spriteBatch.Draw(heroSprite, heroPositionVector, new Rectangle(0, 0, 30, 62), Color.White);
            }
            else {
                spriteBatch.Draw(heroSprite, heroPositionVector, new Rectangle(frame * 32, (directionFlag - 1) * 64, 31, 63), Color.White);
                
            }

        }
        public bool IsMoving() {
            return isMoving;
        }
    }
}
