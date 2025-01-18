using TMPro;
using UnityEngine;

public class RupeeDisplayer : MonoBehaviour
{
    public Inventory inventory;
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
        if (inventory && text_component)
        {
            text_component.text = "Rupees: " + inventory.GetRupees().ToString();
        }
    }
}
