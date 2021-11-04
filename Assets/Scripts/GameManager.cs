using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<CarController> cars = new List<CarController>();
    public Transform[] spawnPoints;
    public List<TrackZone> trackZones;

    public float positionUpdateRate = 0.05f;
    private float lastPositionUpdateTime;

    public bool gameStarted = false;

    public int playersToBegin = 2;
    public int lapsToWin = 3;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Time.time - lastPositionUpdateTime > positionUpdateRate)
        {
            lastPositionUpdateTime = Time.time;
            UpdateCarRacePositions();
        }

        if (!gameStarted && cars.Count == playersToBegin)
        {
            gameStarted = true;
            StartCountdown();
        }
    }

    private void StartCountdown()
    {
        PlayerUI[] uis = FindObjectsOfType<PlayerUI>();

        foreach (PlayerUI ui in uis)
        {
            ui.StartCountdownDisplay();
        }

        Invoke("BeginGame", 3.0f);
    }

    private void BeginGame()
    {
        foreach (CarController car in cars)
        {
            car.canControl = true;
        }
    }

    private void UpdateCarRacePositions()
    {
        cars.Sort(SortPosition);

        for (int i = 0; i < cars.Count; i++)
        {
            cars[i].racePosition = cars.Count - i;
        }
    }

    private int SortPosition(CarController a, CarController b)
    {
        if (a.zonesPassed > b.zonesPassed)
            return 1;
        else if (b.zonesPassed > a.zonesPassed)
            return -1;

        float aDist = Vector3.Distance(a.transform.position, a.currentTrackZone.transform.position);
        float bDist = Vector3.Distance(b.transform.position, b.currentTrackZone.transform.position);

        return aDist > bDist ? 1 : -1;
    }

    public void CheckIsWinner(CarController car)
    {
        if (car.currentLap > lapsToWin)
        {
            foreach (CarController currentCar in cars)
            {
                currentCar.canControl = false;
            }

            PlayerUI[] uis = FindObjectsOfType<PlayerUI>();

            foreach (PlayerUI ui in uis)
            {
                ui.GameOver(ui.car == car);
            }
        }
    }
}
