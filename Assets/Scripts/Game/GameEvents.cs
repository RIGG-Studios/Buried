public class GameEvents
{
    public delegate void DropPlayer();
    public static DropPlayer OnDropPlayer;

    public delegate void StartGame();
    public static StartGame OnStartGame;

    public delegate void TentacleAttack(TentacleController controller);
    public static TentacleAttack OnTentacleAttackPlayer;

    public delegate void TentacleRetreat(TentacleController controller);
    public static TentacleRetreat OnTentacleRetreat;

    public delegate void PlayerSpawn();
    public static PlayerSpawn onPlayerSpawn;

    public delegate void PlayerDie();
    public static PlayerDie OnPlayerDie;

    public delegate void PlayerGetGrabbed(TentacleController controller);
    public static PlayerGetGrabbed OnPlayerGetGrabbed;

    public delegate void PlayAudio(string audio);
    public static PlayAudio OnPlayAudio;

    public delegate void NotePickedUp(int noteCount);
    public static NotePickedUp OnNotePickedUp;

    public delegate void ToggleRechargingStation(bool inside);
    public static ToggleRechargingStation OnToggleRechargingStation;
}
