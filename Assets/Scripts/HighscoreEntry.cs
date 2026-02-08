[System.Serializable]
public struct HighScoreEntry
{
    public string name;
    public int points;

    public HighScoreEntry(string name, int points)
    {
        this.name = name;
        this.points = points;
    }
}
