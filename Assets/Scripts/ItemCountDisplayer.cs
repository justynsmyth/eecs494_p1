using TMPro;
using UnityEngine;

public class ItemCountDisplayer : MonoBehaviour
{
    public Inventory inventory;
    public string item_to_display;
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
        if (inventory && text_component && item_to_display == "rupees")
        {
            text_component.text = "Rupees: " + inventory.GetRupees().ToString();
        }
        else if (inventory && text_component && item_to_display == "keys")
        {
            text_component.text = "Keys: " + inventory.GetKeys().ToString();
        }
    }
}
