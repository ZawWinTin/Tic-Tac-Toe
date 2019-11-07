using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour {

    public Button button;
    public Text buttonText;
    private GameController gameController;
    public void SetSpace(int id)
    {
        if((!gameController.isBot) || (gameController.currentSide == gameController.playerSide))
        {
            buttonText.text = gameController.GetCurrentSide();
            button.interactable = false;
            button.image.sprite =gameController.clickedbuttonicon;
            gameController.AddId(id);
            gameController.EndTurn();
        }  
    }
    public void SetGameControllerReference(GameController controller)
    {
        gameController = controller;
    }
}
