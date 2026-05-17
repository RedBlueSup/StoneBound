using System;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private DoorManager[] doors;
    [SerializeField] private bool rune = false;

    bool buttonArmed = true;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D Player)
    {
        if ((buttonArmed && Player.CompareTag("Player") && rune == false) || (buttonArmed && Player.CompareTag("Spirit") && rune == true))
        {
            foreach (DoorManager door in doors)
            {
                door.ToggleDoor();
            }
            buttonArmed = false;
        }         
    }

    private void OnTriggerExit2D(Collider2D Player)
    {
        if ((Player.CompareTag("Player") && rune == false ) || (Player.CompareTag("Spirit") && rune == true)) buttonArmed = true;
    }
}
