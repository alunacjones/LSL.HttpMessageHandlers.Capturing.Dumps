using System;
using System.Threading.Tasks;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Error logging executor
/// </summary>
public class ErrorLoggingExecutor
{
    /// <summary>
    /// Execute <paramref name="toExecute"/> and handle any exceptions with <paramref name="errorHandler"/>
    /// </summary>
    /// <param name="toExecute"></param>
    /// <param name="errorHandler"></param>
    /// <returns></returns>
    public async Task ExecuteAsyncWithErrorHandling(Func<Task> toExecute, Action<Exception> errorHandler)
    {
        try
        {
            await toExecute().ConfigureAwait(false);
        }
        catch  (Exception ex)
        {
            errorHandler(ex);
        }
    }

    /// <summary>
    /// Execute <paramref name="toExecute"/> and handle any exceptions with <paramref name="errorHandler"/>
    /// </summary>
    /// <param name="toExecute"></param>
    /// <param name="errorHandler"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public async Task<T> ExecuteAsyncWithErrorHandling<T>(Func<Task<T>> toExecute, Action<Exception> errorHandler, T defaultValue)
    {
        try
        {
            return await toExecute().ConfigureAwait(false);
        }
        catch  (Exception ex)
        {
            errorHandler(ex);
            return defaultValue;
        }
    }

    /// <summary>
    /// Execute <paramref name="toExecute"/> and handle any exceptions with <paramref name="errorHandler"/>
    /// </summary>
    /// <param name="toExecute"></param>
    /// <param name="errorHandler"></param>
    /// <returns></returns>
    public void ExecuteWithErrorHandling(Action toExecute, Action<Exception> errorHandler)
    {
        try
        {
            toExecute();
        }
        catch  (Exception ex)
        {
            errorHandler(ex);
        }        
    }

    /// <summary>
    /// Execute <paramref name="toExecute"/> and handle any exceptions with <paramref name="errorHandler"/>
    /// </summary>
    /// <param name="toExecute"></param>
    /// <param name="errorHandler"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>    
    public T ExecuteWithErrorHandling<T>(Func<T> toExecute, Action<Exception> errorHandler, T defaultValue)
    {
        try
        {
            return toExecute();
        }
        catch (Exception ex)
        {
            errorHandler(ex);
            return defaultValue;
        }
    }
}