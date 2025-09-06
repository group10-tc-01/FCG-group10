namespace FCG.Domain
{
    public class Sonar
    {
        public Sonar(string name)
        {
            Name = name;
        }

        public string Name { get; private set; } = string.Empty;
    }
}
