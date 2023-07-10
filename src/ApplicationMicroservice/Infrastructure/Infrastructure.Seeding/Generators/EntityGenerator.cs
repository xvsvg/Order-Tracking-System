namespace Infrastructure.Seeding.Generators;

public static class EntityGenerator<T> where T : class
{
    public static T Generate()
    {
        return FakerGenerator.GeneratorOfType<T>().Generate();
    }

    public static ICollection<T> Generate(int count)
    {
        return FakerGenerator.GeneratorOfType<T>().Generate(count);
    }
}