namespace ToDoList.Test;

using System;
using Microsoft.AspNetCore.Mvc;

public static class ActionResultExtensions
{
    public static T? GetValue<T>(this ActionResult<T> result)
    {
        if (result.Result == null)
            return result.Value;

        var objectResult = result.Result as ObjectResult;
        if (objectResult != null)
        {
            if (objectResult.Value is T t)
                return t;
            else
            {
                throw new InvalidCastException($"Cannot cast ObjectResult.Value of type {objectResult.Value?.GetType().FullName} to {typeof(T).FullName}");
            }
        }
        return default;
    }

    public static T? GetOther<T, U>(this ActionResult<U> result)
    {
        var objectResult = result.Result as ObjectResult;
        if (objectResult != null)
        {
            if (objectResult.Value is T t)
                return t;
            else
            {
                throw new InvalidCastException($"Cannot cast ObjectResult.Value of type {objectResult.Value?.GetType().FullName} to {typeof(T).FullName}");
            }
        }
        return default;
    }
}
