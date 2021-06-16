using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    public Entity playerEntity;

    public RectTransform main;
    public RectTransform secondary;

    void Update()
    {
        float playerHealthPercent = playerEntity.health["current"]/playerEntity.health["max"];

        secondary.sizeDelta = new Vector2(1000 * playerHealthPercent, 5);
        secondary.localPosition = new Vector2((1000 * playerHealthPercent)/2f - 1000/2f, 0);
    }
}
