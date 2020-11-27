using Actor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Code : MonoBehaviour
{
    // Shamelessly taken from: https://answers.unity.com/questions/1146027/c-make-something-happen-if-i-press-a-sequence-of-k.html
    private KeyCode[] sequence = new KeyCode[]{
        KeyCode.P,
        KeyCode.E,
        KeyCode.K,
        KeyCode.O};

    private int sequenceIndex;
    private bool accessGranted = false;
    public PlayerLiving player;

    private void Update() {
        if (Input.GetKeyDown(sequence[sequenceIndex])) {
            if (++sequenceIndex == sequence.Length){
                sequenceIndex = 0;
                accessGranted = true;
            }
        } else if (Input.anyKeyDown) sequenceIndex = 0;

        if (accessGranted)
        {
            player.health = player.totalHealth;
        }
    }


}
