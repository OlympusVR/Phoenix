using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// This script handles amount of enemies to spawn depending on currentWave which is updated
/// by TargetManager, might switch that around since think should actually be other way around
/// </summary>
namespace Phoenix
{
    public class GameManager : MonoBehaviour
    {
       
        #region All of the managers
        public static GameManager gameManager;
        public Text playerPointText;
        private TargetManager _manageAnchors;
        #endregion

        #region Bools that check whether time target at certain time points have already been spawned

        bool halfTimeSpawnTarget;
        bool quarterTimeSpawnTarget;

        //Added the suppress warning
        bool quarterTimeLeftSpawnTarget;

        #endregion

        private RectTransform _endGamePanel;
        private Text _displayPoints;

        #region Variables managing the round
        private int difficulty = 1;
        private int waveNumber = 1;
        private float _totalTime = 40.0f;
        #endregion

        public int _playerPoints;

        //  public abstract IEnumerator spawnPointTarget(int amountToSpawn);
        public string currentDifficulty
        {
            get; set;
        }
        public float timeLeftInRound
        {
            set { _totalTime -= value; }
            //return time left to keep track of in UI
            get { return _totalTime; }
        }
        /// <summary>
        /// This adds to player points and returns the tallied up score when round is over.
        /// </summary>
        public int playerPoints
        {
            set
            {
                _playerPoints += value;
                playerPointText.text = "Player Points: " + _playerPoints.ToString();
            }
            get { return _playerPoints; }//Returning for UI
        }

        //Adds time left in round when player kills TimeTarget
        public float addTimeLeftInRound
        {
            set { _totalTime += value; }

        }

        private void endRound()
        {
            //uncomment when display is connected
            //_displayPoints.text = playerPoints.ToString();
            //_endGamePanel.gameObject.SetActive(true);
        }
        private void Awake()
        {
            if (gameManager == null)
            {
                gameManager = this;
            }
            else
            {
                Destroy(this);
            }
            _manageAnchors = GetComponent<TargetManager>();

          //  _endGamePanel = GameObject.Find("EndGamePanel").GetComponent<RectTransform>();
           // _displayPoints = _endGamePanel.gameObject.GetComponentInChildren<Text>();
        }

        private void Start()
        {
     //       _endGamePanel.gameObject.SetActive(false);
        }


        public void waveOver()
        {
            _manageAnchors.initializeWave((playerPoints+ 1)*100.002f, difficulty);
        }

        //Instead of doing it on start, it will do when player clicks start button, just start for now
        public void startRound()
        {
            _manageAnchors.startWave();
        }
        private void Update()
        {
            if (timeLeftInRound != -1.0f)
            {
                if (timeLeftInRound > 0)
                {
                    timeLeftInRound = Time.deltaTime;
                }
                if (timeLeftInRound <= 0)
                {
                    _manageAnchors.despawnAllAnchors();
                    endRound();
                }

           
            }
        }
    }
}