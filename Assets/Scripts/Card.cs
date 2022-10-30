using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Event", menuName = "Event Card", order = 1)]
public class Card : ScriptableObject {
    public Sprite eventSprite;
    public int weight;
    public bool recurring;
    public Option option1;
    public Option option2;
    public Option option3;
}
