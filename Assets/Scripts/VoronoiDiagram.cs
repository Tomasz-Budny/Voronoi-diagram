using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoronoiDiagram : MonoBehaviour
{
    public int textureHeight, textureWidth;
    public Region[] regions;
    public float minkowskiDistanceOfOrder;
    public void GenerateVoronoiDiagram()
    {
        Color[] pixelColor = new Color[textureWidth * textureHeight];
        for (int x = 0; x < textureWidth; x++)
        {
            for (int y = 0; y < textureHeight; y++)
            {
                int index = x * textureWidth + y;
                pixelColor[index] = regions[GetClosestSetIndex(new Vector2Int(x, y))].colour;
            }
        }

        pixelColor = AddSetsOnColorMap(pixelColor);
        Texture2D texture =  GetTextureFromColorArray(pixelColor);
        TextureDisplay txtDisp = FindObjectOfType<TextureDisplay>();
        txtDisp.DrawTexture(texture);
    }

    int GetClosestSetIndex(Vector2Int currentPosition)
    {
        int index = 0;
        int i = 0;
        float smallestDistance = float.MaxValue;

        foreach(Region regionSet in regions)
        {
            int x1 = currentPosition.x;
            int y1 = currentPosition.y;
            int x2 = regionSet.set.x;
            int y2 = regionSet.set.y;
            float p = minkowskiDistanceOfOrder;
            float minkowskiDistance = Mathf.Pow(Mathf.Pow(Mathf.Abs(x2 - x1), p) + Mathf.Pow(Mathf.Abs(y2 - y1), p), 1f / p);

            if(minkowskiDistance < smallestDistance)
            {
                index = i;
                smallestDistance = minkowskiDistance;
            }
            i++;
        }
        return index;
    }

    Color[] AddSetsOnColorMap(Color[] pixelColor)
    {
        foreach(Region region in regions)
        {
            int xSet = region.set.x;
            int ySet = region.set.y;
            if(ItIsWithinTheBoundariesOfPixelMAp(xSet, ySet))
            {
                int pointSize = textureWidth / 200;
                int x = xSet - pointSize < 0 ? 0 : xSet - pointSize;
                int y = ySet - pointSize < 0 ? 0 : ySet - pointSize;

                while(x < textureWidth && x - xSet < pointSize)
                {
                    while(y < textureHeight && y - ySet < pointSize)
                    {
                        pixelColor[x * textureWidth + y] = new Color(0f, 0f, 0f);
                        y++;
                    }
                    x++;
                    y = ySet - pointSize < 0 ? 0 : ySet - pointSize;
                }
            }
        }
        return pixelColor;
    }

    bool ItIsWithinTheBoundariesOfPixelMAp(int x, int y)
    {
        if (x < 0 || x > textureWidth)
            return false;
        if (y < 0 || y > textureHeight)
            return false;
        return true;
    }

    Texture2D GetTextureFromColorArray(Color[] pixelColor)
    {
        Texture2D texture = new Texture2D(textureWidth, textureHeight);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(pixelColor);
        texture.Apply();
        return texture;
    }
}

[System.Serializable]
public class Region
{
    public Vector2Int set;
    public Color colour;
}
