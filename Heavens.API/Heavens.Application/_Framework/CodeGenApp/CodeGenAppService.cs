﻿using Furion;
using Heavens.Application._Framework.CodeGenApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace Heavens.Application._Framework.CodeGenApp;

#if DEBUG
/// <summary>
/// 根据后端实体生成代码
/// </summary>
[ApiDescriptionSettings("Framework")]
public class CodeGenAppService : IDynamicApiController
{
    public CodeGenAppService(ICodeGenService codeGenService)
    {
        _codeGenService = codeGenService;
    }

    public ICodeGenService _codeGenService { get; }

    /// <summary>
    /// 根据实体生成Application代码
    /// </summary>
    /// <param name="path"></param>
    [HttpGet]
    public void GenApplications(string path = null)
    {
        _codeGenService.GenApplication(path);
    }

    /// <summary>
    /// 根据Application 生成Vue Api代码
    /// </summary>
    /// <param name="path"></param>
    [HttpGet]
    public void GenVueApi(string path = null)
    {
        _codeGenService.GenVueApi(path);

    }

    /// <summary>
    /// 根据Application 生成Vue Page代码
    /// </summary>
    /// <param name="path"></param>
    [HttpGet]
    public void GenVuePages(string path = null)
    {
        _codeGenService.GenVuePage(path);

    }

}
#endif
