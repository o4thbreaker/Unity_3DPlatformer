using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectibleCountText : MonoBehaviour
{
    private TextMeshProUGUI collectibleCountText;
    private int collectibleCount;

    private void Awake()
    {
        collectibleCountText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        UpdateCount();
    }

    private void OnEnable() => Collectible.OnCollected += OnCollectibleCollected;
    private void OnDisable() => Collectible.OnCollected -= OnCollectibleCollected;

    private void OnCollectibleCollected()
    {
        collectibleCount++;
        UpdateCount();
    }

    private void UpdateCount()
    {
        collectibleCountText.text = $"{collectibleCount} / {Collectible.totalCollectibles}";
    }
}
