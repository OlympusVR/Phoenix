using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Phoenix
{

    public delegate Vector3 Movement(Vector3 position, Vector3 direction,GameObject obj);

    public static class MovementManager
    {
        private static Vector3 ecMult=Random.insideUnitSphere;
        private static GameObject _playerPoint;
        public static bool simpleMove=true;

        public static Vector3 eccentricityModifier
        {
            get { return ecMult; }
            set { ecMult = value; }
        }
        
        public static GameObject playerPoint
        {
            get { return _playerPoint; }
            set { _playerPoint = value; }
        }

        public static Movement generateTargetMovement(float minDistance,float maxDistance,float speed,float eccentricity = 1.0f)
        {
            Movement returnedMovement;
            if (simpleMove)
            {

                returnedMovement = (Vector3 position, Vector3 direction, GameObject Obj) =>
                {

                    Vector3 newDirection = new Vector3(0, 0, 0);

                    newDirection = Vector3.Cross(new Vector3(0, 0, 1), position);
                    position.x = position.y = 0;
                    newDirection -= position;

                    return newDirection;
                };
            }
            else
            {
                //This movement is too "erratic" making new movement for a simple revolve
                returnedMovement = (Vector3 position, Vector3 direction, GameObject Obj) =>
                {
                    Vector3 newDirection = new Vector3();
                    if (position.sqrMagnitude > maxDistance * maxDistance)
                    {
                        newDirection = direction.normalized + Vector3.Cross(Vector3.Cross(position + eccentricityModifier * (eccentricity), direction), direction).normalized * eccentricity;
                        if (Vector3.Angle(position, newDirection) < 90) newDirection = Vector3.Cross(position + eccentricityModifier * (eccentricity), direction).normalized;
                        if (Vector3.Angle(direction, position) > 90)
                        {
                            newDirection = direction.normalized * eccentricity;
                        }
                    }
                    else if (position.sqrMagnitude < minDistance * minDistance)
                    {
                        newDirection = Vector3.Cross(direction, Vector3.Cross(position + eccentricityModifier * eccentricity, direction)).normalized * eccentricity;
                        if (Vector3.Angle(direction, position) > 90)
                        {
                            newDirection = direction.normalized * eccentricity;
                        }
                    }
                    else
                    {
                        newDirection = eccentricityModifier * speed * eccentricity * 0.001f;
                    }
                    newDirection = newDirection + direction.normalized;
                    return newDirection.normalized * speed;
                };

                returnedMovement = (Vector3 position, Vector3 direction, GameObject Obj) =>
                {
                    Vector3 newDirection = new Vector3();


                    newDirection = Vector3.Cross(position, direction);
                    newDirection = Vector3.Cross(newDirection, position);
                    newDirection += -position * (position.sqrMagnitude - minDistance * minDistance);

                    newDirection += Vector3.Cross(Obj.transform.InverseTransformPoint(playerPoint.transform.position), position).normalized;

                    return newDirection.normalized * speed;
                };
            }

            return returnedMovement;
        }
       



        public static Movement generateAnchorMovement(Vector3 initialPosition,GameObject target,float time)
        {
            float runTime = 0;

            //Anchor movement will be linear
            return (Vector3 position, Vector3 oldMovement,GameObject obj) =>
            {
                runTime += Time.deltaTime;
                Vector3 difference = (target.transform.localPosition - position);
                if (runTime >= time) return difference;
                else Debug.Log(time-runTime);
                if ((position - target.transform.localPosition).sqrMagnitude < 0.4) return difference;
                return (target.transform.localPosition-position)/(time-runTime);
            };
        }



    }
}