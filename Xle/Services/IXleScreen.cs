namespace ERY.Xle.Services
{
    public interface IXleScreen : IXleService
    {
        void Redraw();

        bool CurrentWindowClosed { get; }
    }
}
