using UnityEngine;

[CreateAssetMenu(fileName = "Player Settings", menuName = "GameSettings/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public KeyCode ShootKey;
    public float ShotCooldown = 0.5f;
    public GameObject ProjectilePrefab;
    public AudioClip ShotSound;
    public float LightningPropagationRadius = 4f;
    public GameObject ImpactFXPrefab;
    public LightningPropagator LightningPropagator;
}
