using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectMap : MonoBehaviour
{
    // Start is called before the first frame update

    public void Map1()
    {
        Debug.Log("Map1 Selected");
        SceneManager.LoadScene("Level1");
    }
    public void Map2()
    {
        Debug.Log("Map2 Selected");
        SceneManager.LoadScene("Level2");
    }
    public void Map3()
    {
        Debug.Log("Map3 Selected");
        SceneManager.LoadScene("LEVEL3");
    }
}
