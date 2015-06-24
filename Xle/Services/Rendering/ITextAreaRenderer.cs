using ERY.Xle.Services.ScreenModel;

namespace ERY.Xle.Services.Rendering
{
    public interface ITextAreaRenderer : IXleService
    {
        void Draw(ITextArea TextArea);
    }
}
