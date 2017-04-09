using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TestUtils
{
  public class RequestDataBuilder<T> : IAnonymousDataBuilder<T>
    where T : new()
  {
    private Dictionary<string, object> data = new Dictionary<string, object>();

    public RequestDataBuilder() { }

    public RequestDataBuilder(T template)
    {
      var props = template.GetType().GetProperties();

      foreach (var prop in props)
        data.Add(prop.Name, GetPropertyValue(template, prop.Name));
    }

    public Dictionary<string, object> Create() => data;

    public IAnonymousDataBuilder<T> Omit<TKey>(Expression<Func<T, TKey>> keySelector)
    {
      var propertyName = GetPropertyName(keySelector);

      if (data.ContainsKey(propertyName))
        data.Remove(propertyName);

      return this;
    }

    public IAnonymousDataBuilder<T> Set<TKey>(Expression<Func<T, TKey>> keySelector, object value)
    {
      var propertyName = GetPropertyName(keySelector);

      if (data.ContainsKey(propertyName))
        data[propertyName] = value;
      else
        data.Add(propertyName, value);

      return this;
    }

    private string GetPropertyName<TKey>(Expression<Func<T, TKey>> keySelector) =>
      (keySelector.Body as MemberExpression).Member.Name;

    public static object GetPropertyValue(object src, string propName) =>
      src.GetType().GetProperty(propName).GetValue(src, null);

  }
  public interface IAnonymousDataBuilder<T>
  {
    IAnonymousDataBuilder<T> Set<TKey>(Expression<Func<T, TKey>> keySelector, object value);
    IAnonymousDataBuilder<T> Omit<TKey>(Expression<Func<T, TKey>> keySelector);
    Dictionary<string, object> Create();
  }
}
