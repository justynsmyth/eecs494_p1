using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthDisplayer : MonoBehaviour
{
    public HasHealth playerHealth;
    TextMeshProUGUI text_component;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text_component = GetComponent<TextMeshProUGUI>();
    }
    
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded");
        playerHealth = GameObject.FindWithTag("Player").GetComponent<HasHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth == null)
        {
            playerHealth = GameObject.FindWithTag("Player").GetComponent<HasHealth>();
        }
        // If our inventory and text component exist, set the text to number of rupees we have
        if (playerHealth && text_component)
        {
            text_component.text = "HP: " + playerHealth.GetHealth().ToString() + "/" + playerHealth.GetMaxHealth().ToString();
        }
    }
}
