using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SomeGame.Actors
{
    class Hook: GameComponent 
    {   
        //
        public MainHero currentHero;
        public HOOK_STATE currentHookState = HOOK_STATE.NOT_READY_FOR_FROW;
        //
        private HookPart[] hookPartsArray = new HookPart[8];
        private Vector2 frowTargetCoordsVector = new Vector2();
        private double rotationAngl = 0;
        public float hookSpeedCoef = 3;
        private Game game;
        // Используеться в цикле управления частями крюка
        private int currentHookPartsNumber = 0;
        private Vector2 tempPartSpeedVector;
        
        public enum HOOK_STATE { 
            FLYING_FORVARD, FLYING_BACK , READY_FOR_FROW, NOT_READY_FOR_FROW
        }

        public Hook(Game game,MainHero hero):base(game) {
            currentHero = hero;
            this.game = game;
            InitHookParts(game);
            


        }
        private void InitHookParts(Game game) {
            for (int i = 0; i < hookPartsArray.Length; i++)
            {
                if (i == 0)
                {
                    hookPartsArray[i] = new HookPart(game, PudgeWarsGame.textureProvider.hookFirstElementSprite,
                                                                         currentHero.heroPositionVector, this);
                }
                else
                {
                    hookPartsArray[i] = new HookPart(game, PudgeWarsGame.textureProvider.hookElementSprite,
                                                                         currentHero.heroPositionVector, this);
                }
                game.Components.Add(hookPartsArray[i]);
            }
        
        }
        public override void Update(GameTime gameTime)
        {
            if (hookPartsArray[0].boxingRectangle.Intersects(currentHero.boxingRectangle))
                currentHero.isHited = true;
            else
                currentHero.isHited = false;

            if (currentHookState == HOOK_STATE.FLYING_FORVARD)
            {
                if (currentHookPartsNumber == 0)
                    currentHookPartsNumber++;
                for (int i = 0; i < currentHookPartsNumber; i++)
                {
                    
                    hookPartsArray[i].SetRendered(true);
                    hookPartsArray[i].SetRorationAngle(rotationAngl);
                    hookPartsArray[i].SetSpeedVector(tempPartSpeedVector);
                   // hookPartsArray[i].SetBoxRorationAngle(rotationAngl);
                    hookPartsArray[i].currentPartState = HookPart.PART_STATE.FLYING_FORVARD;
                    //Проверка на коллизию с предыдушим звеном цепи
                    if (currentHookPartsNumber < hookPartsArray.Length)
                    {
                        RotatedRectangle tempPartRect = hookPartsArray[currentHookPartsNumber].boxingRectangle;
                        RotatedRectangle tempPreviousPartRect = hookPartsArray[currentHookPartsNumber - 1].boxingRectangle;
                        if(!tempPartRect.Intersects(tempPreviousPartRect)){
                            currentHookPartsNumber++;
                        }
                    }
                }
                //Проверка на вылет последнего звена цепи.
                if (!currentHero.boxingRectangle.Intersects(hookPartsArray[hookPartsArray.Length - 1].boxingRectangle))
                {
                    currentHookState = HOOK_STATE.FLYING_BACK;
                    foreach (HookPart part in hookPartsArray)
                    {
                        part.currentPartState = HookPart.PART_STATE.FLYING_BACK;
                    }
                }
            }
            
            //---- Обработка полета назад
            if (currentHookState == HOOK_STATE.FLYING_BACK) {
                
                if (hookPartsArray[0].currentPartState == HookPart.PART_STATE.STAND)
                {
                    currentHookState = HOOK_STATE.NOT_READY_FOR_FROW;
                    currentHookPartsNumber = 0;
                }
            }
        }

        public void Frow(MouseState currentMouseState ) {
            this.currentHookState = HOOK_STATE.FLYING_FORVARD;
            this.frowTargetCoordsVector.X = currentMouseState.X;
            this.frowTargetCoordsVector.Y = currentMouseState.Y;
            //Расчет угла между прямой y=x и прямой из точек положения героя и целевой точки броска.
            // Расчет ведеться для поворота крюка на нужный угол.
            
            double a = frowTargetCoordsVector.Y - currentHero.heroPositionVector.Y;
            double b = frowTargetCoordsVector.X - currentHero.heroPositionVector.X;
           
            
            double k2 =  - a / b;
            double tgF = (1 - k2) / (1 + k2);
            // Из-за того,что функция арккотангенса изменяеться от -п\2 до п\2 при попадании функции в третью и четвертую
            // четверти необходимо добовить п для того,чтобы угол менялся во всех 360 градусах.
            if (frowTargetCoordsVector.Y - currentHero.heroPositionVector.Y >= frowTargetCoordsVector.X - currentHero.heroPositionVector.X)
            {
                rotationAngl = Math.Atan(tgF)  + 5 * Math.PI / 4;
            }
            else
            {
                rotationAngl = Math.Atan(tgF) + Math.PI / 4;
            }
            // Расчет вектора скорости для первой части хука
           double tempSpeedX =     Math.Sin(rotationAngl) * hookSpeedCoef;
           double tempSpeedY = - Math.Cos(rotationAngl) * hookSpeedCoef;
           tempPartSpeedVector = new Vector2((float)tempSpeedX, (float)tempSpeedY);
          // InitHookParts(game);
           
            
        }

    }
}
