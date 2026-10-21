namespace Core.Database;

using System.Data.Common;

internal interface IDatabaseAccess<T>
{
    public T ExecuteInsert(string sqlKey, T entity);
    public List<T> ExecuteSelect(string sqlKey);
    public List<T> ExecuteUpdate(string sqlKey);
    public List<T> ExecuteDelete(string sqlKey);
}
