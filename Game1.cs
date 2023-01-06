using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Birdle
{
    public class Game1 : Game
    {
        // This is a tuple
        private static (int width, int height) t_SCREEN_DIMENSIONS = (1920, 1080);
        // Used for the solution preview
        private static Rectangle rec_SOLUTIONRECTANGLE = new Rectangle(1450, 140, 400, 400);
        private static Rectangle rec_SOLUTIONRECTANGLEBORDER = new Rectangle(rec_SOLUTIONRECTANGLE.X - 5, rec_SOLUTIONRECTANGLE.Y - 5, 410, 410);
        // Determines the dimensions of the grid (not scale)
        private const int i_GRIDSIZE = 3;

        private GraphicsDeviceManager m_Graphics;
        private SpriteBatch m_SpriteBatch;
        private Grid m_Grid;

        // Font for drawing info on screen
        private SpriteFont m_Font;
        // Timer
        private float f_TimeElapsed;
        private Vector2 vec_TimerLocation;
        private Vector2 vec_MoveLocation;


        public Game1()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            m_Grid = new Grid(t_SCREEN_DIMENSIONS, i_GRIDSIZE);

            m_Graphics.PreferredBackBufferWidth = t_SCREEN_DIMENSIONS.width;
            m_Graphics.PreferredBackBufferHeight = t_SCREEN_DIMENSIONS.height;

            f_TimeElapsed = 0f;
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

            // These need to be here as they need font to be loaded
            vec_TimerLocation = new Vector2(60, 140);
            vec_MoveLocation = new Vector2(60, vec_TimerLocation.Y + m_Font.LineSpacing);
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
            // Draws grid
            m_Grid.Render(m_SpriteBatch);
            // Draws solution
            m_SpriteBatch.Draw(m_Grid.m_GridBorderTexture, rec_SOLUTIONRECTANGLEBORDER, null, Color.White);
            m_SpriteBatch.Draw(m_Grid.m_Image, rec_SOLUTIONRECTANGLE, null, Color.White);
            // Draws timer
            m_SpriteBatch.DrawString(m_Font, $"Time: {(int)f_TimeElapsed}", vec_TimerLocation, Color.Black);
            // Draws moves made
            m_SpriteBatch.DrawString(m_Font, $"Moves: {m_Grid.i_MovesMade}", vec_MoveLocation, Color.Black);

            m_SpriteBatch.End();


            base.Draw(gameTime);
        }
    }
}