using CleanAspCore.Data.Models;

namespace CleanAspCore.Api.Tests.Data;

public class EntityTests
{
    private class TestEntity : Entity;

    [Test]
    public void EntityWithDifferentId_OpEqual_ReturnsFalse()
    {
        var entity1 = new TestEntity { Id = Guid.NewGuid() };
        var entity2 = new TestEntity { Id = Guid.NewGuid() };

        (entity1 == entity2).Should().BeFalse();
    }

    [Test]
    public void EntityWithSameId_OpEqual_ReturnsTrue()
    {
        var id = Guid.NewGuid();
        var entity1 = new TestEntity { Id = id };
        var entity2 = new TestEntity { Id = id };

        (entity1 == entity2).Should().BeTrue();
    }

    [Test]
    public void EntitySameInstance_OpEqual_ReturnsTrue()
    {
        var id = Guid.NewGuid();
        var entity1 = new TestEntity { Id = id };
        var entity2 = entity1;

        (entity1 == entity2).Should().BeTrue();
    }

    [Test]
    public void EntityAndNull_OpEqual_ReturnsFalse()
    {
        var entity1 = new TestEntity { Id = Guid.NewGuid() };
        TestEntity entity2 = null!;

#pragma warning disable CA1508
        (entity1 == entity2).Should().BeFalse();
#pragma warning restore CA1508
    }

    [Test]
    public void NullAndNull_OpEqual_ReturnsTrue()
    {
        TestEntity entity1 = null!;
        TestEntity entity2 = null!;

#pragma warning disable CA1508
        (entity1 == entity2).Should().BeTrue();
#pragma warning restore CA1508
    }

    [Test]
    public void EntityWithDifferentId_OpNotEqual_ReturnsFalse()
    {
        var entity1 = new TestEntity { Id = Guid.NewGuid() };
        var entity2 = new TestEntity { Id = Guid.NewGuid() };

        (entity1 != entity2).Should().BeTrue();
    }

    [Test]
    public void EntityWithSameId_OpNotEqual_ReturnsTrue()
    {
        var id = Guid.NewGuid();
        var entity1 = new TestEntity { Id = id };
        var entity2 = new TestEntity { Id = id };

        (entity1 != entity2).Should().BeFalse();
    }
}

