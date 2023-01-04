using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Birdle
{
    public class Game1 : Game
    {
        // This is a tuple
        private static (int width, int height) t_SCREEN_DIMENSIONS = (1920, 1080);
        private const int i_GRIDSIZE = 3;

        private GraphicsDeviceManager m_Graphics;
        private SpriteBatch m_SpriteBatch;
        private Grid m_Grid;

        private SpriteFont m_Font;
        private float f_TimeElapsed;
        private Vector2 vec_TimerLocation;

        public Game1()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            m_Grid = new Grid(t_SCREEN_DIMENSIONS, i_GRIDSIZE);

            m_Graphics.PreferredBackBufferWidth = t_SCREEN_DIMENSIONS.width;
            m_Graphics.PreferredBackBufferHeight = t_SCREEN_DIMENSIONS.height;

            f_TimeElapsed = 0f;
            vec_TimerLocation = new Vector2(60, 140);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);

            m_Grid.m_GridBorderTexture = Content.Load<Texture2D>("Graphics/grid-border");
            m_Grid.m_Image = Content.Load<Texture2D>("Graphics/numbered-grid");

            m_Font = Content.Load<SpriteFont>("Fonts/Default");
        }

        protected override void Update(GameTime gameTime)
        {
            // Add time since last frame to the timer
            f_TimeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            m_Grid.Update();    

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            m_SpriteBatch.Begin();
            // Render things here
            m_Grid.Render(m_SpriteBatch);
            m_SpriteBatch.DrawString(m_Font, $"Time: {(int)f_TimeElapsed}", vec_TimerLocation, Color.Black);

            m_SpriteBatch.End();


            base.Draw(gameTime);
        }
    }
}