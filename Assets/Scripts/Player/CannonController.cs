using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    [Header("Events")]
    [Tooltip("")]
    [SerializeField] public EventHandler<OnFireEventArgs> OnLeftSideFire;
    [Tooltip("")]
    [SerializeField] public EventHandler<OnFireEventArgs> OnRightSideFire;
    [Tooltip("")]
    [SerializeField] public EventHandler<OnFireCooldownChangedEventArgs> OnFireCooldownChanged;
    [Tooltip("")]
    [SerializeField] public EventHandler OnFireCooldownEnd;

    [Tooltip("")]
    [SerializeField] private List<Cannon> leftSide;
    [Tooltip("")]
    [SerializeField] private List<Cannon> rightSide;

    [Header("Cannon Status")]
    [Tooltip("The current charged amount that determines the range of the cannonball")]
    [Range(4, 20)]
    [SerializeField] private float currentCharge;
    [Tooltip("The current cooldown before another shot can be made")]
    [Range(0, 8)]
    [SerializeField] private float currentFireCooldown;

    [Header("Cannon Parameters")]
    [Tooltip("The upper limit of charge that determines the maximum range of a shot")]
    [Range(16, 20)]
    [SerializeField] private float maximumCharge;
    [Tooltip("The lower limit of charge that determines the minimum range of a shot")]
    [Range(4, 12)]
    [SerializeField] private float minimumCharge;
    [Tooltip("The rate at which charge is accumulated")]
    [Range(12, 16)]
    [SerializeField] private float chargeMultiplier;
    [Tooltip("The amount of force of a shot")]
    [Range(8, 18)]
    [SerializeField] private float fireForce;
    [Tooltip("The cooldown before another shot can be made")]
    [Range(2, 8)]
    [SerializeField] private float fireCooldown;

    #region Engine Methods

    private void OnEnable()
    {
        leftSide.ForEach(cannon => OnLeftSideFire += cannon.GetComponent<Cannon>().CannonController_OnFire);    
        rightSide.ForEach(cannon => OnRightSideFire += cannon.GetComponent<Cannon>().CannonController_OnFire);    
    }

    private void Update()
    {
        UpdateFireCooldown();
    }

    private void OnDisable()
    {
        leftSide.ForEach(cannon => OnLeftSideFire -= cannon.GetComponent<Cannon>().CannonController_OnFire);    
        rightSide.ForEach(cannon => OnRightSideFire -= cannon.GetComponent<Cannon>().CannonController_OnFire);    
    }

    #endregion

    #region Primary Methods

    public void Aim(Vector3 targetPosition)
    {
        float angle = GetTargetAngle(transform.position, targetPosition);

        if (angle < -90 || angle > 90) leftSide.ForEach(cannon => DirectCannon(cannon, GetTargetAngle(cannon.transform.position, targetPosition)));
        else if (angle < 90 || angle > -90) rightSide.ForEach(cannon => DirectCannon(cannon, GetTargetAngle(cannon.transform.position, targetPosition)));
    }

    public void Charge()
    {
        if (currentFireCooldown <= 0)
        {   
            if (currentCharge < maximumCharge) currentCharge += Time.deltaTime * chargeMultiplier;    
        }
    }

    public void Fire(Vector3 targetPosition, float? charge)
    {
        currentCharge = charge == null ? currentCharge : charge.Value; 

        float angle = GetTargetAngle(transform.position, targetPosition);
        
        if (angle < -90 || angle > 90) 
        {
            StartCoroutine(FireSalveRoutine(rightSide, targetPosition));
        }
        if (angle < 90 || angle > -90) 
        {
            StartCoroutine(FireSalveRoutine(leftSide, targetPosition));
        }

        
    }

    private IEnumerator FireSalveRoutine(List<Cannon> cannonSide, Vector3 targetPosition)
    { 
        for (int i = cannonSide.Count() - 1; i >= 0; i--)
        {
            cannonSide[i].Fire(new OnFireEventArgs(cannonSide[i], gameObject, targetPosition, fireForce, currentCharge));
            yield return new WaitForSeconds(0.25f);
        }
        RevertCharge();
        RevertFireCooldown();
    }

    #endregion

    #region Helper Methods

    private void DirectCannon(Cannon cannon, float angle) => cannon.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);

    private float GetTargetAngle(Vector3 originPosition, Vector3 targetPosition)
    {
        Vector3 direction = transform.InverseTransformDirection((targetPosition - originPosition).normalized);

        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    #endregion

    #region Invocation Methods

    private void InvokeLeftSideFire(Vector2 targetPosition, Cannon cannon) => OnLeftSideFire?.Invoke(this, new OnFireEventArgs(cannon, gameObject, targetPosition, fireForce, currentCharge)); 
    
    private void InvokeRightSideFire(Vector2 targetPosition, Cannon cannon) => OnRightSideFire?.Invoke(this, new OnFireEventArgs(cannon, gameObject, targetPosition, fireForce, currentCharge)); 

    private void InvokeFireCooldownChanged() => OnFireCooldownChanged?.Invoke(this, new OnFireCooldownChangedEventArgs(fireCooldown, currentFireCooldown));
    
    private void InvokeFireCooldownEnd() => OnFireCooldownEnd.Invoke(this, EventArgs.Empty);

    #endregion

    #region Update Methods

    private void UpdateFireCooldown()
    {
        if (currentFireCooldown > 0)
        {
            currentFireCooldown -= Time.deltaTime;

            if (currentFireCooldown < 0) InvokeFireCooldownEnd();
        }

        InvokeFireCooldownChanged();
    }

    #endregion

    #region Revert Methods

    private void RevertCharge() => currentCharge = minimumCharge;

    private void RevertFireCooldown() => currentFireCooldown = fireCooldown;

    #endregion
}

    // private void DrawChargeLine(Transform relationPoint, Vector3 targetDirection, Color lineColor)
    // {
    //     chargeLine.SetPosition(0, new Vector3(relationPoint.position.x, relationPoint.position.y, 0));
    //     chargeLine.SetPosition(1, targetDirection);
    //     chargeLine.startColor = lineColor;
    //     chargeLine.endColor = lineColor;
    // }

    // private void ResetChargeLine()
    // {
    //     chargeLine.SetPosition(0, transform.position);
    //     chargeLine.SetPosition(1, transform.position);
    // }
