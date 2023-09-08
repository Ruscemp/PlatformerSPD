using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Quest_Checker : MonoBehaviour
{
    [SerializeField] private GameObject Quest_End_Popup;
    [SerializeField] private TextMeshProUGUI Quest_End_Popup_Text;
    [TextArea(10, 10)][SerializeField] private string Quest_End_Finished_Text, Quest_End_Unfinished_Text;
    [SerializeField] private AudioClip[] Cheer_Sounds, Alert_Sounds;

    private Animator anim;
    private AudioSource Audio_Source;

    private void Start()
    {
        anim = GetComponent<Animator>();
        Audio_Source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Quest_End_Popup.SetActive(true);
            var Player = other.GetComponent<Player_Movement>();
            if (Player.QuestItems_Collected >= Player.QuestItems_Target_Count)
            {
                Quest_End_Popup_Text.text = Quest_End_Finished_Text;
                Player.QuestItems_Collected = 0;
                Player.QuestItems_Target_Count = 0;
                Player.QuestItems_Target_Tag = "";
                Player.UpdateQuest(null);
                PlaySound(Cheer_Sounds);
                anim.SetTrigger("Flag");
                // TODO: Change level
            }
            else
            {
                Quest_End_Popup_Text.text = Quest_End_Unfinished_Text;
                PlaySound(Alert_Sounds);
            }
        }
    }
    private void PlaySound(AudioClip[] sounds)
    {
        Audio_Source.pitch = Random.Range(0.75f, 1.25f);
        int RandomIndex = Random.Range(0, sounds.Length);
        Audio_Source.PlayOneShot(sounds[RandomIndex], 0.5f);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Quest_End_Popup.SetActive(false);
        }
    }
}
