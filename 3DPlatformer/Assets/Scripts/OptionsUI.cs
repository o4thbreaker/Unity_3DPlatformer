using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }   

    //[SerializeField] private Button someButton;
    //[SerializeField] private Button someOtherButton;
    [SerializeField] private Button closeOptionsButton;

    private void Awake()
    {
        Instance = this;

        /*someButton.onClick.AddListener(() =>
        {

        });

        someOtherButton.onClick.AddListener(() =>
        {

        });*/

        closeOptionsButton.onClick.AddListener(() =>
        {
            Hide();
        });
    }

    private void Start()
    {
        SanyaGameManager.Instance.OnGameUnpaused += SanyaGameManager_OnGameUnpaused;
        Hide();
    }

    private void SanyaGameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
