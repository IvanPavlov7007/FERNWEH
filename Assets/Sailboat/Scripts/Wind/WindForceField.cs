using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystemForceField))]
public class WindForceField : MonoBehaviour
{
    [SerializeField]
    ParticleSystemForceField forceField;

    private void FixedUpdate()
    {
        //If you ever come back to particle system as wind particles

        //Vector2 windSpeed = WindManager.Instance.WindSpeed;
        //Vector2 windAccel = windSpeed / Time.fixedDeltaTime;
        //forceField.directionX = windAccel.x;
        //forceField.directionY = windAccel.y;
    }

}
