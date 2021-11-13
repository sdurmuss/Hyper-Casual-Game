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
        if (PlayerPrefs.HasKey("lastDailyReward")) //daha önce böyle bir tanýmlama yapýlmýþ mý diye kontrol ediyor.
        {
            rewardGivingTimeTicks = long.Parse(PlayerPrefs.GetString("lastDailyReward")) + 864000000000;//stringi longa çevirdik. "864000000000" 1 yýlýn trik sayýsý?????????????????????????
            currentTime = System.DateTime.Now.Ticks;
            if (currentTime >= rewardGivingTimeTicks)//sonraki ödül zamaný gelmiþ mi
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
        PlayerPrefs.SetString("lastDailyReward", System.DateTime.Now.Ticks.ToString());// son ödül alým tarihini güncelledik.,
        rewardGivingTimeTicks = long.Parse(PlayerPrefs.GetString("lastDailyReward")) + 864000000000; // bir sonraki ödül alým tarihini günceller.
    }

    // Update is called once per frame
    void Update()
    {
        if (initilazed)
        {
            if (LevelController.current.startMenu.activeInHierarchy)//start menü aktifse
            {
                currentTime = System.DateTime.Now.Ticks;
                remainigTime = rewardGivingTimeTicks - currentTime;
                if (remainigTime <= 0)
                {
                    GiveReward();
                }
                else
                {
                    System.TimeSpan timeSpan = System.TimeSpan.FromTicks(remainigTime);//timespan ile trik zamanýný dakika saat gün ay cinsinden bulabiliyoruz.timespan.hours gibi
                    remainigTimeText.text = string.Format("{0}:{1}:{2}", timeSpan.Hours.ToString("D2"), timeSpan.Minutes.ToString("D2"), timeSpan.Seconds.ToString("D2"));//D2 iki bamaktan küçük sayýlar için baþýna 0 getirilmesini saðlar. 01 05 09 gibi
                }
            }
        }
    }
    
    public void TapToReturnButton()
    {
        rewardMenu.SetActive(false);
    }
}
