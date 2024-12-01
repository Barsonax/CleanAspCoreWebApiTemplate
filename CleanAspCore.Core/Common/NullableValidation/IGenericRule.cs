﻿using FluentValidation;

namespace CleanAspCore.Core.Common.NullableValidation;

internal interface IGenericRule
{
    public void ApplyRule<T, TProperty>(IRuleBuilderInitial<T, TProperty> builder);
}