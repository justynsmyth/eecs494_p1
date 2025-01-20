using TMPro;
using UnityEngine;

public class WeaponDisplayer : MonoBehaviour
{
    public string weapon_type;
    private string weapon_name;
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
        if (weapon_type == "primary" && text_component)
        {
            if (Inventory.HasSword)
            {
                weapon_name = "Sword";
            }
            text_component.text = "(X): " + weapon_name;
        }
        else if (weapon_type == "secondary" && text_component)
        {
            if (Inventory.HasBow)
            {
                weapon_name = "Bow";
            }
            text_component.text = "(Z): " + weapon_name;
        }
    }
}