namespace Pretzel.Core.Interfaces;

public interface ISettingProvider<T> where T : class
{
    T GetValue();
    Task<T> UpdateValueAsync(Action<T> updateAction);
}
