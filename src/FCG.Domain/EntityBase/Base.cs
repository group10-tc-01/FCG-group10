namespace FCG.Domain.EntityBase
{
    public class Base
    {
        public Guid Id { get; private set; }

        public Base()
        {
            Id = Guid.NewGuid();
        }
    }

}
