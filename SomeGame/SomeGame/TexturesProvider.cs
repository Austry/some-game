using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SomeGame
{
    public class TexturesProvider
    {
        public Texture2D cursorSprite { get; set; }
        public Texture2D heroSprite { get; set; }
        public Texture2D hookFirstElementSprite { get; set; }
        public Texture2D hookElementSprite { get; set; }
        public TexturesProvider(PudgeWarsGame game) 
        {
            
            cursorSprite = game.Content.Load<Texture2D>("cursor");
            heroSprite = game.Content.Load<Texture2D>("hero-sprite");
            hookElementSprite = game.Content.Load<Texture2D>("hook_part");
            hookFirstElementSprite = game.Content.Load<Texture2D>("hook_first");

        }
    }
}
