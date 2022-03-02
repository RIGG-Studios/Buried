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

    public delegate void PlayAudio(string audio);
    public static PlayAudio OnPlayAudio;

    public delegate void ToggleFlashlight(bool state);
    public static ToggleFlashlight OnToggleFlashlight;

    public delegate void PlayerLootChest();
    public static PlayerLootChest OnSearchChest;

    public delegate void PlayerTakeItem(Item item);
    public static PlayerTakeItem OnPlayerTakeItem;
}
