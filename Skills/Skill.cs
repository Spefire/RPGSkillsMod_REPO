using System.Collections.Generic;

public class Skill
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> Properties { get; set; }

    public float Cooldown { get; set; }

    public float ActiveDuration { get; protected set; } = 2f;

    public Skill()
    {
        Name = "No skill";
        Description = "";
        Properties = new List<string>();
        Cooldown = 0f;
    }

    public virtual bool Execute() { return true; }

    public virtual void Update() { }
}