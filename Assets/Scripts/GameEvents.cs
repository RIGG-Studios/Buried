
public class GameEvents
{
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
}
