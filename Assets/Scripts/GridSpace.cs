using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour
{
    public Button button;
    public Text buttonText;
    private OldGameController gameController;
    public void SetSpace(int id)
    {
        if ((!gameController.isBot) || (gameController.currentSide == gameController.playerSide))
        {
            buttonText.text = gameController.GetCurrentSide();
            button.interactable = false;
            button.image.sprite = gameController.clickedButtonIcon;
            gameController.AddId(id);
            gameController.EndTurn();
        }
    }
    public void SetGameControllerReference(OldGameController controller)
    {
        gameController = controller;
    }
}
