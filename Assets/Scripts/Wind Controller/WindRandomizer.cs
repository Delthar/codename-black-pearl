using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindRandomizer : MonoBehaviour
{
    [SerializeField] private float minRandTime = 30;
    [SerializeField] private float maxRandTime = 60;

    private WindController windController;
    private float currentTimer;
    private float randTimer;

    private void Awake() {
        windController = GetComponent<WindController>();
    }

    private void Start()
    {
        randTimer = GetRandomTime();
        currentTimer = 0;
    }
    
    private void Update()
    {
        currentTimer += Time.deltaTime;
        if(currentTimer > randTimer)
        {
            randTimer = GetRandomTime();
            currentTimer = 0;
            windController.Randomize();
        }
    }

    private float GetRandomTime() => Random.Range(minRandTime, maxRandTime);
}
