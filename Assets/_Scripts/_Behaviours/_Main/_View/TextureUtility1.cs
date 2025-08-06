using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureUtility1
{
    public static Texture2D GetSubTexture(Texture2D texture, Vector2 offset, Vector2 size)
    {
        // Check to make sure that the offset and size are valid.
        if (offset.x < 0 || offset.y < 0 || offset.x + size.x > texture.width || offset.y + size.y > texture.height)
        {
            return null;
        }

        // Create a new texture to hold the subtexture.
        Texture2D subTexture = new Texture2D((int)size.x, (int)size.y);

        // Copy the pixels from the original texture to the new texture.
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                subTexture.SetPixel(x, y, texture.GetPixel(x + (int)offset.x, y + (int)offset.y));
            }
        }

        // Return the subtexture.
        return subTexture;
    }
}
