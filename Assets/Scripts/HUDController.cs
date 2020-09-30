using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField] Text totalMoneyLabel; // текущее колличесво денег (текст)
    [SerializeField] Text currentWave; // текущая волна (текст)
    [SerializeField] Text totalEscapeLabel; // сколько прошло противников (текст)
    public Button playBtn; // кнопка
    Text playBtnLabel; // текст кнопки "Play next Wave" (текст)

    // // ЭТО ЯВНО ДОЛЖНО БЫТЬ НЕ ТУТ, НО У МЕНЯ ТОЛЬКО ТАК ВЫШЛО
    //     if (waveNumber == totalWaves - 2)
    //     {
    //         playBtnLabel.text = "Final wave";
    //     }

    private void Start() 
    {
        playBtnLabel = playBtn.GetComponentInChildren<Text>(); 
    }

    private void Update() 
    {
        totalMoneyLabel.text = GameManager.Instance.TotalMoney.ToString();
        totalEscapeLabel.text = "Escaped " + GameManager.Instance.TotalEscaped + " of 10";
        currentWave.text = "Wave " + GameManager.Instance.WaveNumber;
    }
    public void ShowMenu()
    {
        playBtnLabel.text = "Play Game";
    }

    public void UpdateMenu(Manager.GameState state)
    {
        PlayButtonSetActive(true);

        switch(state)
        {
            case Manager.GameState.gameover:
            {
                playBtnLabel.text = "Play again!";
                break;
            }
            case Manager.GameState.nextWave:
            {
                playBtnLabel.text = "Next wave";
                break;
            }
            case Manager.GameState.win:
            {
                playBtnLabel.text = "WIN";
                break;
            }
            default: break;
        }
    }

    public void PlayButtonSetActive(bool isActive)
    {
        playBtn.gameObject.SetActive(isActive);
    }

    public void WaveNumberUpdate(int waveNumber)
    {
        currentWave.text = "Wave " + (waveNumber + 1); // увеличиваем значение волны
    }
}

// switch (Manager.Instance.currentState) // определяем состояние игры
        // {
        //     // Меняем текста кнопки в зависимости от положения игры

        //     case Manager.GameState.gameover:
        //         playBtnLabel.text = "Play again!";
        //         //AudioSource.PlayOneShot(SoundManager.Instance.GameOver); // звук проигрыша
        //         break;

        //     case Manager.GameState.nextWave:
        //             playBtnLabel.text = "Next wave";
        //         break;

        //     case Manager.GameState.play:
        //         playBtnLabel.text = "Play Game";
        //         break;

        //     case Manager.GameState.win:
        //         playBtnLabel.text = "WIN";
        //         break;
        // }
        // playBtn.gameObject.SetActive(true); // активируем кнопку
        // playBtn.gameObject.SetActive(false); // при старте кнопка "Play выключена"