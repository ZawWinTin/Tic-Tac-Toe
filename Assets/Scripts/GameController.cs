using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Player
{
    public Image panel;
    public Text text;
    public Button button;
}

[System.Serializable]
public class PlayerColor
{
    public Color panelColor, textColor;
}

[System.Serializable]
public class PlayerIcon
{
    public Sprite icon;
    public Color textColor;
}

public class GameController : MonoBehaviour {
    public Player playerX, playerO;
    public PlayerColor activePlayerColor, inactivePlayerColor, normalColor;
    public Sprite clickedbuttonicon, inactivebuttonicon, activebuttonicon;
    public Text[] buttonList;
    public GameObject gameOverPanel,gameModePanel,restartButton, startInfo,gameMode,buttonX,buttonO,buttonPvP,buttonPvC,computerFirstButton;
    public Text gameOverText,startInfoText,gameModeText;
    public ArrayList clickedButtonIndex;
    private int moveCount,lastClickedButtonId;
    public float computerDelay;
    public bool isBot,isGameOver;
    public string computerSide, currentSide,playerSide;
    private ArrayList buttonid = new ArrayList()
    {
        new int[,]{{1,2},{3,6},{4,8}},      //0
        new int[,]{{0,2},{4,7}},            //1
        new int[,]{{0,1},{4,6},{5,8}},      //2
        new int[,]{{0,6},{4,5}},            //3
        new int[,]{{0,8},{1,7},{2,6},{3,5}},//4
        new int[,]{{2,8},{3,4}},            //5
        new int[,]{{0,3},{2,4},{7,8}},      //6
        new int[,]{{1,4},{6,8}},            //7
        new int[,]{{0,4},{2,5},{6,7}}       //8
    };
    const int middleId = 4;
    const string draw = "draw";
    const string draw_text = "It's a draw !";
    const string playMode_text = "Choose\nPlay Mode";
    const string chooseSide_text = "X or O ?\nChoose a side";
    const string x = "X";
    const string o = "O";
    const string playervsPlayer = "Player vs Player";
    const string playerVsAI = "Player vs AI";
    void SetGameControllerReferenceOnButtons()
    {
        for(int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }
    void Awake()
    {
        computerFirstButton.SetActive(false);
        SetGameModeButton(null, false);
        SetIntroButton(true);
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
        moveCount = 0;
        isGameOver = false;
        isBot = false;
        SetGameControllerReferenceOnButtons();
    }
    public string GetCurrentSide()
    {
        return currentSide;
    }
    public void AddId(int id)
    {
        lastClickedButtonId = id;
        clickedButtonIndex.Add(id);
    }
    public void EndTurn()
    {
        computerFirstButton.SetActive(false);
        moveCount++;
        int[,] checkid = (int[,])buttonid[lastClickedButtonId];
        bool isGameEnd = false;
        for(int i = 0; i < checkid.GetLength(0); i++)
        {
            if(buttonList[lastClickedButtonId].text==currentSide && buttonList[checkid[i,0]].text==currentSide && buttonList[checkid[i, 1]].text == currentSide)
            {
                GameOver(currentSide);
                isGameEnd = true;
                break;
            }
        }
        if(!isGameEnd && (moveCount == 9))
        {
            GameOver(draw);
        }
        else if(!isGameEnd)
        {
            ChangeSides();
            computerDelay = 10;
        }
    }
    void GameOver(string winningPlayer)
    {
        isGameOver = true;
        SetBoardInteractable(false);
        restartButton.SetActive(true);
        if (winningPlayer == draw)
        {
            SetPlayerColorsInactive();
            SetGameOverText(draw_text);
        }
        else
        {
            SetGameOverText(winningPlayer+ " Wins!");
        }       
    }
    void ChangeSides()
    {
        if (currentSide == x)
        {
            currentSide = o;
            SetPlayerColors(playerO, playerX);
        }
        else
        {
            currentSide = x;
            SetPlayerColors(playerX, playerO);
        }
    }
    void SetGameOverText(string value)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = value;
    }
    public void RestartGame()
    {
        isBot = false;
        changeBoardInactiveIcon();
        SetGameModeButton(null, false);
        SetIntroButton(true);
        startInfo.SetActive(true);
        SetPlayerColorsNormal();
        SetPlayerButtons(true);
        gameOverPanel.SetActive(false);       
        moveCount = 0;
        computerDelay = 10;
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].text = "";
        }
        restartButton.SetActive(false);
    }
    void changeBoardInactiveIcon()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().image.sprite = inactivebuttonicon;
        }
    }
    void SetBoardInteractable(bool toggle)
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle;
            if (toggle)
            {
                buttonList[i].GetComponentInParent<Button>().image.sprite = activebuttonicon;
            }
        }
    }
    void SetPlayerColors(Player newPlayer,Player oldPlayer)
    {
        newPlayer.panel.color = activePlayerColor.panelColor;
        newPlayer.text.color = activePlayerColor.textColor;
        oldPlayer.panel.color = inactivePlayerColor.panelColor;
        oldPlayer.text.color = inactivePlayerColor.textColor;
    }
    public void SetStartingSide(string startingSide)
    {
        currentSide = startingSide;
        if (currentSide == x)
        {
            playerSide = x;
            computerSide = o;
            SetPlayerColors(playerX, playerO);
        }
        else
        {
            playerSide = o;
            computerSide = x;
            SetPlayerColors(playerO, playerX);
        }
        StartGame();
    }
    void StartGame()
    {
        SetBoardInteractable(true);
        SetPlayerButtons(false);
        startInfo.SetActive(false);
        computerFirstButton.SetActive(isBot);
        clickedButtonIndex = new ArrayList();
        isGameOver = false;
    }
    void SetPlayerButtons(bool toggle)
    {
        playerX.button.interactable = toggle;
        playerO.button.interactable = toggle;
    }
    void SetPlayerColorsInactive()
    {
        playerX.panel.color = inactivePlayerColor.panelColor;
        playerX.text.color = inactivePlayerColor.textColor;
        playerO.panel.color = inactivePlayerColor.panelColor;
        playerO.text.color = inactivePlayerColor.textColor;
    }
    void SetPlayerColorsNormal()
    {
        playerX.panel.color = normalColor.panelColor;
        playerX.text.color = normalColor.textColor;
        playerO.panel.color = normalColor.panelColor;
        playerO.text.color = normalColor.textColor;
    }
    public void SetGameMode(string playmode)
    {
        SetIntroButton(false);
        SetGameModeButton(playmode, true);
        isBot = (playmode == playerVsAI) ? true : false;
    }
    void SetIntroButton(bool toggle)
    {
        buttonPvP.SetActive(toggle);
        buttonPvC.SetActive(toggle);
        buttonX.SetActive(!toggle);
        buttonO.SetActive(!toggle);
        startInfoText.text =toggle? playMode_text:chooseSide_text;
    }
    void SetGameModeButton(string text,bool toggle)
    {
        gameModePanel.SetActive(toggle);
        gameModeText.text = text;
    }
    public void SetComputerFirst()
    {
        computerFirstButton.SetActive(false);
        currentSide = computerSide;
        if (currentSide == x)
        {
            SetPlayerColors(playerX, playerO);
        }
        else
        {
            SetPlayerColors(playerO, playerX);
        }
    }
    void ComputerMove()
    {
        int computerClickedIndex = GetComputerCalculatedMoveId();
        lastClickedButtonId = computerClickedIndex;
        clickedButtonIndex.Add(computerClickedIndex);
        buttonList[computerClickedIndex].GetComponentInParent<Button>().interactable = false;
        buttonList[computerClickedIndex].GetComponentInParent<Text>().text = currentSide;
        buttonList[computerClickedIndex].GetComponentInParent<Button>().image.sprite = clickedbuttonicon;
        EndTurn();
    }
    private void Update()
    {
        if (isBot && (currentSide == computerSide) && !(isGameOver))
        {
            computerDelay += computerDelay * Time.deltaTime;
            if (computerDelay >= 60)
            {
                ComputerMove();
            }
        }
    }
    int GetComputerCalculatedMoveId()
    {
        //Attack
        for(int i = 0; i < buttonid.Count; i++)
        {
            int[,] attackcheckid = (int[,])buttonid[i];
            for (int ii = 0; ii < attackcheckid.GetLength(0); ii++)
            {
                if ((buttonList[i].text == computerSide) && (buttonList[attackcheckid[ii, 0]].text == computerSide) && (buttonList[attackcheckid[ii, 1]].text == ""))
                {
                    return attackcheckid[ii, 1];
                }
                else if ((buttonList[i].text == computerSide) && (buttonList[attackcheckid[ii, 1]].text == computerSide) && (buttonList[attackcheckid[ii, 0]].text == ""))
                {
                    return attackcheckid[ii, 0];
                }
            }
        }
 
        //Defence
        int[,] defencecheckid = (int[,])buttonid[lastClickedButtonId];
        for (int i = 0; i < defencecheckid.GetLength(0); i++)
        {
            if ((buttonList[defencecheckid[i, 0]].text == playerSide) && (buttonList[defencecheckid[i, 1]].text == ""))
            {
                return defencecheckid[i, 1];
            }
            else if((buttonList[defencecheckid[i, 1]].text == playerSide) && (buttonList[defencecheckid[i, 0]].text == ""))
            {
                return defencecheckid[i, 0];
            }
        }

        //Middle
        if (buttonList[middleId].text == "")
        {
            return middleId;
        }

        //Random
        int computerCalculatedMoveId;
        bool isComputerMoveOk;
        do
        {
            isComputerMoveOk = true;
            computerCalculatedMoveId = Random.Range(0, 9);
            for (int i = 0; i < clickedButtonIndex.Count; i++)
            {
                if ((int)clickedButtonIndex[i] == computerCalculatedMoveId)
                {
                    isComputerMoveOk = false;
                    break;
                }
            }
        } while (!isComputerMoveOk);
        return computerCalculatedMoveId;
    }
}
