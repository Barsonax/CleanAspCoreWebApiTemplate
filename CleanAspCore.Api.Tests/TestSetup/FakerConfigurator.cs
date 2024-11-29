using Bogus;

namespace CleanAspCore.Api.Tests.TestSetup;

internal delegate Faker<T> FakerConfigurator<T>(Faker<T> t) where T : class;
