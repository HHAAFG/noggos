using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    
    public int x;
    public int y;
    public int[] gridX;
    public int[] gridY;
    public bool hasMoved = false;
    public bool white;
    public int speed = 1;
    public Material normalMaterial;
    public Material selectedMaterial;
    public Renderer thisRenderer;
    public void Movement()
    {
        ClearArray(gridX, gridY);


        switch (speed)
        {
            case 1:
                SpeedOne();
                break;
            case 2:
                SpeedTwo();
                break;
            case 3:
                SpeedThree();
                break;
            case 4:
                BishopRules();
                break;
            case 5:
                RookRules();
                break;
            case 6:
                QueenRules();
                break;
            case 7:
                KingRules();
                break;
            default:
                break;

        }
    }



    public void SpeedOne()
    {

        //one speed grid
        gridX = new int[] { 1, 0, 1, -1, 0, 0, 1, -1 ,-1 };
        gridY = new int[] { 1, 1, 0,  0,-1, 0,-1, -1 , 1};
    }
    public void SpeedTwo()
    {
        //Two speed grid
        gridX = new int[] { 0, 1, 0,-1 ,1,2,-1,-2, 0, 1, 0, -1};
        gridY = new int[] { 2, 1, 1, 1 ,0,0, 0, 0,-2,-1,-1,-1};

    }
    public void SpeedThree()
    {
        //Three speed grid
        gridX = new int[] { 0, 1 , 2, -1, -2, 0, 1, 2, -1, -2, 0, 1, 2, -1, -2, 0 , 0,  0,  0, 1, 1, 1,  1,  1, 2, 2, 2,  2,  2, -1, -2, -1, -2 };
        gridY = new int[] { 0, 0 , 0,  0,  0, 1, 1, 1,  1,  1, 2, 2, 2,  2,  2, 1 , 2, -1, -2, 0, 1, 2, -1, -2, 0, 1, 2, -1, -2, -1, -1, -2, -2 };

    }
    public void BishopRules()
    {
        gridX = new int[] { 1, 2, 3, 4, 5, 6, 7, -1, -2, -3, -4, -5, -6, -7, 1, 2, 3, 4, 5, 6, 7, -1, -2, -3, -4, -5, -6, -7 };
        gridY = new int[] { 1, 2, 3, 4, 5, 6, 7, -1, -2, -3, -4, -5, -6, -7, -1, -2, -3, -4, -5, -6, -7, 1, 2, 3, 4, 5, 6, 7 };
    }
    public void RookRules()
    {
        gridX = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 3, 4, 5, 6, 7, -1, -2, -3, -4, -5, -6, -7, 0, 0, 0, 0, 0, 0, 0 };
        gridY = new int[] { 1, 2, 3, 4, 5, 6, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -2, -3, -4, -5, -6, -7 };
    }
    public void QueenRules()
    {
        gridX = new int[] { 1, 2, 3, 4, 5, 6, 7, -1, -2, -3, -4, -5, -6, -7, 1, 2, 3, 4, 5, 6, 7, -1, -2, -3, -4, -5, -6, -7,
         0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 3, 4, 5, 6, 7, -1, -2, -3, -4, -5, -6, -7, 0, 0, 0, 0, 0, 0, 0};
        gridY = new int[] { 1, 2, 3, 4, 5, 6, 7, -1, -2, -3, -4, -5, -6, -7, -1, -2, -3, -4, -5, -6, -7, 1, 2, 3, 4, 5, 6, 7,
         1, 2, 3, 4, 5, 6, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -2, -3, -4, -5, -6, -7 };
    }
    public void KingRules()
    {
        gridX = new int[] { 0, 0, 1, -1, 1, -1, 1, -1 };
        gridY = new int[] { 1, -1, 0, 0, 1, -1, -1, 1 };
    }
    void ClearArray(int[] gridX, int[] gridY)
    {
        Array.Clear(gridX, 0, gridX.Length);
        Array.Clear(gridY, 0, gridY.Length);
    }

}
