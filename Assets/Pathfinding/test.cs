using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.Tilemaps;

public class test : MonoBehaviour
{
    // Start is called before the first frame update

    public int height;
    public int width;
    public int shiftheight;
    public int shiftwidth;
    private Grid grid;
    
    void Start()
    {
        Debug.Log("x ");
        grid = new Grid(width, height, shiftheight, shiftwidth, 1.0f);

        setTilemaps();
    }



    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //grid.SetValue(UtilsClass.GetMouseWorldPosition(),1);
            grid.calculateDistance(UtilsClass.GetMouseWorldPosition());
        }
        if (Input.GetMouseButtonDown(1))
        {
            //grid.SetValue(UtilsClass.GetMouseWorldPosition(),1);
            grid.markPath(UtilsClass.GetMouseWorldPosition());
        }

        //grid.calculateDistance(GameObject.Find("Player").transform.position);

    }


    private void setTilemaps()
    {
        GameObject obj = GameObject.Find("Tilemap");
        Tilemap tilemap = obj.GetComponent<Tilemap>();

        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    grid.SetValue(x, y, -1);
                }
                
            }
        }
    }
}
