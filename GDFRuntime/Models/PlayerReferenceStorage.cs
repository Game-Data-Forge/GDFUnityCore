namespace GDFRuntime
{
    public class PlayerReferenceStorage
    {
        public string Reference { get; set; }
        public string Classname { get; set; }
        public int Count => GameSaves.Count;

        public GameSaves GameSaves = new GameSaves ();
    }
}