namespace TriviaSharp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        Console.WriteLine("test");
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}