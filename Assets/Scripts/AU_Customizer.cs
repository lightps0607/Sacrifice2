using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AU_Customizer : MonoBehaviour
{
    [SerializeField] public Color[] AllColors;
    [SerializeField] public Sprite[] AllHats;
    [SerializeField] GameObject ColorPanel;
    [SerializeField] GameObject HatPanel;
    [SerializeField] Button ColorTabButton;
    [SerializeField] Button HatTabButton;



    public void SetColor(int ColorIndex)
    {
        AU_PC.LocalPlayer.SetColor(ColorIndex);
    }

    public void SetHat(int HatIndex)
    {
        AU_PC.LocalPlayer.SetHat(HatIndex);
    }



    public void NextScene(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }

    public void EnableColors()
    {
        ColorPanel.SetActive(true);
        HatPanel.SetActive(false);
        HatTabButton.interactable = true;
        ColorTabButton.interactable = false;
    }

    public void EnableHats()
    {
        ColorPanel.SetActive(false);
        HatPanel.SetActive(true);
        HatTabButton.interactable = false;
        ColorTabButton.interactable = true;
    }


}
