using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tut_WidgetSwitcher : MonoBehaviour
{

    [SerializeField] GameObject[] AllPages;


    int CurrentPage;



    public void SlidePage(bool Left)
    {
        if (Left)
        {
            CurrentPage--;

            if (CurrentPage < 0)    //Clamp
            {
                CurrentPage = AllPages.Length - 1;
            }
        }

        else
        {
            CurrentPage++;

            if (CurrentPage > AllPages.Length - 1)  //Clamp
            {
                CurrentPage = 0;
            }
        }

        SwitchPage(CurrentPage);
    }



    public void SwitchPage(int id)
    {
        if (!(id < AllPages.Length))    //Stops for Invalid ID
            return;

        CurrentPage = id;

        for (int i = 0; i < AllPages.Length; i++)
        {
            if(i == id)
            {
                AllPages[i].SetActive(true);
            }
            else
            {
                AllPages[i].SetActive(false);
            }
        }
    }


    public void ShowTutorial(GameObject TutorialPanel)
    {
        TutorialPanel.SetActive(!TutorialPanel.activeInHierarchy);
    }



    //Quit Game
    public void OnClickQuitGame()
    {
        Application.Quit();
    }


}
