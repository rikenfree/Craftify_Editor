using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelVisulizer : MonoBehaviour
{


    [Header("Grid Data")]
    public GameObject PixelPrefab;
    public int gridSizeX; // Number of cells in the X-axis
    public int gridSizeY; // Number of cells in the Y-axis
    public float cellSize; // Size of each cell
    public Vector3 offset;

    public Transform Parent;

    public GameObject[,] Pixelreference ;
    [Header("Texture Data")]
    public Texture2D capeTexture;

    private void Start()
    {
        
        CreateGrid();
    }


    [Button]
    private void CreateGrid()
    {
        Pixelreference = new GameObject[gridSizeX, gridSizeY];
        offset = new Vector3(-(gridSizeX / 2), -(gridSizeY / 2), 0) * cellSize;
        Parent = new GameObject("PixelGridParent").transform;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                // Calculate the position of each cell
                Vector3 cellPosition = new Vector3(x * cellSize, y * cellSize, 0f) + offset;

                // Create a cube as a visual representation of the cell
                GameObject cube = Instantiate(PixelPrefab, Parent);
                cube.transform.position = cellPosition;
                cube.transform.rotation = Quaternion.identity;
                Pixelreference[x, y] = cube;
                //cube.transform.localScale = new Vector3(cellSize, cellSize, 1f);

                // Attach the Cell script to the cube to store additional data if needed
                //  Cell cell = cube.AddComponent<Cell>();
                // cell.gridX = x;
                // cell.gridY = y;
            }
        }
    }


    [Button]
    private void ApplyTextureOnGrid()
    {
        if (capeTexture==null)
        {
            return;
        }

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Color pixelColor = capeTexture.GetPixel(x, y);
                if (pixelColor.a == 0)
                {
                    Pixelreference[x, y].GetComponent<MeshRenderer>().material.color = Color.white;

                }
                else { 
                Pixelreference[x, y].GetComponent<MeshRenderer>().material.color = pixelColor;
                }
            }
        }
    }

    [Button]
    public void DestoyGrid()
    {
        if (Parent != null)
        {
            DestroyImmediate(Parent.gameObject);
        }
    }
}
