using System.Collections.Generic;

public class Skill
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> Properties { get; set; }

    public float Cooldown { get; set; }

    public Skill()
    {
        Name = "No skill";
        Description = "";
        Properties = new List<string>();
        Cooldown = 0f;
    }

    public virtual void Execute() { }

    public virtual void Update() { }
}