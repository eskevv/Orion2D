namespace Orion2D;

public class DroneSpawner : Script {

   private float _spawnTimer;
   private float _spawnInterval;

   public override void Start()
   {
      _spawnInterval = 5f;
   }

   public override void Update(float deltaTime)
   {
      _spawnTimer += deltaTime;

      if (_spawnTimer >= _spawnInterval && _spawnInterval != 0 && CoreGame.Registry.TotalEntities < 300)
      {
         _spawnTimer = 0;
         Factory.CreateSpaceDrone(false, "enemy-space-02", true);
      }
   }
}
