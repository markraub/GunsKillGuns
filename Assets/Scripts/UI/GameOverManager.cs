using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
   public void OnReturnClick()
   {
      if (SceneManager.GetActiveScene().name == "WIN")
      {
         SceneManager.LoadScene("Battle");
      }
      else
      {
         SceneManager.LoadScene("Main Menu");
      }
   }
}
