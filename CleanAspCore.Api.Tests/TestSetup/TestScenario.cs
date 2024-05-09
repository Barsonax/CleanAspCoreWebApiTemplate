namespace CleanAspCore.Api.Tests.TestSetup;

public record TestScenario<TInput>
{
    public TInput Input { get; }
    private string Name { get; }

    public TestScenario(string name, TInput input)
    {
        Name = name;
        Input = input;
    }

    public override string ToString() => Name;
}
