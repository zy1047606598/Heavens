﻿using Heavens.Core.Extension.PageQueayFilter.helper;
using System.Linq.Expressions;

namespace Heavens.Core.Extension.PageQueayFilter.common;

/// <summary>
/// 
/// </summary>
public class PageRequest
{
    /// <summary>
    /// 页码
    /// </summary>
    public int Page { get; set; } = 1;
    /// <summary>
    /// 每页大小
    /// </summary>
    public int PageSize { get; set; } = 50;
    /// <summary>
    /// 排序集合
    /// </summary>
    public SortBy Sort { get; set; } = new SortBy() { Field = "Id" };

    /// <summary>
    /// 查询条件组
    /// </summary>
    public ICollection<FilterRule> Filters { get; set; } = new List<FilterRule>();

    /// <summary>
    /// 获取查询条件表达式树
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Expression<Func<T, bool>> GetRulesExpression<T>(List<IQueryAction<T>>? queryActions = null)
    {
        return FilterHelper.GetExpression(Filters, queryActions);
    }
}