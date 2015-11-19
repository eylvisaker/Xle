namespace ERY.Xle.Services.Menus.Implementation
{
    public interface IXleSubMenuRedraw : IXleService
    {
        SubMenu Menu { get; set; }

        void Redraw();
    }
}