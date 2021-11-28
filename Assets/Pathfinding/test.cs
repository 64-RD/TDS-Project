using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.Tilemaps;

public class test : MonoBehaviour
{
    // Start is called before the first frame update

    public int height=0;
    public int width=0;
    public int shiftheight=0;
    public int shiftwidth=0;
    private Grid grid;
   

    void Start()
    {
        Debug.Log("x ");
        grid = new Grid(width, height, shiftheight, shiftwidth, 1.0f);

        grid.setTilemaps("Tilemap");

    }



    private void Update()
    {
        
       /* if (Input.GetMouseButtonDown(0))
        {
            //grid.SetValue(UtilsClass.GetMouseWorldPosition(),1);
            start = UtilsClass.GetMouseWorldPosition();
            //grid.calculateDistance(start);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("liczba");
            //grid.SetValue(UtilsClass.GetMouseWorldPosition(),1);
            Vector3 end = UtilsClass.GetMouseWorldPosition();
            //grid.markPath(end);
            List<Vector3> path = grid.FindPath(start, end);
            Debug.Log("liczba");
            foreach (var x in path)
            {
                Debug.Log(x.ToString());
            }

        }*/

        //grid.calculateDistance(GameObject.Find("Player").transform.position);

    }

    public List<Vector3> getPath(Vector3 startPos,Vector3 endPos)
    {
        return grid.FindPath(startPos, endPos);
    }

   
}
