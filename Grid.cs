using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;

namespace Birdle
{
    internal class Grid
    {
        private static int i_GridSideLength = 900;
        public Texture2D m_Image;
        public Texture2D m_GridBorderTexture;
        private Vector2 vec_GridBorderCoordinates;
        private List<Tile> l_Tiles;
        private Point m_GridCoordinates;

        // Size in images (e.g. 3x3, 5x5 etc.) 
        private int i_Size;

        public Grid((int width, int height) t_ScreenDimensions, int size)
        {
            i_Size = size;
            l_Tiles = new List<Tile>();

            // Centers the grid
            m_GridCoordinates = new Point((int)((t_ScreenDimensions.width - i_GridSideLength) / 2f), (int)((t_ScreenDimensions.height - i_GridSideLength) / 2f));
            vec_GridBorderCoordinates = new Vector2((float)m_GridCoordinates.X - 10, (float)m_GridCoordinates.Y - 10);

            CreateTiles();
        }

        public void Update()
        {
        }

        // This method 
        public void Render(SpriteBatch m_SpriteBatch)
        {
            // TODO: Render all tiles in the grid
            // Draws the grid
            m_SpriteBatch.Draw(m_GridBorderTexture, vec_GridBorderCoordinates, Color.White);

            foreach (Tile tile in l_Tiles)
            {
                tile.Render(m_SpriteBatch, m_Image);
            }

        }

        private void CreateTiles()
        {
            int i_TileSize = (int)(i_GridSideLength / i_Size);
            // Each row of the grid
            for (int row = 0; row < i_Size; row++)
            {
                // Each column of the grid
                for (int column = 0; column < i_Size; column++)
                {
                    Rectangle CurrentTileRectangle = new Rectangle(m_GridCoordinates.X + i_TileSize * column, m_GridCoordinates.Y + i_TileSize * row, i_TileSize, i_TileSize);
                    Rectangle CurrentTileRegionRectangle = new Rectangle(column * i_TileSize, row * i_TileSize, i_TileSize, i_TileSize);
                    Tile tile = new Tile(CurrentTileRectangle, CurrentTileRegionRectangle, true);
                    l_Tiles.Add(tile);
                }
            }
        }

        private void ShuffleTiles()
        {

        }

    }
}