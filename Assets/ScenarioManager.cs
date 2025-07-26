using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
// [System.Serializable]
public class ScenarioManager : MonoBehaviour
{
    // NOTE: THE POSITIONS CAN BE IN THE SAME ORDER AS THE OBJECTS
    /*
     EXAMPLE TO ILLUSTRATE:
     JSON:.....UNITY:
     ObjA.Pos..ObjC --> ObjC will spawn at ObjC.Pos
     ObjB.Pos..ObjA --> ObjA will spawn at ObjA.Pos
     ObjC.Pos..ObjB --> ObjB will spawn at ObjB.Pos
    YOU CAN MIX UP THE ORDERS NOW
     */
    public string ScenarioFileName;
    public string scenarioPath;
    public GameObject[] objectPrefab;
    public string[] prefabNames; // Must be same length/order as objectPrefabs!
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(" Successfully got The Scenario Package manager working!");

        string scenarioPath = Path.Combine(Application.dataPath, "Scenarios", ScenarioFileName);
        Scenario scenario = ScenarioLoader.LoadScenario(scenarioPath);

        if (scenario != null)
        {
            Debug.Log("Loaded scenario: " + scenario.Name);
            ApplyScenario(scenario);
        }
        else
        {
            Debug.LogError("Failed to load scenario: " + ScenarioFileName);
        }
    }
    void ApplyScenario(Scenario scenario)
    {
        // Example: Log data (expand this to spawn vehicles, set weather, etc.)
        Debug.Log("Scenario Description: " + scenario.Description);

        // Example: spawn vehicle (expand as needed)
        // GameObject vehiclePrefab = ...; // assign via Inspector
        // Vector3 spawn = new Vector3(scenario.Vehicle.Spawn.X, scenario.Vehicle.Spawn.Y, scenario.Vehicle.Spawn.Z);
        // Instantiate(vehiclePrefab, spawn, Quaternion.identity);

        // TODO: Spawn objects, set environment, etc.
        for (int i = 0; i < scenario.spawnPositions.Count; i++)
        {
            Vector3 pos = new Vector3(
                scenario.spawnPositions[i].X,
                scenario.spawnPositions[i].Y,
                scenario.spawnPositions[i].Z
            );
            int prefabIdx = System.Array.IndexOf(prefabNames, scenario.spawnPositions[i].PrefabName);
            if (prefabIdx >= 0 && prefabIdx < objectPrefab.Length)
            {
                GameObject prefabToSpawn = objectPrefab[prefabIdx];
                Instantiate(prefabToSpawn, pos, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning($"PrefabName '{scenario.spawnPositions[i].PrefabName}' not found in prefabNames array!");
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    } 
 }



public class Scenario
{
    public string Name { get; set; }
    public string Description { get; set; }
    public EnvironmentSettings Environment { get; set; }
    public VehicleSettings Vehicle { get; set; }
    public List<Objective> Objectives { get; set; }
    public List<Event> Events { get; set; }
    public List<SuccessCondition> SuccessConditions { get; set; }
    public List<FailCondition> FailConditions { get; set; }
    public List<Location> spawnPositions { get; set; }
}

public class EnvironmentSettings
{
    public string Weather { get; set; }
    public string Time { get; set; }
}

public class VehicleSettings
{
    public string Type { get; set; }
    public SpawnPoint Spawn { get; set; }
}

public class SpawnPoint
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
}

public class Objective
{
    public string Type { get; set; }
    public Location Location { get; set; }
    public string Description { get; set; }
}

public class Event
{
    public string Type { get; set; }
    public Location TriggerLocation { get; set; }
    public float Delay { get; set; }
}

public class SuccessCondition
{
    public string Type { get; set; }
    public Location Location { get; set; }
}

public class FailCondition
{
    public string Type { get; set; }
    public ScenarioBounds Bounds { get; set; }
}

public class Location
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public string PrefabName { get; set; }
}

public class ScenarioBounds
{
    public float XMin { get; set; }
    public float XMax { get; set; }
    public float ZMin { get; set; }
    public float ZMax { get; set; }
}


public static class ScenarioLoader
{

    public static Scenario LoadScenario(string path)
    {
        string json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<Scenario>(json);
    }
}
