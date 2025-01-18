using TMPro;
using UnityEngine;

public class HealthDisplayer : MonoBehaviour
{
    public HasHealth playerHealth;
    TextMeshProUGUI text_component;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text_component = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // If our inventory and text component exist, set the text to number of rupees we have
        if (playerHealth && text_component)
        {
            text_component.text = "HP: " + playerHealth.GetHealth().ToString() + "/" + playerHealth.GetMaxHealth().ToString();
        }
    }
}
