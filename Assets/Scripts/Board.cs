using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board instance;
    public int[] gridX;
    public int[] gridY;
    public float UiScale => 2.5f;
    public Vector3[] uiGridX;
    public Vector3[] uiGridY;
    public GameObject[] pieces;
    public List<Piece> pieceLocations = new List<Piece>();

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        SetupGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetupGame()
    {
        for (int i = 0; i < 18; i++)
        {
            gridX[i] = 1 + i;
            gridY[i] = 1 + i;
            
        }
        int counter = 0;
        for (int x = 0; x < 18; x++)
        {
            if (counter == 0)
            {
                uiGridX[x] = new Vector3(0, 0, 0);

            }
            if (counter >= 1)
            {

                uiGridX[x] = new Vector3(gridX[x - 1] * UiScale, 0, 0);
            }

            counter++;
        }
        int yCounter = 0;
        for (int y = 0; y < 18; y++)
        {
            if (yCounter == 0)
            {
                uiGridY[y] = new Vector3(0, 0, 0);

            }
            if (yCounter >= 1)
            {

                uiGridY[y] = new Vector3(0, 0, gridY[y - 1] * UiScale);
            }
            yCounter++;
        }
           

        
    }

    public void GetLoc()
    {
        pieceLocations.Clear();
        foreach (var piece in pieces)
        {
            var p = piece.GetComponent<Piece>();

            pieceLocations.Add(p);
        }
    }
    public void CheckPositions()
    {

    }
}
