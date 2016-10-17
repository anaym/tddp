using TagsCloudVisualization.Geometry;

namespace TagsCloudVisualization.Layers
{
    public interface ILayer
    {
        Vector CurrentPosition { get; }
        bool IsCompleted { get; }
        Rectangle Put(Size size);
    }
}