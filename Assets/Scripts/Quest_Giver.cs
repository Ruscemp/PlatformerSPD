using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quest_Giver : MonoBehaviour
{
    [SerializeField] private GameObject Quest_Popup;
    [SerializeField] private TextMeshProUGUI Quest_Popup_Text;
    [TextArea(14, 13)] [SerializeField] private string Quest_Text;
    [SerializeField] private int Qust_Target_Count = 0;
    [SerializeField] private string Quest_Target_Tag;
    [SerializeField] private AudioClip[] Quest_Sounds;


    private AudioSource Audio_Source;
    private void Start()
    {
        Audio_Source = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Quest_Popup.SetActive(true);
            Quest_Popup_Text.text = Quest_Text;
            other.gameObject.GetComponent<Player_Movement>().QuestItems_Target_Count = Qust_Target_Count;
            other.gameObject.GetComponent<Player_Movement>().QuestItems_Target_Tag = Quest_Target_Tag;
            other.gameObject.GetComponent<Player_Movement>().UpdateQuest(null);
            PlaySound(Quest_Sounds);
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
            Quest_Popup.SetActive(false);
        }
    }
}
