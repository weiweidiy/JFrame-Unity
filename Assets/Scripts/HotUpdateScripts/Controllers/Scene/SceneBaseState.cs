namespace JFrame.Game.HotUpdate
{
    public abstract class SceneBaseState
    {
        public abstract string Name { get; }

        public SceneController Owner { get; set; }
    }
}

