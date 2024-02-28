﻿using Blog.Domain;
using Xunit.Sdk;

namespace Blog.UnitTest.Domain.Entities;

public class BaseEntityTests
{
    [Fact]
    public void BaseEntity_IdGenerated_WhenNullIdProvided()
    {
        Guid? id = null;

        var entity = new ConcreteEntity(id);

        Assert.NotEqual(Guid.Empty, entity.Id);
        Assert.NotNull(entity.Id);
    }

    [Fact]
    public void BaseEntity_IdGenerated_WhenEmptyIdProvided()
    {
        var entity = new ConcreteEntity();

        Assert.NotEqual(Guid.Empty, entity.Id);
        Assert.NotNull(entity.Id);
    }

    [Fact]
    public void BaseEntity_Validate_IdGenerated_WhenEmptyGuidProvided()
    {
        var guid = Guid.Empty;
        var entity = new ConcreteEntity(guid);

        entity.Validate();

        Assert.NotEqual(Guid.Empty, entity.Id);
        Assert.NotNull(entity.Id);
    }

    [Fact]
    public void BaseEntity_IdSet_WhenIdProvided()
    {
        Guid id = Guid.NewGuid();

        var entity = new ConcreteEntity(id);

        Assert.Equal(id, entity.Id);
    }

}

//Test purpose class
public class ConcreteEntity : BaseEntity
{
    public ConcreteEntity(Guid? id) : base(id) { }
    public ConcreteEntity() : base() { }

}