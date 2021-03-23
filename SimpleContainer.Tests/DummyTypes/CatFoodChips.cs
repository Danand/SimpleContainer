namespace SimpleContainer.Tests.DummyTypes
{
    public sealed class CatFoodChips : IPetFood
    {
        private readonly string name;

        public CatFoodChips(string name)
        {
            this.name = name;
        }
    }
}