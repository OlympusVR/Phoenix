using UnityEngine;
using System.Collections;

public class GameInstance : MonoBehaviour {
    protected Object _anchorPool;
    protected int _wave;
    protected float _points;

    public float addPoints
    {
        set
        {
            _points += value;
        }
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
