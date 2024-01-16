using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleGroup : Singleton<ToggleGroup>
{
    public int GetActiveToggle()
    {
        Toggle[] toggles = this.GetComponentsInChildren<Toggle>();

        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn)
            {
                return i;
            }
        }
        return 0;
    }
}
