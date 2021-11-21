using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Pathfinding 
{
    
        public int height;
        public int width;
        public int shiftheight;
        public int shiftwidth;
        private Grid grid;


        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                grid.SetValue(UtilsClass.GetMouseWorldPosition(), 1);
            }
        }
    


}
