using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Birdle
{
    // Button object that can be pressed
    internal class Button
    {
        private Rectangle rec_ButtonHitbox;
        private Texture2D m_Texture;

        private SpriteFont m_Font;
        private string str_Text;
        private Vector2 vec_TextLocation;

        public bool b_Pressed;
        public Button(Vector2 location, Texture2D texture, int width, int height, string text, SpriteFont font)
        {
            rec_ButtonHitbox = new Rectangle((int)location.X, (int)location.Y, width, height);
            m_Texture = texture;

            m_Font = font;
            str_Text = text;

            Vector2 vec_TextSize = m_Font.MeasureString(str_Text);
            // Position relative to
            float f_XPosition = (int)((width - vec_TextSize.X) / 2f);
            float f_YPosition = (int)((height - vec_TextSize.Y) / 2f);

            vec_TextLocation = new Vector2(f_XPosition + (float)rec_ButtonHitbox.X, f_YPosition + (float)rec_ButtonHitbox.Y);

            b_Pressed = false;
        }
        // Called every frame
        public void Update()
        {
            var m_MouseState = Mouse.GetState();

            // If clicking, point of click is in the box and the button is not already pressed
            if (m_MouseState.LeftButton == ButtonState.Pressed && rec_ButtonHitbox.Contains(new Point(m_MouseState.X, m_MouseState.Y)) && b_Pressed == false)
            {
                b_Pressed = true;
            }
        }

        // Renders button on screen
        public void Render(SpriteBatch m_SpriteBatch)
        {
            m_SpriteBatch.Draw(m_Texture, rec_ButtonHitbox, null, Color.White);
            m_SpriteBatch.DrawString(m_Font, str_Text, vec_TextLocation, Color.Black);
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
