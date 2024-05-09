using Bogus;

namespace CleanAspCore.Api.Tests.TestSetup;

public delegate Faker<T> FakerConfigurator<T>(Faker<T> t) where T : class;
