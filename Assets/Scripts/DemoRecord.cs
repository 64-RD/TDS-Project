using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class DemoRecord :MonoBehaviour
{
    public string filename;
    ArrayList observations = new ArrayList();
    ArrayList actions = new ArrayList();
    private Enemy enemy;

    public void init()
    {
        enemy = GetComponent<Enemy>();
    }

    public void record ()
    {
        var x = enemy.transform.GetChild(2).GetComponent<RayPerceptionSensorComponent2D>().GetRayPerceptionInput();
        var y = RayPerceptionSensor.Perceive(x);
        var rays = y.RayOutputs;
        foreach (var ray in rays)
        {
            float[] vector = new float[10];
            ray.ToFloatArray(8, 0, vector);
            foreach (var o in vector)
                observations.Add(o);
        }

        x = enemy.transform.GetChild(3).GetComponent<RayPerceptionSensorComponent2D>().GetRayPerceptionInput();
        y = RayPerceptionSensor.Perceive(x);
        rays = y.RayOutputs;
        foreach (var ray in rays)
        {
            float[] vector = new float[10];
            ray.ToFloatArray(8, 0, vector);
            foreach (var o in vector)
                observations.Add(o);
        }

        /*var obs = GetObservations();
        foreach (float o in obs)
            observations.Add(o);

        observations.Add(-2137f);

        var act = GetAction();
        foreach (float a in act)
            actions.Add(a);

        actions.Add(-2137f);*/
    }


    public void save_data()
    {
        using (StreamWriter outputFile = new StreamWriter("E:/REPOS/"+filename, true))
        {
            outputFile.Write("B\n");
            foreach (float o in observations)
            {
                if (o == -2137)
                    outputFile.Write("\n");
                else
                    outputFile.Write(o.ToString() + ";");

            }
            outputFile.Write("\n");
            foreach (float a in actions)
            {
                if (a == -2137)
                    outputFile.Write("\n");
                else
                    outputFile.Write(a.ToString() + ";");

            }
            outputFile.Write("\n");
            observations.Clear();
            actions.Clear();
        }

    }
}
