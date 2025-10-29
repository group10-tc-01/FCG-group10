namespace FCG.Domain.Services
{
    public interface ICorrelationIdProvider
    {
        string GetCorrelationId();
    }
}
