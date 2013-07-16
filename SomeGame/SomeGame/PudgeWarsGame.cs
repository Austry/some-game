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
        static public TexturesProvider textureProvider;
        //---------------------------------

        MouseController mouseConroller;
        MainHero mainHero;

        public PudgeWarsGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1024; //ширина экрана 
            graphics.PreferredBackBufferHeight = 768; //его высота   
            graphics.IsFullScreen = false; //включаем полноэкранный режим
        }

     
        protected override void Initialize()
        {
            Window.Title = "PudgeWars";
            textureProvider = new TexturesProvider(this);

            base.Initialize();
        }

      
        protected override void LoadContent()
        {
            
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            Services.AddService(typeof(SpriteBatch),spriteBatch);
            
           

            mouseConroller = new MouseController(this,textureProvider.cursorSprite);
            Components.Add(mouseConroller);

            mainHero = new MainHero(this, textureProvider.heroSprite);
            Components.Add(mainHero);

        
        }

  
        protected override void UnloadContent()
        {
           
        }

       
        protected override void Update(GameTime gameTime)
        {
          

        

            
            
            base.Update(gameTime);
        }

   
        protected override void Draw(GameTime gameTime)
        {
            
           // GraphicsDevice.Clear(Color.White);
            if (mainHero.isHited)
            {
                GraphicsDevice.Clear(Color.Red);
            }
            else
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
            }
            
            spriteBatch.Begin();


            base.Draw(gameTime);
            
            spriteBatch.End();
           

            
        }

        public TexturesProvider getTextureProvider() {
            return textureProvider;
        }
    }
}
