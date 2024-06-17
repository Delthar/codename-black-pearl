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
    /// Gets the current normalized wind direction.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetWindDirectionNormalized() => windDirection.normalized;

    /// <summary>
    /// Gets the current wind direction.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetWindDirection() => windDirection;

    /// <summary>
    /// Gets the current wind force.
    /// </summary>
    /// <returns></returns>
    public float GetWindForce() => windForce;

    /// <summary>
    /// Sets the wind direction.
    /// </summary>
    /// <param name="windDirection"></param>
    public void SetWindDirection(Vector2 windDirection) => this.windDirection = windDirection;

    /// <summary>
    /// Sets the wind force.
    /// </summary>
    /// <param name="windForce"></param>
    public void SetWindForce(float windForce) => this.windForce = windForce;

    /// <summary>
    /// Randomizes the wind direction and force.
    /// </summary>
    public void Randomize()
    {
        SetWindDirection(new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)));
        SetWindForce(UnityEngine.Random.Range(0f, maxWindSpeed));
    }



}
