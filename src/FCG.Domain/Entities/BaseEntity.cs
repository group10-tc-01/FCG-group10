namespace FCG.Domain.Entities
{
    public abstract class BaseEntity
    {
        protected BaseEntity() { }

        public Guid Id { get; protected set; } = Guid.NewGuid();
    }
}
