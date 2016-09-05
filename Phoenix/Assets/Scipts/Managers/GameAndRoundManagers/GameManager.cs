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
<<<<<<< HEAD
        private TargetManager _enemyManager;
=======
        private TargetManager _manageAnchors;
>>>>>>> 9fe897ff0d7369d9a3533367630db2cf667a5dac
        #endregion

        //UI Variables
        private GameObject _startGamePanel;
        private GameObject _gameOverPanel;
        //Updates in game stats
        private Text _displayPoints;
<<<<<<< HEAD
        private Text _displayTimeLeft;
        //Final end of game stats
        //Kinda just went to do in endround function since only need to use in there. Yeah I'll do that, unless we're saving score, well whatever I'll keep as is.
        private Text _displayTotalPoints;
        private Text _displayFinalTime;
        

        
=======

>>>>>>> 9fe897ff0d7369d9a3533367630db2cf667a5dac
        #region Variables managing the round
        private int difficulty = 1;
        private int waveNumber = 1;
        private float _totalTime = 40.0f;
        #endregion

        public int _playerPoints;

        public string currentDifficulty
        {
            get; set;
        }
        public float timeLeftInRound
        {
<<<<<<< HEAD
            set { _timeLeftInRound = value;
                _displayTimeLeft.text = _timeLeftInRound.ToString();
            }
=======
            set { _totalTime -= value; }
>>>>>>> 9fe897ff0d7369d9a3533367630db2cf667a5dac
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
                _displayPoints.text = _playerPoints.ToString();
            }
            get { return _playerPoints; }//Returning for UI
        }

        //Adds time left in round when player kills TimeTarget
        public float addTimeLeftInRound
        {
<<<<<<< HEAD
            set { _timeLeftInRound += value;}

        }

      
=======
            set { _totalTime += value; }

        }

        private void endRound()
        {
            //uncomment when display is connected
            //_displayPoints.text = playerPoints.ToString();
            //_endGamePanel.gameObject.SetActive(true);
        }
>>>>>>> 9fe897ff0d7369d9a3533367630db2cf667a5dac
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
<<<<<<< HEAD
            
            

            //Getting references to UI
            _startGamePanel = GameObject.Find("StartGamePanel");
            _gameOverPanel = GameObject.Find("GameOverPanel");
            
            _displayPoints = GameObject.Find("DisplayPoints").GetComponent<Text>();
            _displayTimeLeft = GameObject.Find("DisplayTimeLeft").GetComponent<Text>();
            _displayTotalPoints = GameObject.Find("DisplayFinalScore").GetComponent<Text>();
            _displayFinalTime = GameObject.Find("DisplayFinalTime").GetComponent<Text>();

=======
            _manageAnchors = GetComponent<TargetManager>();

          //  _endGamePanel = GameObject.Find("EndGamePanel").GetComponent<RectTransform>();
           // _displayPoints = _endGamePanel.gameObject.GetComponentInChildren<Text>();
>>>>>>> 9fe897ff0d7369d9a3533367630db2cf667a5dac
        }

        private void Start()
        {
<<<<<<< HEAD
            _enemyManager = GetComponent<TargetManager>();
            _gameOverPanel.gameObject.SetActive(false);
            playerPoints = 0;
            timeLeftInRound = -1.0f;
        }

        #region Game state functions
        public void GoMainMenu()
        {
            _startGamePanel.SetActive(true);

        }
        public void startRound()
        {
            if (_gameOverPanel.gameObject.activeInHierarchy)
                _gameOverPanel.SetActive(false);
            _startGamePanel.SetActive(false);
            _enemyManager.initializeWave(100.002f, 1);

            _enemyManager.inWave = true;
            _timeLeftInRound = _roundTimer;
            playerPoints = 0;
        }
        private void endRound()
        {
            _enemyManager.inWave = false;
            _enemyManager.despawnAllAnchors();
            _gameOverPanel.SetActive(true);
            
            _displayTotalPoints.text = playerPoints.ToString();
            _displayFinalTime.text = timeLeftInRound.ToString();
=======
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
>>>>>>> 9fe897ff0d7369d9a3533367630db2cf667a5dac
        }
        
        public void leaveGame()
        {
            //Will only work on build release of game, but does work.
            Application.Quit();
        }
        #endregion
        private void Update()
        {
            if (timeLeftInRound != -1.0f)
            {
                if (timeLeftInRound > 0)
                {
                    timeLeftInRound -= Time.deltaTime;
                    _displayTimeLeft.text = timeLeftInRound.ToString();
                }
                if (timeLeftInRound <= 0)
                {
                   // _manageAnchors.despawnAllAnchors();
                    endRound();
                }

                //temp input for showing updating score working.
                if (Input.GetKeyDown(KeyCode.S))
                {
                    playerPoints = 5;
                }
            }
        }
    }
}