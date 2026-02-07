using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    private static WorldManager _instance;
    public static WorldManager Instance => _instance;

    [HideInInspector] public Transform homeStation;
    private void Awake()
    {
        //Make this an Instance
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        homeStation = null;


        int homeStationCount = 0;

        foreach (Station station in GetComponentsInChildren<Station>())
        {
            if (station.getStationType() == Station.StationType.HomeStation)
            {
                homeStationCount++;
                homeStation = station.gameObject.transform;
            }
        }

        if (homeStationCount == 0)
        {
            Debug.LogError("Missing Home Station!");
        }

        if (homeStationCount > 1)
        {
            Debug.LogError("Too Many Home Stations!");
        }

    }
}
