using Game.SeniorEventBus.Signals;

public interface IHostageSaver
{ 
   public int HostageToSave { get; set; }
   public int HostageSaved { get; set; }
   public void Save(SaveHostage hostage);
}
