namespace Pretzel.Core.Interfaces;

public interface ISettingProvider<T> where T : class, ICloneable, new()
{
    T Value { get;  }
    Task<T> UpdateValueAsync(Action<T> updateAction);
}
