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
        public readonly GridBounds OccupiedBounds;

        public Obstacle(ObstacleHandle p_handle, GridBounds p_occupiedBounds)
        {
            Handle = p_handle;
            OccupiedBounds = p_occupiedBounds;
        }
    }
}