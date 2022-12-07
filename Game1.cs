using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Birdle
{
    public class Game1 : Game
    {
        // This is a tuple
        private static (int width, int height) t_ScreenDimensions = (1000, 1000);

        private GraphicsDeviceManager m_Graphics;
        private SpriteBatch m_SpriteBatch;
        private Grid m_Grid;

        public Game1()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            m_Grid = new Grid(t_ScreenDimensions, 3);

            m_Graphics.PreferredBackBufferWidth = t_ScreenDimensions.width;
            m_Graphics.PreferredBackBufferHeight = t_ScreenDimensions.height;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);

            m_Grid.m_GridBorderTexture = Content.Load<Texture2D>("Graphics/grid-border");
            m_Grid.m_Image = Content.Load<Texture2D>("Graphics/numbered-grid");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            m_SpriteBatch.Begin();
            // TODO: Add your drawing code here
            m_Grid.Render(m_SpriteBatch);
            m_SpriteBatch.End();


            base.Draw(gameTime);
        }
    }
}