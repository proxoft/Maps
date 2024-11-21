namespace Proxoft.Maps.Core.Api.Shapes;

public interface IShape : IApiObject
{
    public void SetStyle(Style style);

    public void AddClass(params string[] classes);

    public void RemoveClass(params string[] classes);
}
