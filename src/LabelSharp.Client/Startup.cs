﻿using Autofac;
using Caliburn.Micro;
using LabelSharp.ViewModels.HomeContext;
using SD.Common;
using SD.Infrastructure.WPF.Caliburn.Extensions;
using SD.IOC.Core.Extensions;
using SD.IOC.Core.Mediators;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace LabelSharp
{
    /// <summary>
    /// Caliburn启动器
    /// </summary>
    public class Startup : BootstrapperBase
    {
        #region # 构造器

        #region 00.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public Startup()
        {
            this.Initialize();
        }
        #endregion

        #endregion

        #region # 事件

        #region 应用程序启动事件 —— override void OnStartup(object sender...
        /// <summary>
        /// 应用程序启动事件
        /// </summary>
        protected override async void OnStartup(object sender, StartupEventArgs e)
        {
            //启动屏幕
            SplashScreen splashScreen = new SplashScreen("Content/Images/SD.png");
            splashScreen.Show(true);

            await base.DisplayRootViewForAsync<IndexViewModel>();
        }
        #endregion

        #region 应用程序异常事件 —— override void OnUnhandledException(object sender...
        /// <summary>
        /// 应用程序异常事件
        /// </summary>
        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs eventArgs)
        {
            Exception exception = eventArgs.Exception;
            eventArgs.Handled = true;

            //释放遮罩
            BusyExtension.GlobalIdle();

            //提示消息
            MessageBox.Show(exception.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);

            //记录日志
            WriteLog(exception);
        }
        #endregion

        #region 应用程序退出事件 —— override void OnExit(object sender...
        /// <summary>
        /// 应用程序退出事件
        /// </summary>
        protected override void OnExit(object sender, EventArgs e)
        {
            ResolveMediator.Dispose();
        }
        #endregion

        #endregion

        #region # 方法

        #region 配置应用程序 —— override void Configure()
        /// <summary>
        /// 配置应用程序
        /// </summary>
        protected override void Configure()
        {
            //初始化依赖注入容器
            if (!ResolveMediator.ContainerBuilt)
            {
                ContainerBuilder containerBuilder = ResolveMediator.GetContainerBuilder();
                containerBuilder.RegisterConfigs();
                ResolveMediator.Build();
            }
        }
        #endregion

        #region 解析服务实例 —— override object GetInstance(Type service...
        /// <summary>
        /// 解析服务实例
        /// </summary>
        /// <param name="service">服务类型</param>
        /// <param name="key">键</param>
        /// <returns>服务实例</returns>
        protected override object GetInstance(Type service, string key)
        {
            object instance = ResolveMediator.Resolve(service);
            return instance;
        }
        #endregion

        #region 解析服务实例列表 —— override IEnumerable<object> GetAllInstances(Type service)
        /// <summary>
        /// 解析服务实例列表
        /// </summary>
        /// <param name="service">服务类型</param>
        /// <returns>服务实例列表</returns>
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            IEnumerable<object> instances = ResolveMediator.ResolveAll(service);
            return instances;
        }
        #endregion

        #region 记录日志 —— static void WriteLog(Exception exception)
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="exception">异常</param>
        private static void WriteLog(Exception exception)
        {
            string exceptionLogPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\ExceptionLogs\\{{0:yyyy-MM-dd}}.txt";
            Task.Run(() =>
            {
                FileExtension.WriteFile(string.Format(exceptionLogPath, DateTime.Today),
                    "===================================WPF运行异常, 详细信息如下==================================="
                    + Environment.NewLine + "［异常时间］" + DateTime.Now
                    + Environment.NewLine + "［异常消息］" + exception.Message
                    + Environment.NewLine + "［异常明细］" + exception
                    + Environment.NewLine + "［内部异常］" + exception.InnerException
                    + Environment.NewLine + "［堆栈信息］" + exception.StackTrace
                    + Environment.NewLine + Environment.NewLine, true);
            });
        }
        #endregion

        #endregion
    }
}
