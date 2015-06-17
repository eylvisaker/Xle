namespace ERY.Xle.Services
{
    public interface ITextArea : IXleService
    {
        void Clear(bool setCursorAtTop = false);
    }
}
