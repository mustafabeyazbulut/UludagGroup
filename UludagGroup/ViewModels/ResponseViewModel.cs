namespace UludagGroup.ViewModels
{
    public class ResponseViewModel<T> where T : new()
    {
        public bool Status { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public T Data { get; set; } = new T();
    }
}
