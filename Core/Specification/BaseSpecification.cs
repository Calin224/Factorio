using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ObjectiveC;
using Core.Interfaces;

namespace Core.Specification;

public class BaseSpecification<T>(Expression<Func<T, bool>>? criteria) : ISpecification<T>
{
    public BaseSpecification(): this(null)
    {
    }

    public Expression<Func<T, bool>>? Criteria => criteria;
    public List<Expression<Func<T, object>>> Includes { get; private set; } = [];
    public Expression<Func<T, object>>? OrderBy {get; private set;}
    public Expression<Func<T, object>>? OrderByDescending {get; private set;}

    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
    {
        OrderByDescending = orderByDescExpression;
    }
}
