using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.Tilemaps;


public class Grid
{
    private int width;
    private int height; 
    private float cellSize;
    private int shiftheight;
    private int shiftwidth;
    private int[,] grid;
    private int[,] visited;
    private float[,] distance;
    private TextMesh[,] debugArray;

    public Grid(int width, int height, int shiftheight, int shiftwidth, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.shiftheight = shiftheight;
        this.shiftwidth = shiftwidth;
        grid = new int[width, height];
        visited = new int[width, height];
        distance = new float[width, height];
        debugArray = new TextMesh[width, height];

        for (int i=0; i < grid.GetLength(0);i++)
        {
            for (int j = 0; j< grid.GetLength(1); j++)
            {
                debugArray[i,j]=UtilsClass.CreateWorldText(grid[i,j].ToString(), null, Grid2World(i, j),10, Color.white, TextAnchor.MiddleCenter);
                distance[i, j] = 100000000000;
            }
        }

        
    }
    public void SetValue(int i, int j, int value)
    {
        if ( i >=0 &&j>=0 && i<width&& j<height)
        grid[i, j] = value;
        debugArray[i, j].text = grid[i,j].ToString();
    }

    private void clear()
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                distance[i, j] = 100000000000;
                visited[i, j] = 0;
            }
        }
    }

    public void SetMinDistanceValue(int i, int j, float value)
    {
        if (i >= 0 && j >= 0 && i < width && j < height)
            if (value < distance[i, j])
            {
                distance[i, j] = value;
                debugArray[i, j].text = distance[i, j].ToString();
            }
    }
    public void SetValue(Vector3 worldPosition,int value)
    {
        Vector2 position = World2Grid(worldPosition);
        SetValue((int)position.x, (int)position.y, value);
    }

    private Vector3 Grid2World(int x, int y)
    {
        return new Vector3(x+shiftwidth, y+shiftheight) * cellSize + new Vector3(0.5f,0.5f);
    }
  

    private  Vector2 World2Grid(Vector3 worldPosition)
    {
        return new Vector2(Mathf.FloorToInt(worldPosition.x / cellSize)-shiftwidth, Mathf.FloorToInt(worldPosition.y / cellSize)-shiftheight);
    }

    private void setTilemaps(string name)
    {
        GameObject obj = GameObject.Find(name);
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

    private void A_star(int x,int y)
    {
        List<Vector2> open = new List<Vector2>();
        visited[x, y] = 1;
        float distance = this.distance[x, y];
        SetMinDistanceValue(x, y, distance);

        open.Add(new Vector2(x, y));

        //while(open.Count > 0)
        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (i >= 0 && j >= 0 && i < width && j < height)
                {
                    if ((i != x || j != y) && grid[i, j] == 0)
                        if(i==x || j==y)
                            SetMinDistanceValue(i,j, distance + 1);
                        else
                            SetMinDistanceValue(i, j, distance + 1.41f);
                }
            }
        }
        
        int min_x=0, min_y=0;
        float min_v = 1000000000;

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if(this.distance[i,j]<min_v && visited[i,j]==0)
                {
                    min_v =this. distance[i, j];
                    min_x = i;
                    min_y = j;

                }
            }
        }

        A_star(min_x, min_y);

   



     }

    public void calculateDistance(Vector3 worldPosition)
    {

        Vector2 position = World2Grid(worldPosition);
        clear();
        distance[(int)position.x, (int)position.y] = 0;
        A_star((int)position.x, (int)position.y);
    }

    private Vector2 findMinNeighbour(int x,int y)
    {
        float min_distance = 99999999;
        int min_x = 0, min_y = 0;

        for(int i=x-1;i<=x+1;i++)
        {
            for(int j=y-1;j<=y+1;j++)
            {
                if (i >= 0 && j >= 0 && i < width && j < height )
                {
                    if((i!=x || j!=y) && grid[i,j]==0)
                        if (distance[i, j] < min_distance)
                        {
                            min_distance = distance[i, j];
                            min_x = i;
                            min_y = j;
                        }
                }
            }
        }
        return new Vector2(min_x, min_y);
       
    }
    public void markPath(Vector3 worldPosition)
    {
        Vector2 position = World2Grid(worldPosition);
        int x =(int) position.x;
        int y =(int) position.y;
        int cnt = 100;
        while(distance[x,y]!=0 || cnt==0)
        {
            debugArray[x,y].text = "X";
            Vector2 minNeighbourCord = findMinNeighbour(x, y);
            x = (int)minNeighbourCord.x;
            y =(int) minNeighbourCord.y;
            cnt--;
        }

    }
}
