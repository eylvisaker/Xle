namespace ERY.Xle.Services.ScreenModel
{
    public interface IPlayerAnimator : IXleService
    {
        void AnimateStep();

        bool Animating { get; }

        int AnimFrame { get; }
    }
}
