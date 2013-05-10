using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SomeGame.Actors;

namespace SomeGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PudgeWarsGame : Microsoft.Xna.Framework.Game
    {
        static public GraphicsDeviceManager graphics;
        static public SpriteBatch spriteBatch;
        //--------------Textures-----------
        static public Texture2D cursorSprite;
        static public Texture2D heroSprite;
        //---------------------------------

        MouseController mouseConroller;
        MainHero mainHero;

        public PudgeWarsGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
        }

     
        protected override void Initialize()
        {
            Window.Title = "PudgeWars";

            mouseConroller = new MouseController();
            mainHero = new MainHero();
            
            base.Initialize();
        }

      
        protected override void LoadContent()
        {
          
            spriteBatch = new SpriteBatch(GraphicsDevice);
            cursorSprite = Content.Load<Texture2D>("cursor");
            heroSprite = Content.Load<Texture2D>("hero");
        
        }

  
        protected override void UnloadContent()
        {
           
        }

       
        protected override void Update(GameTime gameTime)
        {
          

            mouseConroller.Update();

            mainHero.Update();
            
            base.Update(gameTime);
        }

   
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();

            mainHero.Draw(spriteBatch);

            mouseConroller.Draw(spriteBatch);
            
            
            
            
            spriteBatch.End();
           

            base.Draw(gameTime);
        }
    }
}
