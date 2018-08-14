namespace SimpleContainer.Tests.DummyTypes
{
    public class ColorPalette : IColorPalette
    {
        private readonly IColor[] colors;

        public ColorPalette(IColor[] colors)
        {
            this.colors = colors;
        }

        IColor[] IColorPalette.Colors
        {
            get { return colors; }
        }
    }
}