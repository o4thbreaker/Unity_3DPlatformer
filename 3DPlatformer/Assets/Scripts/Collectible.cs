using System;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum CollectibleType
    {
        DashJar,
        DoubleJumpJar,
        BreakJar
    }

    [SerializeField] JarType jarType;
    public static event Action OnCollected;
    public static int totalCollectibles;
    public JarType JarType { get { return jarType; } }

    private void Awake()
    {
        totalCollectibles++;
    }

    private void Update()
    {
        transform.localRotation = Quaternion.Euler(0, Time.time * 100f, 20);
    }

    private void OnEnable()
    {
        jarType.IsAbilityAllowed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnCollected?.Invoke();
            jarType.IsAbilityAllowed = true;
            Destroy(gameObject);
        }
    }

    private void OnValidate()   
    {
        gameObject.name = jarType.name;
    }
}
