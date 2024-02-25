using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBorder : Singleton<DeathBorder>
{
    public List<GameObject> DeathBorders;

    private void Start()
    {
        TurnOnAllDeathBorder();
    }
    public void TurnOffAllDeathBorder()
    {
        foreach (GameObject obj in DeathBorders)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
    public void TurnOnAllDeathBorder()
    {
        foreach (GameObject obj in DeathBorders)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }
}
