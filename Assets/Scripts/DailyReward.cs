using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyReward : MonoBehaviour
{
    public GameObject rewardMenu;
    public Text remainigTimeText;
    
    public bool initilazed;
    public long rewardGivingTimeTicks;
    long currentTime, remainigTime;

    public void InitializDailyReward()
    {
        if (PlayerPrefs.HasKey("lastDailyReward")) //daha �nce b�yle bir tan�mlama yap�lm�� m� diye kontrol ediyor.
        {
            rewardGivingTimeTicks = long.Parse(PlayerPrefs.GetString("lastDailyReward")) + 864000000000;//stringi longa �evirdik. "864000000000" 1 y�l�n trik say�s�?????????????????????????
            currentTime = System.DateTime.Now.Ticks;
            if (currentTime >= rewardGivingTimeTicks)//sonraki �d�l zaman� gelmi� mi
            {
                GiveReward();
            }
        }
        else
        {
            GiveReward();
        }
        initilazed = true;
    }

    public void GiveReward()
    {
        LevelController.current.GiveMoneyToPlayer(100);
        rewardMenu.SetActive(true);
        PlayerPrefs.SetString("lastDailyReward", System.DateTime.Now.Ticks.ToString());// son �d�l al�m tarihini g�ncelledik.,
        rewardGivingTimeTicks = long.Parse(PlayerPrefs.GetString("lastDailyReward")) + 864000000000; // bir sonraki �d�l al�m tarihini g�nceller.
    }

    // Update is called once per frame
    void Update()
    {
        if (initilazed)
        {
            if (LevelController.current.startMenu.activeInHierarchy)//start men� aktifse
            {
                currentTime = System.DateTime.Now.Ticks;
                remainigTime = rewardGivingTimeTicks - currentTime;
                if (remainigTime <= 0)
                {
                    GiveReward();
                }
                else
                {
                    System.TimeSpan timeSpan = System.TimeSpan.FromTicks(remainigTime);//timespan ile trik zaman�n� dakika saat g�n ay cinsinden bulabiliyoruz.timespan.hours gibi
                    remainigTimeText.text = string.Format("{0}:{1}:{2}", timeSpan.Hours.ToString("D2"), timeSpan.Minutes.ToString("D2"), timeSpan.Seconds.ToString("D2"));//D2 iki bamaktan k���k say�lar i�in ba��na 0 getirilmesini sa�lar. 01 05 09 gibi
                }
            }
        }
    }
    
    public void TapToReturnButton()
    {
        rewardMenu.SetActive(false);
    }
}
