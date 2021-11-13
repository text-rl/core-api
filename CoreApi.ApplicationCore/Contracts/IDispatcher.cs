namespace CoreApi.ApplicationCore.Contracts
{
    public interface IDispatcher<TMessage>
    {
        public void Dispatch(TMessage message);
    }
}