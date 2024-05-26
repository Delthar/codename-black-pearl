using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour
{
    public static WindController Instance { get; private set; }

    [SerializeField] private Vector2 windDirection;
    [SerializeField] private float windForce = 1;

    void Awake()
    {
        Instance = this;
    }

    public Vector2 GetWindDirection() => windDirection;
    public float GetWindForce() => windForce;

    public void SetWindDirection(Vector2 windDirection) => this.windDirection = windDirection;
    public void SetWindForce(float windForce) => this.windForce = windForce;
}
