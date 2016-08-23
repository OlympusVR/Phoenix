using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Phoenix
{

    public delegate Vector3 Movement(Vector3 position, Vector3 direction);

    public static class MovementManager
    {
        private static Vector3 ecMult=Random.insideUnitSphere;

        public static Vector3 eccentricityModifier
        {
            get { return ecMult; }
            set { ecMult = value; }
        }
        
        public static Movement generateTargetMovement(float minDistance,float maxDistance,float speed,float eccentricity = 1.0f)
        {
            Movement returnedMovement;
            returnedMovement = (Vector3 position,Vector3 direction) =>
            {
                Vector3 newDirection = new Vector3();
                if (position.sqrMagnitude > maxDistance * maxDistance)
                {
                    newDirection = Vector3.Cross(Vector3.Cross(position + ecMult * eccentricity, direction), direction).normalized * eccentricity;
                    if (Vector3.Angle(direction, position) > 90)
                    {
                        newDirection = direction.normalized * eccentricity;
                    }
                }
                else if (position.sqrMagnitude < minDistance * minDistance)
                {
                    newDirection = Vector3.Cross(direction, Vector3.Cross(position + ecMult * eccentricity, direction)).normalized * eccentricity;
                    if (Vector3.Angle(direction, position) > 90)
                    {
                        newDirection = direction.normalized*eccentricity;
                    }
                }
                else
                {
                    newDirection = ecMult * speed * eccentricity * 0.001f;
                }
                newDirection = newDirection + direction.normalized;
                return newDirection.normalized*speed;
            };
            return returnedMovement;
        }
       



        public static Movement generateAnchorMovement(Vector3 initialPosition,float distance,float speed,float ecentricity)
        {
            //Anchor movement will be linear
            return (Vector3 position, Vector3 oldMovement) =>
            {
                Vector3 newMovement = new Vector3(oldMovement.x,oldMovement.y,oldMovement.z);
                //TODO: make actual movement algorithm
                if ((position - initialPosition).sqrMagnitude > distance * distance)
                {
                    newMovement = new Vector3(-oldMovement.x, -oldMovement.y, -oldMovement.z);
                }


                return newMovement.normalized*speed;
            };
        }



    }
}