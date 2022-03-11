using Bing.Date;
using Serilog;
using Serilog.Events;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args).Inject();


#region Log ����
builder.Host.UseSerilogDefault(config =>//Ĭ�ϼ����� ����̨ �� �ļ� ��ʽ�������Զ���д�룬������Ҫд��Ľ��ʼ��ɣ�
{
    string date = DateTime.Now.ToString("yyyy-MM-dd");//��ʱ�䴴���ļ���
    string outputTemplate = "{NewLine}��{Level:u3}��{Timestamp:yyyy-MM-dd HH:mm:ss.fff}" +
    "{NewLine}#Msg#{Message:lj}" +
    "{NewLine}#Pro #{Properties:j}" +
    "{NewLine}#Exc#{Exception}" +
    new string('-', 50) + "{NewLine}";//���ģ��

    ///1.�������restrictedToMinimumLevel��LogEventLevel����
    config
        //.MinimumLevel.Debug() // ����Sink����С��¼����
        //.MinimumLevel.Override("Microsoft", LogEventLevel.Fatal)
        //.Enrich.FromLogContext()
        .WriteTo.Console(outputTemplate: outputTemplate)
        .WriteTo.File($"log/{LogEventLevel.Information}/{date}.log",
               outputTemplate: outputTemplate,
                restrictedToMinimumLevel: LogEventLevel.Information,
                rollOnFileSizeLimit: true,          // ���Ƶ����ļ�����󳤶�
                retainedFileCountLimit: 50,         // ��󱣴��ļ���,����nullʱ��Զ�����ļ���
                fileSizeLimitBytes: 1024 * 1024,      // ��󵥸��ļ���С
                encoding: Encoding.UTF8            // �ļ��ַ�����
            )

    #region 2.��LogEventLevel.�����������/���ļ�

        // Debug 
        .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Debug)//ɸѡ����
            .WriteTo.File($"log/{LogEventLevel.Debug}/{date}.log",
                outputTemplate: outputTemplate,
                encoding: Encoding.UTF8            // �ļ��ַ�����
             )
        )
        // Warning 
        .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Warning)//ɸѡ����
            .WriteTo.File($"log/{LogEventLevel.Warning}/{date}.log",
                outputTemplate: outputTemplate,
                encoding: Encoding.UTF8            // �ļ��ַ�����
             )
        )
        // Error 
        .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Error)//ɸѡ����
            .WriteTo.File($"log/{LogEventLevel.Error}/{date}.log",
                outputTemplate: outputTemplate,
                encoding: Encoding.UTF8            // �ļ��ַ�����
             )
        );

    #endregion ��LogEventLevel ��������/���ļ�


});
#endregion

var app = builder.Build();
app.Run();
