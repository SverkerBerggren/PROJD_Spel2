


public class GameActionPassPriority : GameAction
{
    public bool priority = false;
    public GameActionPassPriority()
    {
        Type = 15;
    }

    public GameActionPassPriority(bool priority)
    {
        Type = 15;
        this.priority = priority;
    }
}
