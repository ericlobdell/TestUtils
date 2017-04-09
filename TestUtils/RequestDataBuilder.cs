using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TestUtils
{
  public class RequestDataBuilder<T> : IRequestDataBuilder<T>
    where T : new()
  {
    private Dictionary<string, object> data = new Dictionary<string, object>();

    public RequestDataBuilder() { }

    public RequestDataBuilder(T instance)
    {
      if (instance == null)
        throw new ArgumentException("Instance supplied cannot be null");

      foreach (var prop in instance.GetType().GetProperties())
        data.Add(prop.Name, GetPropertyValue(instance, prop.Name));
    }

    public IDictionary<string, object> Create() => data;

    public IRequestDataBuilder<T> Omit<TKey>(Expression<Func<T, TKey>> keySelector)
    {
      var propertyName = GetPropertyName(keySelector);

      if (data.ContainsKey(propertyName))
        data.Remove(propertyName);

      return this;
    }

    public IRequestDataBuilder<T> Set<TKey>(Expression<Func<T, TKey>> keySelector, object value)
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

    private object GetPropertyValue(T instance, string propName) =>
      instance.GetType().GetProperty(propName).GetValue(instance, null);

  }
  public interface IRequestDataBuilder<T>
  {
    IRequestDataBuilder<T> Set<TKey>(Expression<Func<T, TKey>> keySelector, object value);
    IRequestDataBuilder<T> Omit<TKey>(Expression<Func<T, TKey>> keySelector);
    IDictionary<string, object> Create();
  }
}
