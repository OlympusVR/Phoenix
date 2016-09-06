using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Phoenix
{
    public struct TargetInfo
    {
        public Movement moveSet;
        public Vector3 initialPosition;
        public Vector3 initialMovement;
    }
    public class Target : MonoBehaviour , PoolObject
    {

        //For point targets, it will add to player points, for Time targets it will add to current time left in round
        protected GameManager _gainedFromTarget;
        protected TargetManager _manageTargets;
        protected float _penetrationThreshHold;
        //Direction the target will move, decided in algorithim at targetMovementManager
        protected Movement _moveSet;
        protected Vector3 _lastMove;
        protected int _indexPatternMovement;
        protected float maxDistance;
        protected float minDistance;
        protected float speed = 10.0f;
        protected float _amplitude;
        protected float _frequency;

        protected DeathCall _onDeath;

        public DeathCall onDeath
        {
            get { return _onDeath == null ? (GameObject o,PoolObject p) => { } : _onDeath; }
            set { _onDeath = value == null ? null : _onDeath + value; }
        }


        char _chosenAxisForPattern;
        protected float _currentPattern;
    
        public void setPatternVars(float amp, float freq)
        {
            _amplitude = amp;
            _frequency = freq;
        }


        protected virtual void Awake()
        {
            _gainedFromTarget = GameObject.Find("GameManager").GetComponent<GameManager>();
            gameObject.tag = "Target";
            
        }
        protected virtual void Start()
        {
            _penetrationThreshHold = 5.0f;
            
        }
       
        protected virtual void Update()
        {

            transform.localPosition += _lastMove * Time.deltaTime;
            _lastMove = _moveSet(transform.localPosition, _lastMove,gameObject);
        }

        protected virtual void OnTriggerEnter(Collider hit)
        {
            if (hit.gameObject.tag == "bullet")
            {
                //Adds to player points
                _gainedFromTarget.playerPoints = 5;

                hit.GetComponent<Bullet>().penetration = _penetrationThreshHold;

                onDeath(gameObject,this);
                onDeath = null;
            }
        }
        
        public void setInfo(TargetInfo copy)
        {
            transform.localPosition = copy.initialPosition;
            _lastMove = copy.initialMovement;
            _moveSet = copy.moveSet;
        }

    }
}