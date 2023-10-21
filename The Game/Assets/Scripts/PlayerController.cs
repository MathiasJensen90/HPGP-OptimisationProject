using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform ProjectileSpawner;
    public AudioSource ShotAudioSource;
    public PlayerSettings PlayerSettings;
    public GameObject cooldownVisualEffect; 

    public Animator animator;

    private Camera mainCamera;
    private float timeOfLastShot = 0f;
    private bool showCooldownVisual = true;

    // Start is called before the first frame update
    void Start()
    {
        GameMain.Instance.PlayerWasKilled += () =>
        {
            animator.SetTrigger("Death");
        };

        ShotAudioSource.clip = PlayerSettings.ShotSound;
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        Plane plane = new Plane(Vector3.up, transform.position);
        Vector3 rayHit;

        if (plane.Raycast(ray, out distance))
        {
            rayHit = ray.GetPoint(distance);
            transform.LookAt(rayHit);
        }
        if (Time.time - timeOfLastShot > PlayerSettings.ShotCooldown && showCooldownVisual == false)
        {
            showCooldownVisual = true;
            cooldownVisualEffect.SetActive(true);
        }
        
        // Shoot
        if (Input.GetKeyDown(PlayerSettings.ShootKey) && Time.time - timeOfLastShot > PlayerSettings.ShotCooldown)
        {
            timeOfLastShot = Time.time;
            ShotAudioSource.Play();
            cooldownVisualEffect.SetActive(false);
            showCooldownVisual = false;

            DOTween.Sequence().AppendInterval(0.1f).AppendCallback(() => { Instantiate(PlayerSettings.ProjectilePrefab, ProjectileSpawner.position, transform.rotation); });
            animator.SetTrigger("Shoot");
        }
    }
}
