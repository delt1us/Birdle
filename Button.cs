using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Birdle
{
    // Button object that can be pressed
    internal class Button
    {
        private Rectangle rec_ButtonHitbox;
        private Texture2D m_Texture;

        public bool b_Pressed;
        public Button(Vector2 location, Texture2D texture, int width, int height, Color color)
        {
            rec_ButtonHitbox = new Rectangle((int)location.X, (int)location.Y, width, height);
            m_Texture = texture;

            b_Pressed = false; 
        }
        // Called every frame
        public void Update()
        {
            
        }

        // Renders button on screen
        public void Render(SpriteBatch m_SpriteBatch)
        {
            // TODO 
            m_SpriteBatch.Draw(m_Texture, rec_ButtonHitbox, null, Color.Black);
        }

        // Checks if given Point is in the button
        public void CheckCollisionWithMouse(Point mousePos)
        {
            if (rec_ButtonHitbox.Contains(mousePos))
            {
                b_Pressed = true;
            }
        }
    }
}
