using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectibleCount : MonoBehaviour
{
    public static CollectibleCount Instance { get; private set; }

    private TextMeshProUGUI collectibleCountText;
    private int collectibleCount;

    private void Awake()
    {
        Instance = this;

        collectibleCountText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        UpdateCount();
    }

    private void OnEnable() 
    { 
        Collectible.OnCollected += OnCollectibleCollected; 
    }
    private void OnDisable() 
    { 
        Collectible.OnCollected -= OnCollectibleCollected; 
    }

    private void OnCollectibleCollected()
    {
        collectibleCount++;
        UpdateCount();
    }

    private void UpdateCount()
    {
        collectibleCountText.text = $"{collectibleCount} / {Collectible.totalCollectibles}";
    }

    public bool IsAllCollected()
    {
        return collectibleCount == Collectible.totalCollectibles;
    }
}
