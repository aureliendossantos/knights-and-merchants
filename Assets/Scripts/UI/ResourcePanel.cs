using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourcePanel : MonoBehaviour
{
    public Resource resource;
    public Image icon;
    public TextMeshProUGUI quantity;

    public void SetQuantity(int number)
    {
        quantity.SetText(number.ToString());
    }
}
