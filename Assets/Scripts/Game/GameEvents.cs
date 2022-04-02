public class GameEvents
{
    public delegate void StartGame();
    public static StartGame OnStartGame;

    public delegate void TentacleAttack(TentacleController controller);
    public static TentacleAttack OnTentacleAttackPlayer;

    public delegate void TentacleRetreat(TentacleController controller);
    public static TentacleRetreat OnTentacleRetreat;

    public delegate void DropPlayer();
    public static DropPlayer OnDropPlayer;

    public delegate void PlayerSpawn();
    public static PlayerSpawn onPlayerSpawn;

    public delegate void PlayerDie();
    public static PlayerDie OnPlayerDie;

    public delegate void PlayerGetGrabbed(TentacleController controller);
    public static PlayerGetGrabbed OnPlayerGetGrabbed;

    public delegate void PlayAudio(string audio);
    public static PlayAudio OnPlayAudio;

    public delegate void GeneratorTurnedOn(int generatorCount);
    public static GeneratorTurnedOn OnGeneratorTurnedOn;

    public delegate void ToggleRechargingStation(bool inside);
    public static ToggleRechargingStation OnToggleRechargingStation;

    public delegate void ToggleHidePlayer();
    public static ToggleHidePlayer OnToggleHidePlayer;
} 
