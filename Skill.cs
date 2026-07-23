using System.Collections.Generic;

public class Skill
{
    public string Name { get; }
    public string Description { get; }
    public List<string> Properties { get; }

    public float Cooldown { get; }

    public Skill(string name, string description, List<string> properties, float cooldown)
    {
        Name = name;
        Description = description;
        Properties = properties;
        Cooldown = cooldown;
    }
}