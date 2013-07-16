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
        private float hookSpeedCoef = 3;
        // Используеться в цикле управления частями крюка
        private int currentHookPartsNumber = 0;
        private Vector2 tempPartSpeedVector;
        
        public enum HOOK_STATE { 
            FLYING_FORVARD, FLYING_BACK , READY_FOR_FROW, NOT_READY_FOR_FROW
        }

        public Hook(Game game,MainHero hero):base(game) {
            currentHero = hero;

            for (int i = 0; i < hookPartsArray.Length; i++)
            {
                if (i == 0)
                {
                    hookPartsArray[i] = new HookPart(game, PudgeWarsGame.textureProvider.hookFirstElementSprite,
                                                                         currentHero.heroPositionVector, this);
                }
                else {
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
                    hookPartsArray[i].currentPartState = HookPart.PART_STATE.FLYING_FORVARD;
                    hookPartsArray[i].SetRorationAngle(rotationAngl);
                    hookPartsArray[i].SetSpeedVector(tempPartSpeedVector);
                    //Проверка на коллизию с предыдушим звеном цепи
                    if (currentHookPartsNumber < hookPartsArray.Length)
                    {
                        Rectangle tempPartRect = hookPartsArray[currentHookPartsNumber].boxingRectangle;
                        Rectangle tempPreviousPartRect = hookPartsArray[currentHookPartsNumber - 1].boxingRectangle;
                        if(!tempPartRect.Intersects(tempPreviousPartRect)){
                            currentHookPartsNumber++;
                        }
                    }
                }
                //Проверка на вылет последнего звена цепи.
                if(!hookPartsArray[hookPartsArray.Length-1].boxingRectangle.Intersects(currentHero.boxingRectangle)){
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
           
           
            
        }

        /// <summary>
        /// Determines if there is overlap of the non-transparent pixels
        /// between two sprites.
        /// </summary>
        /// <param name="rectangleA">Bounding rectangle of the first sprite</param>
        /// <param name="dataA">Pixel data of the first sprite</param>
        /// <param name="rectangleB">Bouding rectangle of the second sprite</param>
        /// <param name="dataB">Pixel data of the second sprite</param>
        /// <returns>True if non-transparent pixels overlap; false otherwise</returns>
        public  bool IntersectPixels(Rectangle rectangleA, Color[] dataA,
                                           Rectangle rectangleB, Color[] dataB)
        {
            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - rectangleA.Left) +
                                         (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }

            // No intersection found
            return false;
        }
    }
}
