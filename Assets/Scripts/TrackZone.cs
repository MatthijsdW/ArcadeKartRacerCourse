using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackZone : MonoBehaviour
{
    public bool isGate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CarController car = other.GetComponent<CarController>();
            if (this != GameManager.instance.trackZones[car.targetTrackZone])
                return;

            car.currentTrackZone = this;
            car.targetTrackZone++;
            if (car.targetTrackZone == GameManager.instance.trackZones.Count)
            {
                car.targetTrackZone = 0;
            }
            car.zonesPassed++;

            if (isGate)
            {
                car.currentLap++;
                GameManager.instance.CheckIsWinner(car);
            }
        }
    }
}
