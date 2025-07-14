namespace TriviaSharp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        LocalTest.Main();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}