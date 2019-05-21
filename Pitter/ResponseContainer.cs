namespace Pitter {
    public class ResponseContainer<T>
    {
        public T Response { get; set; }

        public static implicit operator T(ResponseContainer<T> item) => item.Response;
    }
}