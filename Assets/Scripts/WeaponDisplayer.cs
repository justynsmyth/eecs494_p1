using TMPro;
using UnityEngine;

public class WeaponDisplayer : MonoBehaviour
{
    public string weapon_type;
    private string weapon_name;
    TextMeshProUGUI text_component;
    
    private Inventory inventory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text_component = GetComponent<TextMeshProUGUI>();
        inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        // If our inventory and text component exist, set the text to number of rupees we have
        if (weapon_type == "primary" && text_component)
        {
            weapon_name = inventory.GetCurrentXSlotItem();
            text_component.text = "(X): " + weapon_name;
        }
        else if (weapon_type == "secondary" && text_component)
        {
            weapon_name = inventory.GetCurrentZSlotItem();
            text_component.text = "(Z): " + weapon_name;
        }
    }
}