using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowPopUp : MonoBehaviour
{
    [SerializeField] private GameObject PopupToShow;
    [TextArea(10, 10)][SerializeField] private string PopupText;
    [SerializeField] private AudioClip[] ShowSound, HideSound;

    private AudioSource Audio_Source;
    private TextMeshProUGUI Popup_Text;

    // Start is called before the first frame update
    void Start()
    {
        Audio_Source = GetComponent<AudioSource>();
        Popup_Text = PopupToShow.GetComponentInChildren<TextMeshProUGUI>();
        Popup_Text.text = PopupText;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PopupToShow.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PopupToShow.SetActive(false);
        }
    }
}
