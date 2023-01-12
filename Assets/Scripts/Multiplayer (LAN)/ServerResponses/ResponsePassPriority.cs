
public class ResponsePassPriority : ServerResponse
{
    public bool priority = false;
    public ResponsePassPriority()
    {
        Type = 14;
    }
    public ResponsePassPriority(bool priority)
    {
        Type = 14;

        this.priority = priority;
    }
}