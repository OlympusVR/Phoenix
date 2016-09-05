using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Phoenix
{
    /// <summary>
    /// Object Pool is used when objects need to be instantiated and then subsequently destroyed.
    /// Objects are kept in the object pool instead of destroying it in order to reduce instantiating overhead during runtime.
    /// </summary>
    public class ObjectPool : MonoBehaviour
    {
        //Prefab will change depending on what we're pooling
        protected GameObject _objectPrefab;
        protected List<GameObject> _allObjects;
        protected Stack<GameObject> _objectPool;

        public GameObject objectPrefab
        {
            get
            {
                return _objectPrefab;
            }
            set
            {
                if(value.GetComponent<PoolObject>()!= null)
                {
                    _objectPrefab = value;
                    despawnAllObjects();
                    _objectPool.Clear();
                    initialize();
                }
            }
        }

        public int activeObjects
        {
            get
            {
                return _allObjects.Count - _objectPool.Count;
            }
        }

        public virtual void initialize()
        {
            initialize(5);
        }

        public virtual void initialize(int amountReadyToSpawn)
        {
            //This adds all of the objects into the array
            for (int i = 0; i < amountReadyToSpawn; i++)
            {
                _allObjects.Add(Instantiate(_objectPrefab));
                _objectPool.Push(_allObjects[i]);
                _objectPool.Peek().SetActive(false);
                _objectPool.Peek().transform.parent = transform;
            }
        }

        public GameObject getObject()
        {
            GameObject spawn;
            if (_objectPool.Count > 0)
            {
                spawn = _objectPool.Pop();
                spawn.GetComponent<PoolObject>().onDeath = returnToPool;
            }
            else
            {
                spawn = Instantiate(_objectPrefab);
                spawn.transform.SetParent(transform);
                if (spawn == null)
                {
                    Debug.Log("object was never instantiated");
                }
                _allObjects.Add(spawn);
                spawn.GetComponent<PoolObject>().onDeath = returnToPool;
            }
            return spawn;
        }
        
        public void despawnAllObjects()
        {
            if (_allObjects == null) return;
            foreach (var x in _allObjects)
            {
                if (x.activeInHierarchy)
<<<<<<< HEAD

                    x.GetComponent<PoolObject>().onDeath(x,x.GetComponent<PoolObject>());
=======
                    x.GetComponent<PoolObject>().onDeath(x, x.GetComponent<PoolObject>());
>>>>>>> 9fe897ff0d7369d9a3533367630db2cf667a5dac
            }
        }

        private void returnToPool(GameObject o,PoolObject p)
        {
            p.onDeath = null;
            _objectPool.Push(o);
            o.SetActive(false);
        }
    
        protected virtual void Awake()
        {
            //instantiates the list
            _objectPool = new Stack<GameObject>();
            _allObjects = new List<GameObject>();
        }

    }

    public delegate void DeathCall(GameObject gameObject,PoolObject self);

    public interface PoolObject
    {
        DeathCall onDeath
        {
            set;
            get;
        }
    }

}