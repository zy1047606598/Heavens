﻿using Furion;
using Furion.DatabaseAccessor;
using Furion.UnifyResult;
using Heavens.Application.AuditApp.Dtos;
using Heavens.Core.Entities.Base;
using Heavens.Core.Extension.Extensions;
using Heavens.Core.Extension.QueayFilter;
using Heavens.Core.Extension.QueayFilter.common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

namespace Heavens.Application._Base;

#region Base
/// <summary>
/// 继承此类即可实现基础方法
/// 方法包括：CURD
/// </summary>
/// <typeparam name="TKey">数据实体主键类型</typeparam>
/// <typeparam name="TEntity">数据实体类型</typeparam>
/// <typeparam name="TEntityDto">数据实体Dto类型</typeparam>
/// <typeparam name="TPageEntity">Page 返回类型</typeparam>
[ApiDescriptionSettings(Order = 0)]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

public abstract class BaseAppService<TKey, TEntity, TEntityDto, TPageEntity> : IDynamicApiController
    where TEntity : class, IBaseEntity<TKey>, IPrivateEntity, new()
    where TEntityDto : class, new()
{
    protected IRepository<TEntity> _repository { get; }

    protected List<IQueryAction<TEntity>> _queryAcions;

    /// <summary>
    /// 继承此类即可实现基础方法
    /// 方法包括：CURD、获取全部、分页获取 
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="queryAcions"></param>
    protected BaseAppService(IRepository<TEntity> repository, List<IQueryAction<TEntity>> queryAcions = null)
    {
        this._repository = repository;
        this._queryAcions = queryAcions;
    }

    /// <summary>
    /// 获取分页
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PagedList<TPageEntity>> Page(PageRequest request)
    {
        var exp = request.GetRulesExpression(_queryAcions);

        var data = await _repository
            .Where(exp)
            .SortBy(request.Sort, _queryAcions)
            .Select(x => x.Adapt<TPageEntity>())
            .ToPagedListAsync(request.Page, request.PageSize);
        return data;
    }

    /// <summary>
    /// 根据过滤条件查询
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<TEntityDto>> GetByRequest(Request request)
    {
        var exp = request.GetRulesExpression(_queryAcions);

        var query = _repository
            .Where(exp)
            .SortBy(request.Sort, _queryAcions)
            .Select(x => x.Adapt<TEntityDto>());

        if (request.Limit > 0)
            query.Take(request.Limit);

        return await query.ToListAsync();
    }

    /// <summary>
    /// 获取所有
    /// </summary>
    /// <returns></returns>
    public virtual Task<List<TEntityDto>> GetAll()
    {
        return _repository.AsQueryable().Select(u => u.Adapt<TEntityDto>()).ToListAsync();
    }

    /// <summary>
    /// 根据Id查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual Task<TEntityDto> GetById([Required] TKey id)
    {
        return _repository.Where(r => r.Id.Equals(id)).Select(u => u.Adapt<TEntityDto>()).FirstOrDefaultAsync();
    }



    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="modelDto"></param>
    /// <returns></returns>
    public virtual async Task<TEntityDto> Add(TEntityDto modelDto)
    {
        TEntity model = modelDto.Adapt<TEntity>();
        model.SetCreateByHttpToken();
        return (await _repository.InsertNowAsync(model)).Entity.Adapt<TEntityDto>();
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="modelDto"></param>
    /// <returns></returns>
    public virtual async Task<TEntityDto> Update(TEntityDto modelDto)
    {
        TEntity model = modelDto.Adapt<TEntity>();
        model.SetUpdateByHttpToken();
        return (await _repository.UpdateNowAsync(model)).Entity.Adapt<TEntityDto>();
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns>被删除的实体主键</returns>
    [HttpDelete]
    public virtual Task DeleteById([Required] TKey id)
    {
        return _repository.DeleteNowAsync(id);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns>删除数量</returns>
    [HttpDelete]
    public virtual Task<int> DeleteByIds([FromBody] TKey[] ids)
    {
        if (ids.IsEmpty())
        {
            throw Oops.Oh(Excode.REQUEST_DATA_EMPTY);
        }
        foreach (TKey id in ids)
            _repository.Delete(id);

        return _repository.SaveNowAsync();
    }

}


/// <summary>
/// 继承此类即可实现基础方法
/// 方法包括：CURD
/// </summary>
/// <typeparam name="TEntity">数据实体类型</typeparam>
/// <typeparam name="TEntityDto">数据实体类型</typeparam>
public abstract class BaseAppService<TEntity, TEntityDto> : BaseAppService<int, TEntity, TEntityDto, TEntityDto>
    where TEntity : class, IBaseEntity, IPrivateEntity, new()
    where TEntityDto : class, new()
{
    protected BaseAppService(IRepository<TEntity> repository, List<IQueryAction<TEntity>> queryAcions = null) : base(repository, queryAcions)
    {
    }
}

/// <summary>
/// 继承此类即可实现基础方法
/// 方法包括：CURD
/// </summary>
/// <typeparam name="TEntity">数据实体类型</typeparam>
public abstract class BaseAppService<TEntity> : BaseAppService<int, TEntity, TEntity, TEntity>
    where TEntity : class, IBaseEntity, IPrivateEntity, new()
{
    protected BaseAppService(IRepository<TEntity> repository, List<IQueryAction<TEntity>> queryAcions = null) : base(repository, queryAcions)
    {
    }
}

#endregion
