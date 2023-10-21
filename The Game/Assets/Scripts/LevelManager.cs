using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int _nextLevelIndex = 1;
    [SerializeField] private List<EnemySpawner> _enemySpawners;
    private bool _loadingScene;

    public GameObject screenTransition;

    private float transitionLeft = -2.2f;
    private float transitionCenter = 17.5f;
    private float transitionRight = 40;
    private float transitionTime = 4f;

    private AudioSource audSource; 

    private void Start()
    {
        audSource = GetComponent<AudioSource>(); 
        DOTween.Sequence()
               .AppendCallback(() =>
               {
                   screenTransition.transform.DOLocalMoveX(transitionRight, transitionTime);
               }).Play();
    }

    private void Update()
    {
        if (IsLevelComplete() && !_loadingScene)
        {
            audSource.Play();
            _loadingScene = true;
            var pos = screenTransition.transform.localPosition;
            screenTransition.transform.localPosition = new Vector3(transitionLeft, pos.y, pos.z);
            DOTween.Sequence()
                .AppendCallback(()=> 
                {
                    screenTransition.transform.DOLocalMoveX(transitionCenter, transitionTime);
                }).AppendInterval(transitionTime)
                .AppendCallback(() =>
                {     
                    SceneManager.LoadScene(_nextLevelIndex);
                }).Play();
        }
    }

    public bool IsLevelComplete()
    {
        
        var allEnemiesSpawned = _enemySpawners.All(ene => ene.SpawnCount <= 0);
        var allEnemiesDead = AIController.Instance.enemyCount == 0;
        return allEnemiesSpawned && allEnemiesDead;
    }
}
