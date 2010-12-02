using System;
using System.Reflection;
using Castle.Core.Logging;
using Castle.Core.Interceptor;
using DevText.Framework.Logging;

namespace DevText.Framework.Data
{
    public class ExceptionInterceptor:IInterceptor
    {
        private ILogger _logger;
        
        public void Intercept(IInvocation invocation)
        {
            _logger = Log4netLogger.Instance;

            try
            {
                _logger.Info("Befor call" + invocation.Method.Name + "method");
                invocation.Proceed();
                _logger.Info("After call" + invocation.Method.Name + "method");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw;
            }
        }
    }
}
