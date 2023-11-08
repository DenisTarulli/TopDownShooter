using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldScrolling : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector2Int playerTilePosition;
    [SerializeField] private float tileSize = 20f;
    private Vector2Int currentTilePosition = new Vector2Int(0, 0);
    private Vector2Int onTileGridPlayerPosition;
    private GameObject[,] terrainTiles;

    [SerializeField] private int terrainTileHorizontalCount;
    [SerializeField] private int terrainTileVerticalCount;

    [SerializeField] private int fieldOfVisionHeight = 3; 
    [SerializeField] private int fieldOfVisionWidth = 3; 

    private void Awake()
    {
        terrainTiles = new GameObject[terrainTileHorizontalCount, terrainTileVerticalCount];
    }

    private void Update()
    {
        playerTilePosition.x = (int)(playerTransform.position.x / tileSize);
        playerTilePosition.y = (int)(playerTransform.position.y / tileSize);

        playerTilePosition.x -= playerTransform.position.x < 0 ? 1 : 0;
        playerTilePosition.y -= playerTransform.position.y < 0 ? 1 : 0;

        if (currentTilePosition != playerTilePosition)
        {
            currentTilePosition = playerTilePosition;

            onTileGridPlayerPosition.x = CalculatePositionOnAxis(onTileGridPlayerPosition.x, true);
            onTileGridPlayerPosition.x = CalculatePositionOnAxis(onTileGridPlayerPosition.y, false);
            UpdateTilesOnScreen();
        }
    }

    private void UpdateTilesOnScreen()
    {
        for (int povX = -(fieldOfVisionWidth/2); povX <= fieldOfVisionWidth/2; povX++)
        {
            for (int povY = -(fieldOfVisionHeight / 2); povY <= fieldOfVisionHeight/2; povY++)
            {
                int tileToUpdateX = CalculatePositionOnAxis(playerTilePosition.x + povX, true);
                int tileToUpdateY = CalculatePositionOnAxis(playerTilePosition.x + povY, false);

                GameObject tile = terrainTiles[tileToUpdateX, tileToUpdateY];
                tile.transform.position = CalculatePosition(playerTilePosition.x + povX, playerTilePosition.y + povY);
            }
        }
    }

    private Vector3 CalculatePosition(int x, int y)
    {
        return new Vector3(x * tileSize, y * tileSize, 0f);
    }

    private int CalculatePositionOnAxis(float currentValue, bool horizontal)
    {
        if (horizontal)
        {
            if (currentValue >= 0)
                currentValue %= terrainTileHorizontalCount;
            else
            {
                currentValue += 1;
                currentValue = terrainTileHorizontalCount - 1 + currentValue % terrainTileHorizontalCount;
            }
        }
        else
        {
            if (currentValue >= 0)
                currentValue %= terrainTileVerticalCount;
            else
            {
                currentValue += 1;
                currentValue = terrainTileHorizontalCount - 1 + currentValue % terrainTileVerticalCount;
            }
        }

        return (int)currentValue;
    }

    public void AddTile(GameObject tileGameObject, Vector2Int tilePosition)
    {
        terrainTiles[tilePosition.x, tilePosition.y] = tileGameObject;
    }
}
