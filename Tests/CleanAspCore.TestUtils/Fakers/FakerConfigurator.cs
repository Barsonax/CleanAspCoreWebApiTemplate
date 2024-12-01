using Bogus;

namespace CleanAspCore.Api.TestUtils.Fakers;

public delegate Faker<T> FakerConfigurator<T>(Faker<T> t) where T : class;
