namespace JFrame.Game.View
{
    public abstract class SceneBaseState
    {
        public abstract string Name { get; }

        public SceneController Owner { get; set; }
    }
}

