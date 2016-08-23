using UnityEngine;
using System.Collections;

namespace Phoenix {
    public struct AnchorInfo
        {
            public TargetInfo[] targetList;
            public Vector3 initialPosition;
            public Vector3 initialMovement;
            public Movement moveSet;
        
        }
    /// <summary>
    /// This script is for handling anchor movement/respawning and spawning targets
    /// </summary>
    public class Anchor : MonoBehaviour , PoolObject
    {


        private GameManager _gameManager;
        //Reference to targetManager to spawn new anchor to replace when this one dies, and is also used to get random spawn point for targets
        private TargetManager _spawnNewAnchor;

        //Variables for handling anchor movement
        Movement _moveSet;
        private NavMeshAgent _navAgent;
        private Vector3 _lastMove;
        private float _anchorSpeed;
        DeathCall _onDeath;
        
        //List of targets to spawn
        private ObjectPool _targetPool;
        private TargetInfo[] _targetList;
        //Amount of targets to spawn
        private int amountToSpawn;
       
        public DeathCall onDeath
        {
            set { _onDeath = value; }
            get { return _onDeath; }
        }

        //Gets needed references  
        private void Awake()
        {
            _targetPool = gameObject.AddComponent<ObjectPool>();
            _targetPool.objectPrefab = Resources.Load("Prefabs/TargetPrefabs/Target") as GameObject;
        }

        //Spawns targets for anchor, and changes their properties depending if this anchor is timetarget one or not
       

        private void Update()
        {
            //transform.localPosition += _lastMove*Time.deltaTime;
            //_lastMove = _moveSet(transform.localPosition, _lastMove);
    
        }

        public void setInfo(AnchorInfo copy)
        {
            transform.localPosition = copy.initialPosition;
            _lastMove = copy.initialMovement;
            _moveSet = copy.moveSet;
            _targetList = copy.targetList;
            if (_targetList != null)
                for (int i = 0; i < _targetList.Length; i++)
                {
                    GameObject target = _targetPool.getObject();
                    Target script = target.GetComponent<Target>();
                    if(script!=null)script.setInfo(_targetList[i]);
                    target.SetActive(true);
                }
        }
    }
       
}