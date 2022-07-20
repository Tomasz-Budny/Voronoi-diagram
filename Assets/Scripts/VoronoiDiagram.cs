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

    Texture2D GetTextureFromColorArray(Color[] pixelColor)
    {
        Texture2D texture = new Texture2D(textureWidth, textureHeight);
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
