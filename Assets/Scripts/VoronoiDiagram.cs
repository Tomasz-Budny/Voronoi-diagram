using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoronoiDiagram : MonoBehaviour
{
    public int textureHeight, textureWidth;
    public int numberOfSets;
    public void GenerateVoronoiDiagram()
    {
        Color[] regionsColors = new Color[numberOfSets];
        Vector2Int[] sets = new Vector2Int[numberOfSets];
        for (int i = 0; i < numberOfSets; i++)
        {
            sets[i] = new Vector2Int(Random.Range(0, textureWidth), Random.Range(0, textureHeight));
            regionsColors[i] = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        }

        Color[] pixelColor = new Color[textureWidth * textureHeight];
        for (int x = 0; x < textureWidth; x++)
        {
            for (int y = 0; y < textureHeight; y++)
            {
                int index = x * textureWidth + y;
                pixelColor[index] = regionsColors[GetClosestSetIndex(new Vector2Int(x, y), sets)];
            }
        }
        Texture2D texture =  GetTextureFromColorArray(pixelColor);
        TextureDisplay txtDisp = FindObjectOfType<TextureDisplay>();
        txtDisp.DrawTexture(texture);
    }

    int GetClosestSetIndex(Vector2Int currentPosition, Vector2Int[] sets)
    {
        int index = 0;
        int i = 0;
        float smallestDistance = float.MaxValue;

        foreach(Vector2Int set in sets)
        {
            int x1 = currentPosition.x;
            int y1 = currentPosition.y;
            int x2 = set.x;
            int y2 = set.y;
            float distance = Mathf.Sqrt(Mathf.Pow(x2 - x1, 2) + Mathf.Pow(y2 - y1, 2));
            float manhattanDistance = Mathf.Abs(x1 - x2) + Mathf.Abs(y1 - y2);
            float dumbDistance = x2 * x1 + y2 * x1;
            int p = 15;
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
        //texture.filterMode = FilterMode.Point;
        texture.SetPixels(pixelColor);
        texture.Apply();
        return texture;

    }

}
