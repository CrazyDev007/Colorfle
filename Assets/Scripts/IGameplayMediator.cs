public interface IGameplayMediator
{
    void Notify(object sender, GameplayEvent eventCode, object selectedColor = null);
}

public enum GameplayEvent
{
    PaletteColorClicked,
}