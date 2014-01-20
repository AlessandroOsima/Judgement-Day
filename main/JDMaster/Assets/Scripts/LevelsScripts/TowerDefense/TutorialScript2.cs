using UnityEngine;
using System.Collections;

public class TutorialScript2 : MonoBehaviour
{
    private static float height = Screen.height / 10;
    private static float width = Screen.width / 10;
    private static int charS = 13;
    private float time = 0f;
    private float dt;
    private int messageCount = 0;

    // Use this for initialization
    void Start()
    {
        WriteMessage("Ok, Now you know how to take souls", 7f, 6f, 6f);
    }

    public static void WriteMessage(string s, float h, float w, float d)
    {
        int i = 0;
        int j = 0;
        float space = (w * width);
        string word;
        while ((j = s.IndexOf(" ", i)) != -1)
        {
            word = s.Substring(i, (j - i));
            LevelGUI.levelGUI.writeMessage(word, new Vector3(space, (h * height), 0f), new Vector3(1.4f, 1.4f, 1), d, 1);
            space += (word.Length + 2) * charS;
            j++;
            i = j;
        }
        word = s.Substring(i);
        LevelGUI.levelGUI.writeMessage(word, new Vector3(space, (h * height), 0f), new Vector3(1.4f, 1.4f, 1), d, 1);
    }

    // Update is called once per frame
    void Update()
    {
        dt = Time.deltaTime;
        time += dt;

        if (time > 6f && messageCount == 0)
        {
            WriteMessage("But there are some that you can not take directly", 7f, 4f, 8f);
            WriteMessage("like those with the blue dot, adoring another god", 8f, 4f, 8f);
            messageCount++;
        }

        if (time > 14f && messageCount == 1)
        {
            WriteMessage("Look at them just laughing there", 7f, 6f, 6f);
            messageCount++;
        }
        if (time > 15f && messageCount == 2)
        {
            WriteMessage("You should use your power to teach them", 8f, 5f, 5f);
            messageCount++;
        }
        if (time > 21f && messageCount == 3)
        {
            WriteMessage("Why not try a little FIRE!!", 7f, 6f, 7f);
            messageCount++;
        }
        if (time > 22f && messageCount == 4)
        {
            WriteMessage("Press 1 to select FIRE!", 8f, 6.5f, 6f);
            messageCount++;
        }
        if (time > 28f && messageCount == 5)
        {
            WriteMessage("Use the arrows to move your target", 7f, 6f, 6f);
            messageCount++;
        }
        if (time > 35f && messageCount == 6)
        {
            WriteMessage("Good! Now burn the unworthy", 7f, 6f, 6f);
            messageCount++;
        }
        if (time > 37f && messageCount == 7)
        {
            WriteMessage("Press SPACE to use your Power", 8f, 5.5f, 6f);
            messageCount++;
        }
        if (time > 45f && messageCount == 8)
        {
            if (GlobalManager.globalManager.population <= 1)
            {
                WriteMessage("JAJAJAJAJAJAJAJAJA...", 7f, 6f, 4f);
                messageCount++;
                time = 45f;
            }
            else
            {
                if (GlobalManager.globalManager.souls < 7)
                {
                    time += dt;
                    if (time > 52f)
                    {
                        WriteMessage("You missed. Did you not?", 7f, 6f, 6f);
                        messageCount++;
                    }
                }
                time -= dt;
            }
        }
        if (time > 49f && messageCount == 9 && GlobalManager.globalManager.population < 2)
        {
            WriteMessage("Sorry, I got carried away", 7f, 5.5f, 6f);
            messageCount++;
        }
        if (time > 58f && messageCount == 9 && GlobalManager.globalManager.population == 2
           || (time > 111f && messageCount == 16 && GlobalManager.globalManager.souls <= 7
                && GlobalManager.globalManager.population > 0))
        {
            WriteMessage("Do not worry. You can Redo the level any time", 7f, 4f, 6f);
            WriteMessage("Press the Redo button up in the left", 8f, 5f, 20f);
            messageCount += 10;
        }
        if (time > 55f && messageCount == 10)
        {
            WriteMessage("However be aware! When you use your powers you use SOULS", 7f, 3f, 8f);
            WriteMessage("Your number of SOULS is up there with the crosses", 8f, 4f, 8f);
            messageCount++;
        }
        if (time > 63f && messageCount == 11)
        {
            WriteMessage("You have a limited number of SOULS", 7f, 6f, 8f);
            WriteMessage("But with each infidel you get his sins weight in SOULS", 8f, 4f, 8f);
            messageCount++;
        }
        if (time > 71f && messageCount == 12)
        {
            WriteMessage("Some will worth more your time, some less", 7f, 5f, 8f);
            WriteMessage("Depends on their actions, and Yours", 8f, 6f, 8f);
            messageCount++;
        }
        if (time > 80f && messageCount == 13)
        {
            WriteMessage("The cost of the Powers is stated next to it", 7f, 5f, 8f);
            WriteMessage("You can use some extra SOULS this time", 8f, 6f, 8f);
            GlobalManager.globalManager.incrementSouls(8);
            messageCount++;

        }
        if (time > 89f && messageCount == 14)
        {
            if (GlobalManager.globalManager.population > 0)
            {
                WriteMessage("Now end with the missery of that poor guy", 7f, 5f, 7f);
                WriteMessage("Do him a favor", 8f, 7f, 7f);
                messageCount++;
            }
            else
            {
                WriteMessage("Nice!! you got 2 for 1", 7f, 6f, 8f);
                messageCount += 3;
                time = 103f;
            }
        }
        if (time > 98f && messageCount == 15)
        {
            if (GlobalManager.globalManager.population == 0)
            {
                WriteMessage("Good Job!!!", 7f, 8f, 4f);
                messageCount++;
            }
            else
            {
                if (GlobalManager.globalManager.souls < 7)
                {
                    time += dt;
                    if (time > 105f)
                    {
                        WriteMessage("You missed. Did you not?", 7f, 6f, 6f);
                        messageCount++;
                    }
                }
                time -= dt;
            }
        }
        if (time > 103f && messageCount == 16 && GlobalManager.globalManager.population <= 0)
        {
            LevelGUI.levelGUI.writeMessage("VICTORY", new Vector3(Screen.width / 2, Screen.height / 2, 0f), new Vector3(1.4f, 1.4f, 1), 10f, 0);
            messageCount++;
        }
        if (time > 110f && messageCount == 17)
        {
            Application.LoadLevel("SplashScreen");
        }

        if (time > 109f && messageCount >= 19)
        {
            WriteMessage("Do not worry. You can Redo the level any time", 7f, 4f, 6f);
            WriteMessage("Press the Redo button up in the left", 8f, 5f, 20f);
            time -= 40f;
            messageCount += 10;
        }
    }
}