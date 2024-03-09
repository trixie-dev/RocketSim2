using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    public Explosive explosive;
    public TerrainManager terrainManager;
    
    
    public void Explode(Vector3 position, float radius)
    {
        terrainManager.CreateCrater(position, explosive.radius);
    }
    
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 200, 200));
        if (GUILayout.Button("Place explosive"))
        {
            explosive.transform.position = new Vector3(Random.Range(0, 200), 11, Random.Range(0, 200));
        }
        GUILayout.EndArea();
        GUILayout.BeginArea(new Rect(10, 50, 200, 200));
        if (GUILayout.Button("Create Crater"))
        {
            explosive.Explode();
        }
        GUILayout.EndArea();
    }
}
