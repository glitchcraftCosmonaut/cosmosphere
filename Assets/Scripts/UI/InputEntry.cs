using System;


[Serializable]
public class InputEntry
{
    public string playerName;
    public int scores;

    public InputEntry(string name, int scores)
    {
        playerName = name;
        this.scores = scores;
    }
}
