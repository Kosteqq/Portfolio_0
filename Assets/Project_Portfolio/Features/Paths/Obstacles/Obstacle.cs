using ProjectPortfolio.Global;

namespace ProjectPortfolio.Paths
{
    internal class ObstacleHandle
    {
        public int Index = -1;
        public bool IsValid => Index != -1;
    }

    internal struct Obstacle
    {
        public readonly ObstacleHandle Handle;
        public readonly Bounds2D OccupiedBounds;

        public Obstacle(ObstacleHandle p_handle, Bounds2D p_occupiedBounds)
        {
            Handle = p_handle;
            OccupiedBounds = p_occupiedBounds;
        }
    }
}