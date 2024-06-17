using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour
{
    public static WindController Instance { get; private set; }

    public event EventHandler<OnWindDirectionChangedEventArgs> OnWindDirectionChanged;
    public event EventHandler<OnWindForceChangedEventArgs> OnWindForceChanged;
    public class OnWindForceChangedEventArgs : EventArgs 
    {
        public float maxWindSpeed;
        public float oldWindForce;
        public float newWindForce;
    }
    public class OnWindDirectionChangedEventArgs : EventArgs
    {
        public Vector2 oldWindDirection;
        public Vector2 newWindDirection;
    }

    [SerializeField] private Vector2 windDirection;
    [SerializeField] private float windForce = 1;
    [Range(0, 5)]
    [SerializeField] private float maxWindSpeed = 5;

    private Vector2 currentWindDirection;
    private float currentWindForce;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Randomize();
        InvokeOnWindDirectionEvent();
        currentWindDirection = windDirection;
        currentWindForce = windForce;
    }

    private void Update()
    {
        if(currentWindDirection != windDirection)
        {
            InvokeOnWindDirectionEvent();
            currentWindDirection = windDirection;
        }
        if(currentWindForce != windForce)
        {
            InvokeOnWindForceEvent();
            currentWindForce = windForce;
        }
    }

    private void InvokeOnWindForceEvent()
    {
        OnWindForceChanged.Invoke(this, new OnWindForceChangedEventArgs
        {
            oldWindForce = currentWindForce,
            newWindForce = windForce,
            maxWindSpeed = maxWindSpeed,
        });
    }

    private void InvokeOnWindDirectionEvent()
    {
        OnWindDirectionChanged.Invoke(this, new OnWindDirectionChangedEventArgs
        {
            oldWindDirection = currentWindDirection,
            newWindDirection = windDirection,
        });
    }

    /// <summary>
    /// Diese methode gibt dir die wind richtung zurück lel.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetWindDirection() => windDirection.normalized;
    public float GetWindForce() => windForce;

    public void SetWindDirection(Vector2 windDirection) => this.windDirection = windDirection;
    public void SetWindForce(float windForce) => this.windForce = windForce;
    public void Randomize()
    {
        SetWindDirection(new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)));
        SetWindForce(UnityEngine.Random.Range(0f, maxWindSpeed));
    }



}
